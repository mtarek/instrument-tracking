#include <SPI.h>
 
int ch1 = 4;
int ch2 = 3;

#define CHANNEL(BYTE) (BYTE & 0x80)
#define SIGN(BYTE)    (BYTE & 0x40)
#define OFF(BYTE)     (BYTE & 0x3F)

void setup()
{
    Serial.begin(115200);     // opens serial port, sets data rate to 9600 bps
    SPI.begin();
    SPI.setBitOrder(MSBFIRST);
    SPI.setDataMode(SPI_MODE1);
    pinMode(ch1, OUTPUT);
    pinMode(ch2, OUTPUT);
    digitalWrite(ch1, HIGH);
    digitalWrite(ch2, HIGH);
}
 
void loop()
{
    int i = 0;
    int delta = 0;
    static uint16_t val = 0x0FFF, val1 = 0x07FF, val2 = 0x7FF;
    uint8_t datahi;
    uint8_t datalo;
    int ch;
    int tmp;
   
        if (Serial.available() > 0) {
                // read the incoming byte:
                int inHi = Serial.read();
                int inLo = Serial.read();
                /*if(incomingByte-48 == 1 && (val&0x7FF) < 0x07FF)
                  val += 1;
                if(incomingByte-48 == 0 && (val&0x7FF) > 0)
                  val -= 1;
                if(incomingByte == '-')
                  val ^= (1<<11);
                  ch = ch1;*/
                if(CHANNEL(inHi))
                  ch = ch2;
                else
                  ch = ch1;
               
                /*if(SIGN(incomingByte))
                  val -= OFF(incomingByte);
                else
                  val += OFF(incomingByte);*/
                datahi = 0x7F&inHi;
                datalo = inLo;
                // say what you got:
                Serial.print("val: ");
                Serial.print(val, HEX);
                Serial.print("  hi: ");
                Serial.print(datahi, HEX);
                Serial.print("  lo: ");
                Serial.println(datalo, HEX);
                digitalWrite(ch, LOW);
                SPI.transfer(datahi);
                SPI.transfer(datalo);
                digitalWrite(ch, HIGH);
        }
 
}
