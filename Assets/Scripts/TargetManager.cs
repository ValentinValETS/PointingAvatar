using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enums;
using System;

[Obsolete("Use ExperimentalTrial.cs instead", false)]
public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }
    private float timer = 0.0f;
    private int selectedTarget = -1;

    private const float delay = 3.0f;

    private Pattern<ETargetHand> Pattern1 = new Pattern<ETargetHand>(new List<ETargetHand>() { ETargetHand.PP, ETargetHand.MM, ETargetHand.PM, ETargetHand.MP });
    private Pattern<ETargetHand> Pattern2 = new Pattern<ETargetHand>(new List<ETargetHand>() { ETargetHand.PM, ETargetHand.MM, ETargetHand.MP, ETargetHand.PP });
    private Pattern<ETargetHand> Pattern3 = new Pattern<ETargetHand>(new List<ETargetHand>() { ETargetHand.MP, ETargetHand.PP, ETargetHand.MM, ETargetHand.MP });

    private List<Pattern<ETargetHand>> Patterns;

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
        DisableTargets();
        Patterns = new List<Pattern<ETargetHand>>() { Pattern1, Pattern2, Pattern3 };
    }

    void DisableTargets()
    {
        foreach (Transform target in this.transform)
        {
            if(target != transform.GetChild(transform.childCount - 1))
                target.gameObject.SetActive(false);
        }
    }

    void SelectTarget(int index)
    {
        Transform target = transform.GetChild(index);

        target.gameObject.SetActive(true);

        target.GetComponent<AudioSource>().Play();
    }

    void DisableTarget(int index)
    {
        Transform target = transform.GetChild(index);

        target.gameObject.SetActive(false);
    }

    void ExecutePattern(Pattern<ETargetHand> pattern)
    {
        try
        {
            if (selectedTarget == -1)
            {
                selectedTarget = (int)pattern.FIFO();
                SelectTarget(selectedTarget);
            }

            timer += Time.fixedDeltaTime;

            if (timer >= delay)
            {
                DisableTargets();
                selectedTarget = (int)pattern.FIFO();
                SelectTarget(selectedTarget);
                timer = 0;
            }
        } catch (ExperimentalTrialCompletedException)
        {
            Patterns.RemoveAt(0);
            GetComponent<TargetCalibration>().initCalibrationDone = false;
        }
    }

    void Test()
    {
        if(selectedTarget == -1)
        {
            System.Random r = new System.Random();

            int rInt = selectedTarget;
            while (selectedTarget == rInt)
                rInt = r.Next(0, transform.childCount);

            selectedTarget = rInt;
            SelectTarget(selectedTarget);
        }
        
        timer += Time.fixedDeltaTime;

        if (timer >= delay)
        {
            DisableTarget(selectedTarget);
            System.Random r = new System.Random();

            int rInt = selectedTarget;
            while (selectedTarget == rInt)
                rInt = r.Next(0, transform.childCount);

            selectedTarget = rInt;
            SelectTarget(rInt);
            timer = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<TargetCalibration>().initCalibrationDone)
            ExecutePattern(Patterns[0]);
    }
}
