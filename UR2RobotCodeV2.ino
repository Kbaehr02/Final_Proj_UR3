#include <AccelStepper.h>
#include <MultiStepper.h>
#include <Stepper.h>
#define motorInterfaceType 1//1 means an external stepper driver with Step and Direction pins.
// Creates instance of MultiStepper class "steppers".
MultiStepper steppers;
// Declaring each motor respectively.
AccelStepper BaseStepper(motorInterfaceType, 9,8);
AccelStepper ScrewStepper(motorInterfaceType, 11,12);
// Motor Variable Declarations
long positions[2]; // Array of desired stepper positions
// 200 steps-per-revolution / degrees in full rotation.
double stepsPerDeg = (200.0/360.0);
long BaseSteps =0;
// With 20 threads per inch for the threaded rod, 20 revolutions = 1 inch of movement.
long long stepsPerInch = (200*20);
long ScrewSteps =0;
// Arduino and serial comms set-up
const int ledPin = 13; // the pin that the LED is attached to
const byte buffSize = 2;
unsigned int inputBuffer[buffSize];
const char startMarker = '<';
const char endMarker = '>';
byte bytesRecvd = 0;
boolean readInProgress = false;
boolean newDataFromPC = false;
byte coordinates[2];
void setup() 
{
  // Open serial monitor with match baud rate to C# code.
  Serial.begin(9600);
  pinMode(ledPin, OUTPUT);
  // BaseStepper initial conditions
  BaseStepper.setMaxSpeed(500.0);
  BaseStepper.setAcceleration(50.0);
  BaseStepper.setSpeed(100);
  // Lead Screw initial conditions
  ScrewStepper.setMaxSpeed(1000.0);
  ScrewStepper.setAcceleration(50.0);
  ScrewStepper.setSpeed(100);
  // Store steppers in MultiStepper class for synchronus movement
  steppers.addStepper(BaseStepper);
  steppers.addStepper(ScrewStepper);
  // Trial movements to confirm proper motor control
  // Move rotate 180 degrees CW and the end effector 1 inch away.
  positions[0] = -100;
  positions[1] = 200*20;
  steppers.moveTo(positions);
  steppers.runSpeedToPosition(); // Blocks until all are in position
  delay(1000);
  // Returns each component to its starting point
  positions[0] = 0;
  positions[1] = 0;
  steppers.moveTo(positions);
  steppers.runSpeedToPosition(); // Blocks until all are in position
  delay(1000);
}
void loop() {
  // Loops until data is found in serial.
  if (Serial.available() > 0) {
    //Attains and stores info from the input buffer
    getDataFromPC();
    // If there is new data, instructs the motors to move according to serial data.
    if (newDataFromPC) {
      MoveMotors();
      digitalWrite(ledPin, HIGH);
      digitalWrite(ledPin, LOW);
      sendCoordinatesToPC();
      newDataFromPC = false;
    }
    else 
    {
    Serial.println("No data received");
    }
  }
}
void sendCoordinatesToPC()
{
  // Send the point data to the PC
  Serial.print("<P");
  Serial.print(coordinates[0]);
  Serial.print(",");
  Serial.print(coordinates[1]);
  // Serial.print(",");
  // Serial.print(steps);
  Serial.println(">");
}
void getDataFromPC() 
{
  Serial.println("Getting data");
// Receive data from PC and save it into inputBuffer
  if(Serial.available() > 0) 
  {
    Serial.println("Available");
    char x = Serial.read();
    // The order of these IF clauses is significant
    if (x == endMarker) 
    {
      // Copies the serial data to the input buffer.
      readInProgress = false;
      newDataFromPC = true;
      inputBuffer[bytesRecvd] = 0;
      coordinates[0] = inputBuffer[0];
      coordinates[1] = inputBuffer[1];
    }
    if(readInProgress) 
    {
      inputBuffer[bytesRecvd] = x;
      bytesRecvd ++;
      if (bytesRecvd == buffSize) 
      {
        bytesRecvd = buffSize - 1;
      }
    }
    if (x == startMarker) 
    {
      bytesRecvd = 0;
      readInProgress = true;
    }
  }
}
void MoveMotors()
{
  // Base motor movement in degrees
  BaseSteps = (-1)*long(coordinates[0]*stepsPerDeg);
  // Lead screw movement
  ScrewSteps = long(coordinates[1]*stepsPerInch);
  // Move to positions relative to starting position
  positions[0] = BaseSteps;
  positions[1] = ScrewSteps;
  steppers.moveTo(positions);
  steppers.runSpeedToPosition(); // Blocks until all are in position
  // Base Motor movement to home
  BaseSteps = 0;
  //Lead Screw movement to home
  ScrewSteps = 0;
  // Move to home
  positions[0] = BaseSteps;
  positions[1] = ScrewSteps;
  steppers.moveTo(positions);
  steppers.runSpeedToPosition(); // Blocks until all are in position
  delay(1000);
}