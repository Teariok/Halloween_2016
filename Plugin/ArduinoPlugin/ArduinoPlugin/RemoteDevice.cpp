//
//  RemoteDevice.cpp
//  ArduinoPlugin
//
//  Created by James Carr on 22/10/2016.
//  Copyright © 2016 Teario. All rights reserved.
//

#include "RemoteDevice.hpp"
#include <iostream>
#include <stdio.h>
#include <fcntl.h>
#include <termios.h>
#include <unistd.h>

RemoteDevice::RemoteDevice() : m_pAddress( NULL )
{
    Reset();
}

RemoteDevice::RemoteDevice( char* pAddress ) : m_pAddress( pAddress )
{
    Reset();
}

RemoteDevice::~RemoteDevice()
{
    if( m_CommsHandle >= 0 )
    {
        EndCommunication();
    }
    
    if( m_pAddress != NULL )
    {
        delete m_pAddress;
        m_pAddress = NULL;
    }
}

void RemoteDevice::Reset()
{
    m_IsIdentified = false;
    m_PendingTriggers = 0;
    m_CommsHandle = -1;
    m_ActiveThread = -1;
}

void RemoteDevice::BeginIdentification()
{
    if( m_ActiveThread < 0 )
    {
        BeginCommunication();
        
        pthread_t identifierThread;
        m_ActiveThread = pthread_create( &identifierThread, NULL, StartIdenficationThread, this );
    }
}

bool RemoteDevice::IsIdentified()
{
    bool isIdentified = false;
    
    pthread_mutex_lock( &m_IdentifierMutex );
    isIdentified = m_IsIdentified;
    pthread_mutex_unlock( &m_IdentifierMutex );
    
    return m_IsIdentified;
}

void RemoteDevice::Trigger()
{
    pthread_mutex_lock( &m_TriggerMutex );
    ++m_PendingTriggers;
    pthread_mutex_unlock( &m_TriggerMutex );
    
    if( m_ActiveThread < 0 )
    {
        pthread_t triggerThread;
        pthread_create( &triggerThread, NULL, StartTriggerProcessingThread, this );
    }
}

bool RemoteDevice::BeginCommunication()
{
    if( m_pAddress != NULL && !m_IsIdentified && m_CommsHandle < 0 )
    {
        m_CommsHandle = open( m_pAddress, O_RDWR | O_NOCTTY | O_NDELAY );
        if( m_CommsHandle < 0 )
        {
            return false;
        }
        
        struct termios options;
        
        tcgetattr(m_CommsHandle, &options);
        
        cfsetispeed(&options, B9600);
        cfsetospeed(&options, B9600);
        
        options.c_cflag &= ~PARENB;
        options.c_cflag &= ~CSTOP;
        options.c_cflag &= ~CSIZE;
        options.c_cflag |= CS8;
        options.c_cflag |= (CLOCAL | CREAD);
        
        tcsetattr(m_CommsHandle, TCSANOW, &options);
        
        return true;
    }
    
    return false;
}

void RemoteDevice::EndCommunication()
{
    close( m_CommsHandle );
    m_CommsHandle = -1;
}

void* RemoteDevice::StartIdenficationThread( void* pDevice )
{
    ((RemoteDevice*)pDevice)->StartIdentification();
    return NULL;
}

void RemoteDevice::StartIdentification()
{
    const int MAX_ATTEMPTS = 2;
    const int READ_BUFFER_SIZE = 32;
    const int READ_DELAY_TIME = 2;
    const char IDENTIFICATION_REQUEST = 0xFF;
    const char IDENTIFICATION_RESPONSE[] = {'1','7','0'};
    
    char* pBuff = new char[ READ_BUFFER_SIZE ];
    
    for( int i = 0; i < MAX_ATTEMPTS; ++i )
    {
        sleep( READ_DELAY_TIME );
        
        write( m_CommsHandle, &IDENTIFICATION_REQUEST, sizeof(IDENTIFICATION_REQUEST) );
        
        sleep( READ_DELAY_TIME );
        
        memset( pBuff, '\0', READ_BUFFER_SIZE );
        if( read(m_CommsHandle, pBuff, READ_BUFFER_SIZE) )
        {
            int result = strncmp( pBuff, IDENTIFICATION_RESPONSE, strlen(IDENTIFICATION_RESPONSE) );
            if( result == 0 )
            {
                pthread_mutex_lock( &m_IdentifierMutex );
                m_IsIdentified = true;
                m_ActiveThread = -1;
                pthread_mutex_unlock( &m_IdentifierMutex );
                
                break;
            }
        }
    }
    
    delete[] pBuff;
}

void* RemoteDevice::StartTriggerProcessingThread( void *pDevice )
{
    ((RemoteDevice*)pDevice)->StartTriggerProcessing();
    return NULL;
}

void RemoteDevice::StartTriggerProcessing()
{
    const int WRITE_BUFFER_SIZE = 31;
    const char TRIGGER_REQUEST = 0xF0;
    char* pBuff = new char[ WRITE_BUFFER_SIZE+1 ];
    const int TRIGGER_INTERVAL = 1;
    
    while( m_CommsHandle >= 0 )
    {
        sleep( TRIGGER_INTERVAL );
        
        pthread_mutex_lock( &m_TriggerMutex );
        int queueLength = m_PendingTriggers;
        m_PendingTriggers = 0;
        pthread_mutex_unlock( &m_TriggerMutex );
        
        while( queueLength > 0 )
        {
            int reduction = queueLength > WRITE_BUFFER_SIZE ? WRITE_BUFFER_SIZE : queueLength;
            memset( pBuff, TRIGGER_REQUEST, reduction );
            pBuff[ WRITE_BUFFER_SIZE+1 ] = '\0';
            write( m_CommsHandle, pBuff, reduction );
            
            queueLength -= reduction;
            sleep( TRIGGER_INTERVAL );
        }
    }
    
    delete[] pBuff;
}