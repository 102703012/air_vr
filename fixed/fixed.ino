#include"Wire.h"
#include "SPI.h"
#include "SparkFunLSM6DS3.h"


LSM6DS3 myIMU( I2C_MODE, 0x6A ); 
int data;
int push = 2;
void setup() {
  // put your setup code here, to run once:
Serial.begin(9600);
  if( myIMU.begin() != 0 )
  {
    Serial.println(0);
  }
  else  
  {
      Serial.println(1);
  }
  pinMode(push ,INPUT);
}


void loop() {

    int sensorValue1 = analogRead(A0);
    int sensorValue2 = analogRead(A1);

    
    if (Serial.available())
    {data = Serial.read();

    if(data == 'a'){
      Serial.println(myIMU.readFloatAccelX(),4);
      }
    if(data == 'b'){
      Serial.println(myIMU.readFloatAccelY(),4);
      }
    if(data == 'c'){
      Serial.println(myIMU.readFloatAccelZ(),4);
      }
    if(data == 'd'){
      Serial.println(myIMU.readFloatGyroX(),4);
      }
    if(data == 'e'){
      Serial.println(myIMU.readFloatGyroY(),4);
      }
    if(data == 'f'){
      Serial.println(myIMU.readFloatGyroZ(),4);
      }
    if(data == 'g'){
      
      Serial.println(sensorValue2, DEC);
      }
    if(data == 'h'){
      Serial.println(digitalRead(push));
      }
    if(data == 'i'){
      Serial.println(sensorValue1,DEC);
      }
    }

}
