#include <Adafruit_Sensor.h>
#include <Adafruit_BME280.h>
#include "DS1307.h"
#include <SPI.h>

#define BME_CS 5

DS1307 clk;
Adafruit_BME280 bme(BME_CS); 

void setup() {
  Serial.begin(115200);
  clk.setAddress();
  //clk.setAllDate(50, 59, 11, false, 2, 17, 12, 21);  
   bool status;
  
   // default settings
   status = bme.begin();
   if (!status) {
       Serial.println("Could not find a valid BME280 sensor, check wiring!");
       while (1);
   }
}

void loop() { 
task();
}

void task(){
  enum class serialStates{Init, Send, Waith};
  static auto state = serialStates::Waith;
      
  uint8_t bufferTx[12] = {0};
  uint8_t bufferRx[8] = {0}; //
  
  static uint8_t dataCounter = 0;  
  static uint16_t tm = 0; 

  
  switch(state){
    case serialStates::Init:    
    if(Serial.available()){      
      if(Serial.read() == 0x73){
        //clk.setAllDate(50, 59, 11, false, 2, 17, 12, 21);              
        state = serialStates::Waith;
      }
    }
      break;
      
    case serialStates::Waith:
    //esperar datos
    while(Serial.available()){
      bufferRx[dataCounter] = Serial.read();
      dataCounter++;
      if((dataCounter+1) == sizeof(bufferRx)){
        dataCounter = 0;
        break;
      }      
    }
    
    //ya llegaron
    for (uint8_t i = 0; i < sizeof(bufferRx); i++){
      bufferTx[i] = bufferRx[i];
    }
    clk.getAllDate(bufferTx); 
    state = serialStates::Send;
    tm = millis();
      break;
      
    case serialStates::Send:
    
    if ((tm + 2000) > millis() ){
      if(Serial.available()){
          if(Serial.read() == 0x73){
            clk.getAllDate(bufferTx);
            float num = bme.readHumidity();
            memcpy(bufferTx + 8,(uint8_t *)&num,4);
            Serial.write(bufferTx,12);
            tm = millis();              
          }
       }
    }
    else{
      
      state = serialStates::Init;
    }
      break;
  }
/*  
  if(Serial.available()){
      if(Serial.read() == 0x73){
        clk.getAllDate(dt);
        float num = bme.readHumidity();
        memcpy(dt + 8,(uint8_t *)&num,4);
        Serial.write(dt,12);
          
      }
    }
    */  
}
