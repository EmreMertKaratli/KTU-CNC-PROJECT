#include <LiquidCrystal.h>
LiquidCrystal lcd(12, 11, 5, 4, 3, 2);

void setup() {
    pinMode(13,OUTPUT);
    lcd.begin(16, 2);
    Serial.begin(9600);
    while (!Serial) {;}
        
}
void loop() {
while(baglanti()) {
    String gcodes[10];
    String satir = "" ;
    satir = diziyeAktar();
    satir_isle(satir, gcodes);
    for(int count = 0; gcodes[count]!=NULL ; count++ ){
        digitalWrite(13,LOW);
        delay(250);
        lcd.clear();
        delay(250);
        lcd.setCursor(0,0);
        delay(250);
        lcd.println(gcodes[count]);
    }    
    delay(250);
    lcd.clear();
    digitalWrite(13,HIGH);
}
}
String diziyeAktar() {
    String mArray = "%";
    while(true) {
        delay(50);
        char k = Serial.read();
        mArray += k;
        if(k=='%')break;
    }
    return mArray;
}
bool baglanti() {
    if((Serial.available()>0)&&(Serial.read()=='%')) {
        Serial.print("%");
        return true;
    } else{
        return false;
    }
}
void satir_isle(String satir, String gcodes[]) {
    int satir_length = satir.length() + 1;
    char satir_array[satir_length];
    satir.toCharArray(satir_array , satir_length);
    int gcode_sayaci = 0;
    Serial.print(satir_array);
    for(int i=0 ; i < satir_length ; i++) {
        if(satir_array[i] == '%') {
            continue;
        }else if(isAlpha(satir_array[i])) {
            String gcode = "";
            do {
                gcode += satir_array[i];
                i++;
            }while(isDigit(satir_array[i])||(satir_array[i]==('.'||'-')));
            i--;
            gcodes[gcode_sayaci] = gcode;
            gcode_sayaci++;
        }else {
            continue;
        }
    }
}
