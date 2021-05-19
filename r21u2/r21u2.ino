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
   bool status;  
   status = bme.begin();
   if (!status) {
       //Serial.println("Could not find a valid BME280 sensor, check wiring!");
       while (1);
   }
}

void loop() { 
task();
}

void task(){
  enum class serialStates{Init, Send, Waith};
  static auto state = serialStates::Init;
      
  uint8_t bufferTx[12] = {0}; //transmite
  uint8_t bufferRx[8] = {0};  //resive
  
  static uint8_t dataCounter = 0;  
  static uint16_t tm = 0; 

  switch(state){
    case serialStates::Init:    
    if(Serial.available()){      
      if(Serial.read() == 0x73){    // read "s"  
        tm = millis();  
        state = serialStates::Waith;
      }
    }
      break;
      
    case serialStates::Waith:
    //esperar datos
    if(Serial.available() > 7){
      while(Serial.available()){
        bufferRx[dataCounter] = Serial.read();
        dataCounter++;
        if((dataCounter) == sizeof(bufferRx)){
          dataCounter = 0;
          break;
        }      
      }
      //ya llegaron
      
      bufferTx[0] = 0x49; // send "I" 0x49
      Serial.write(bufferTx,sizeof(bufferTx));
          
      for (uint8_t i = 0; i < sizeof(bufferRx); i++){
        bufferTx[i] = bufferRx[i];
      }
      
      clk.setAllDate(bufferRx[0], bufferRx[1], bufferRx[2], bufferRx[3], bufferRx[4], bufferRx[5], bufferRx[6], bufferRx[7]);
      
      state = serialStates::Send;
      tm = millis();
      
    }
    else if((tm + 10000) < millis()){
      state = serialStates::Init;
    }
      break;
      
    case serialStates::Send:
    
    if(Serial.available()){       
        if(Serial.read() == 0x72){// read "r" 
                      
          float num = bme.readHumidity();
          memcpy(bufferTx + 8,(uint8_t *)&num,4);
                      
          if(clk.onBus()){
            clk.getAllDate(bufferTx); 
          }else{
            for(uint8_t i = 0; i < 8; i++){
               bufferTx[i] = 0;        
            }
          }
          Serial.write(bufferTx,12);           
        }
     }
      break;
  }
}
