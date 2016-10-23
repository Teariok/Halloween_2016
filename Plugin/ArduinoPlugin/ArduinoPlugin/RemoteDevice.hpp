//
//  RemoteDevice.hpp
//  ArduinoPlugin
//
//  Created by James Carr on 22/10/2016.
//  Copyright © 2016 Teario. All rights reserved.
//

#ifndef RemoteDevice_hpp
#define RemoteDevice_hpp

#include <pthread.h>

class RemoteDevice
{
    private:
        char* m_pAddress;
        bool m_IsIdentified;
        pthread_mutex_t m_IdentifierMutex;
        pthread_mutex_t m_TriggerMutex;
        int m_PendingTriggers;
        int m_CommsHandle;
        int m_ActiveThread;
    
    public:
        RemoteDevice();
        RemoteDevice( char* pAddress );
        ~RemoteDevice();
    
        void BeginIdentification();
        void Trigger();
    
        bool IsIdentified();
    
    private:
        void Reset();
        void IdentifyDevice();
        bool BeginCommunication();
        void EndCommunication();
    
        static void* StartIdenficationThread( void* pDevice );
        void StartIdentification();
    
        static void* StartTriggerProcessingThread( void* pDevice );
        void StartTriggerProcessing();
};

#endif /* RemoteDevice_hpp */
