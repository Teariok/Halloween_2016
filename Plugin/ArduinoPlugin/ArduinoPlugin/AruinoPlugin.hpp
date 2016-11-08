//
//  AruinoPlugin.hpp
//  ArduinoPlugin
//
//  Created by James Carr on 22/10/2016.
//  Copyright © 2016 Teario. All rights reserved.
//

#ifndef AruinoPlugin_hpp
#define AruinoPlugin_hpp

extern "C" {
    bool OpenSerialPort( char* pAddress );
    void CloseSerialPort();
}

#endif /* AruinoPlugin_hpp */
