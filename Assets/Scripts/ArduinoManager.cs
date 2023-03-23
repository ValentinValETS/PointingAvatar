using System.Collections;
using System.Collections.Generic;
using Uduino;
using UnityEngine;
using Assets.Scripts.Enums;

public class ArduinoManager : MonoBehaviour
{
    UduinoManager uduino;

    private int pinLabView = 7;
    public static ArduinoManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        uduino = UduinoManager.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (int Pin in System.Enum.GetValues(typeof(EMuscleVibrationPin)))
            uduino.pinMode(Pin, PinMode.Output);
        uduino.pinMode(pinLabView, PinMode.Output);
    }

    public void toggleVibration(EMuscleVibrationPin muscle, State status)
    {
        uduino.digitalWrite((int)muscle, status);
    }

    public void deactivateVibrations() 
    {
        foreach (int Pin in System.Enum.GetValues(typeof(EMuscleVibrationPin)))
            uduino.digitalWrite(Pin, State.LOW);
    }

    public void activateVibrations(List<EMuscleVibrationPin> vibrations)
    {
        foreach (int Pin in vibrations)
            uduino.digitalWrite(Pin, State.HIGH);
    }

    public void sendLabViewSignal()
    {
        uduino.digitalWrite(pinLabView, State.HIGH);

        Invoke("unsendLabViewSignal", 0.5f);
    }

    public void unsendLabViewSignal()
    {
        uduino.digitalWrite(pinLabView, State.LOW);
    }
}
