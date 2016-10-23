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
    bool CreateRemoteDevice( char* pAddress );
    bool BeginDeviceIdentification();
    bool TriggerDevice();
    bool IsDeviceIdentified();
    void DestroyDevice();
}

#endif /* AruinoPlugin_hpp */
