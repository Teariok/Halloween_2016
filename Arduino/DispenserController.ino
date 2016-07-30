const int OUT_PIN = 13;

void setup()
{
	Serial.begin( 9600 );
	pinMode( OUT_PIN, OUTPUT );
}

void loop()
{
	if( Serial.available() )
	{
		int data = Serial.read();
		Serial.println(data);
		digitalWrite( OUT_PIN, HIGH );
		delay( 100 );
		digitalWrite( OUT_PIN, LOW );
		delay( 100 );
	}
}