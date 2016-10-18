#include <Servo.h>

Servo m_Servo;

const int SERVO_PIN = 8;
const int SERIAL_BAUD = 9600;

const int SERVO_OPEN_VAL = 180;
const int SERVO_CLOSED_VAL = 0;
const int TRANSITION_DELAY_TIME = 400;
const int OPEN_HOLD_TIME = 100;

void setup()
{
	m_Servo.attach( SERVO_PIN );
	m_Servo.write( SERVO_CLOSED_VAL );
	
	Serial.begin( SERIAL_BAUD );
}

void loop()
{
	if( Serial.available() )
	{
		m_Servo.write( SERVO_OPEN_VAL );
		delay( TRANSITION_DELAY_TIME );
		
		while( Serial.available() )
		{
			delay( OPEN_HOLD_TIME );
			int data = Serial.read();
			Serial.println( data );
		}
		
		m_Servo.write( SERVO_CLOSED_VAL );
		delay( TRANSITION_DELAY_TIME );
	}
}