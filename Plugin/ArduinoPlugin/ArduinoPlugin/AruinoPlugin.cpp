//
//  AruinoPlugin.cpp
//  ArduinoPlugin
//
//  Created by James Carr on 22/10/2016.
//  Copyright © 2016 Teario. All rights reserved.
//

#include "AruinoPlugin.hpp"
#include "RemoteDevice.hpp"
#include <iostream>
#include <stdio.h>
#include <unistd.h>

char* CopyString( const char* pOriginal )
{
    size_t length = strlen( pOriginal ) + 1;
    
    char* pCopy = new char[length];
    memcpy( pCopy, pOriginal, length );
    
    return pCopy;
}

RemoteDevice* m_pRemoteDevice = NULL;

bool CreateRemoteDevice( char* pAddress )
{
    char* pAddressCopy = CopyString( pAddress );
    if( m_pRemoteDevice == NULL )
    {
        m_pRemoteDevice = new RemoteDevice( pAddressCopy );
        return true;
    }
    
    return false;
}

bool BeginDeviceIdentification()
{
    if( m_pRemoteDevice != NULL )
    {
        m_pRemoteDevice->BeginIdentification();
        return true;
    }
    
    return false;
}

bool TriggerDevice()
{
    if( m_pRemoteDevice != NULL )
    {
        m_pRemoteDevice->Trigger();
        return true;
    }
    
    return false;
}

bool IsDeviceIdentified()
{
    if( m_pRemoteDevice != NULL )
    {
        return m_pRemoteDevice->IsIdentified();
    }
    
    return false;
}

void DestroyDevice()
{
    if( m_pRemoteDevice != NULL )
    {
        delete m_pRemoteDevice;
        m_pRemoteDevice = NULL;
    }
}