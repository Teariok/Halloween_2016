//
//  AruinoPlugin.cpp
//  ArduinoPlugin
//
//  Created by James Carr on 22/10/2016.
//  Copyright © 2016 Teario. All rights reserved.
//

#include "AruinoPlugin.hpp"
#include <iostream>
#include <stdio.h>
#include <unistd.h>
#include <iostream>
#include <fcntl.h>
#include <termios.h>

int m_CommsHandle;

bool OpenSerialPort( char* pAddress )
{
    m_CommsHandle = open( pAddress, O_RDWR | O_NOCTTY | O_NDELAY );
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

void CloseSerialPort()
{
    if( m_CommsHandle >= 0 )
    {
        close( m_CommsHandle );
        m_CommsHandle = -1;
    }
}