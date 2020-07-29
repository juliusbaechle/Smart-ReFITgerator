#define CMD_SEND_DEVICE_NAME '1'
#define CMD_GET_WEIGHT       '2'
#define DEBOUNCE_TIME 100

#include <HX711_ADC.h>   //HX711_ADC als Bibliothek des Messverstäkers
HX711_ADC LoadCell(4,5); //Pin 4 und 5 als Datenpins zum Messverstärker     

void setup(){  
  LoadCell.begin();        //LoadCell initialisieren
  
  LoadCell.start(500);     //500 ms Messwerte aufnehmen und Nullen  
  if(LoadCell.getTareTimeoutFlag()) //Fehler beim Nullen    
    Serial.println("Fehler, Verkabelung prüfen");
  
  LoadCell.setCalFactor(423.2); //CalFactor experimentell bestimmt
  LoadCell.setSamplesInUse(4);  //Mittelwert von 4 Messwerten aufnehmen

  Serial.begin(9600);
  pinMode(2, INPUT_PULLUP);
}

void pollDoor(){
  static bool doorOpenM = false;
  
  static long lastMillis = 0;
  if(millis() - lastMillis < DEBOUNCE_TIME) return;
  lastMillis = millis();
  
  bool doorOpen = !digitalRead(2);
  if(doorOpen == doorOpenM) return;
    
  doorOpenM = doorOpen;
  Serial.println(doorOpenM ? "door opened" : "door closed");  
}

void checkSerial(){
  if (!Serial.available()) return;
  switch(Serial.read()){
    case CMD_SEND_DEVICE_NAME:
      Serial.println("smart refitgerator arduino");
      break;
    case CMD_GET_WEIGHT:
      int i = LoadCell.getData();
      Serial.print("weight: ");
      Serial.println(i);
      break;
  }
  Serial.flush();
}

void loop(){
  pollDoor();
  LoadCell.update();
  checkSerial();
}
