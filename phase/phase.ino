#include <SPI.h>

#define NUMSAMPLES 9182
int analogPin = 3;     // potentiometer wiper (middle terminal) connected to analog pin 3

int val = 0, val_s = 0;           // variable to store the value read
int signal[NUMSAMPLES];
int i = 0;

int ldac = 5;
int sync = 3;
int data;
void setup()

{

  Serial.begin(9600);          //  setup serial
  SPI.begin();
  SPI.setBitOrder(MSBFIRST);
  SPI.setDataMode(SPI_MODE1);
  pinMode(sync, OUTPUT);
  digitalWrite(sync, HIGH);
  digitalWrite(ldac, LOW);
  while( !(Serial.available() > 0) ) Serial.println("Send 0 to begin sampling");
  if(Serial.read() == 0x49)
  {
    
    //Begin sampling the analog signal
    for(i = 0; i < NUMSAMPLES; i++) {
      signal[i] = analogRead(analogPin);    // read the input pin
      Serial.write(signal[i]>>8);
      Serial.write(signal[i]&0xFF);
    }
  }
  
  //PC should perform FFT and give us a value for microseconds/degree
  while( !(Serial.available() > 0) ) Serial.println("Send something!");;
  if(Serial.read() == 0x49)
  {
    int us_deg = Serial.read();
  }
}



void loop()

{
  int datahi, datalo;
  for(i = 0; i < NUMSAMPLES; i++)
  {
    data = signal[i];
    datahi = data>>7;
    datalo = data<<1;
    digitalWrite(sync, LOW);
    SPI.transfer(datahi);
    SPI.transfer(datalo);
    digitalWrite(sync, HIGH);    
  }

}

