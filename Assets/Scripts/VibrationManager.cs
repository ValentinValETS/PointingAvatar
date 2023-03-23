using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enums;
using MuscleVibrations = System.Collections.Generic.List<Assets.Scripts.Enums.EMuscleVibrationPin>;
using System;

[Obsolete("Use ExperimentalTrial.cs instead", false)]
public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance { get; private set; }

    private float timer = 0.0f;
    private const float delay = 3.0f;
    private MuscleVibrations selectedVibrations;

    private Pattern<MuscleVibrations> Pattern1 = new Pattern<MuscleVibrations>(new List<MuscleVibrations> () 
    { 
        new MuscleVibrations() { EMuscleVibrationPin.Bicep, EMuscleVibrationPin.Tricep }, 
        new MuscleVibrations() { EMuscleVibrationPin.DeltoideAnterieur, EMuscleVibrationPin.DeltoidePosterieur },
        new MuscleVibrations() { EMuscleVibrationPin.Tricep, EMuscleVibrationPin.DeltoideAnterieur }
    });

    private Pattern<MuscleVibrations> Pattern2 = new Pattern<MuscleVibrations>(new List<MuscleVibrations>()
    {
        new MuscleVibrations() { EMuscleVibrationPin.Bicep, EMuscleVibrationPin.Tricep, EMuscleVibrationPin.DeltoidePosterieur },
        new MuscleVibrations() { EMuscleVibrationPin.DeltoideAnterieur, EMuscleVibrationPin.DeltoidePosterieur },
        new MuscleVibrations() { EMuscleVibrationPin.Tricep, EMuscleVibrationPin.DeltoideAnterieur }
    });

    private List<Pattern<MuscleVibrations>> Patterns;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Patterns = new List<Pattern<MuscleVibrations>>() { Pattern1, Pattern2 };
    }

    void ExecutePattern(Pattern<MuscleVibrations> vibrationsPattern)
    {
        try
        {
            if (selectedVibrations == null)
            {
                selectedVibrations = vibrationsPattern.FIFO();
                ArduinoManager.Instance.activateVibrations(selectedVibrations);
            }

            timer += Time.fixedDeltaTime;

            if (timer >= delay)
            {
                ArduinoManager.Instance.deactivateVibrations();
                selectedVibrations = vibrationsPattern.FIFO();
                ArduinoManager.Instance.activateVibrations(selectedVibrations);
                timer = 0;
            }
        }
        catch (ExperimentalTrialCompletedException)
        {
            Patterns.RemoveAt(0);
            GetComponent<TargetCalibration>().initCalibrationDone = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<TargetCalibration>().initCalibrationDone)
            ExecutePattern(Patterns[0]);
    }
}
