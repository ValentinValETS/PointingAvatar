int biceps = 9;
int deltoideanterieur = 10;
int deltoideposterieur = 11;
int triceps = 12;

void setup() {
  // put your setup code here, to run once:
  pinMode(biceps, OUTPUT);
  pinMode(deltoideanterieur, OUTPUT);
  pinMode(deltoideposterieur, OUTPUT);
  pinMode(triceps, OUTPUT);
}

void loop() {
  // put your main code here, to run repeatedly:
  digitalWrite(biceps, HIGH);
  delay(2000);
  digitalWrite(biceps, LOW);

  digitalWrite(deltoideanterieur, HIGH);
  delay(2000);
  digitalWrite(deltoideanterieur, LOW);

  digitalWrite(deltoideposterieur, HIGH);
  delay(2000);
  digitalWrite(deltoideposterieur, LOW);

  digitalWrite(triceps, HIGH);
  delay(2000);
  digitalWrite(triceps, LOW);
}
