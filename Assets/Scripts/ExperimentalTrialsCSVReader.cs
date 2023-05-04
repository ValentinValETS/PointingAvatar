using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Enums;

public class ExperimentalTrialsCSVReader : MonoBehaviour
{
    [System.Serializable]

    public class ExperimentalTrial
    {
        public string TargetsHandPattern;
        public bool SendSignalVibration;
        public int SignalRepetitions;
        public float DelayGo;
        public float DelayStay;
        public float DelayGoBack;
        public bool ShowChronometer;
        public bool ShowBlackScreen;
        public bool IsAvatarHumanControlled;
        public float ElbowAngleOffset;
        public float ShoulderAngleOffset;
        public string Name;
        public bool Automatic;
        public EMovementOffset MovementOffset;
        public float Factor;

        public EMovementOffset getMovementOffset(string movementOffset)
        {
            movementOffset = movementOffset.TrimEnd('\r');
            return movementOffsetRaccourcissment.Contains(movementOffset) ? EMovementOffset.Raccourcissement : movementOffsetAllongement.Contains(movementOffset) ? EMovementOffset.Allongement : EMovementOffset.Congruent;
        }

        private List<string> movementOffsetRaccourcissment = new List<string> { "Raccourcissement", "Racourcissement", "raccourcissement", "racourcissement", "moins", "Moins", "Diminution", "diminution", "shortening", "Shortening" };
        private List<string> movementOffsetAllongement = new List<string> { "Allongement", "allongement", "alongement", "Alongement", "Egal", "Égal", "egal", "égal", "elongation", "Elongation", "plus", "Plus" };
    }
    [System.Serializable]

    public class ExperimentalTrialsList
    {
        public ExperimentalTrial[] experimentalTrials;
    }

    public static ExperimentalTrialsCSVReader Instance { get; private set; }
    public ExperimentalTrialsList experimentalTrialList = new ExperimentalTrialsList();

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
        ReadCSV();
    }

    /// <summary>
    /// Reads experimental trial data from a CSV file.
    /// </summary>
    void ReadCSV()
    {
        TextAsset textAssetData = Resources.Load<TextAsset>("experimental_trials"); // Replace "filename" with the name of your CSV file without the ".csv" extension

        string[] data = textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        const int numColumns = 14;
        int tableSize = data.Length / numColumns - 1;
        experimentalTrialList.experimentalTrials = new ExperimentalTrial[tableSize];

        for (int i = 0; i < tableSize; i++)

        {
            experimentalTrialList.experimentalTrials[i] = new ExperimentalTrial();

            experimentalTrialList.experimentalTrials[i].TargetsHandPattern = data[numColumns * (i + 1)];
            experimentalTrialList.experimentalTrials[i].SendSignalVibration = bool.Parse(data[numColumns * (i + 1) + 1]);
            experimentalTrialList.experimentalTrials[i].SignalRepetitions = int.Parse(data[numColumns * (i + 1) + 2]);
            experimentalTrialList.experimentalTrials[i].DelayGo = float.Parse(data[numColumns * (i + 1) + 3]);
            experimentalTrialList.experimentalTrials[i].DelayStay = float.Parse(data[numColumns * (i + 1) + 4]);
            experimentalTrialList.experimentalTrials[i].DelayGoBack = float.Parse(data[numColumns * (i + 1) + 5]);
            experimentalTrialList.experimentalTrials[i].ShowChronometer = bool.Parse(data[numColumns * (i + 1) + 6]);
            experimentalTrialList.experimentalTrials[i].ShowBlackScreen = bool.Parse(data[numColumns * (i + 1) + 7]);
            experimentalTrialList.experimentalTrials[i].IsAvatarHumanControlled = bool.Parse(data[numColumns * (i + 1) + 8]);
            experimentalTrialList.experimentalTrials[i].ElbowAngleOffset = float.Parse(data[numColumns * (i + 1) + 9]);
            experimentalTrialList.experimentalTrials[i].ShoulderAngleOffset = float.Parse(data[numColumns * (i + 1) + 10]);
            experimentalTrialList.experimentalTrials[i].Name = data[numColumns * (i + 1) + 11];
            experimentalTrialList.experimentalTrials[i].Automatic = bool.Parse(data[numColumns * (i + 1) + 12]);
            experimentalTrialList.experimentalTrials[i].MovementOffset = experimentalTrialList.experimentalTrials[i].getMovementOffset(data[numColumns * (i + 1) + 13]);
            experimentalTrialList.experimentalTrials[i].Factor = float.Parse(data[numColumns * (i + 1) + 14]);
        }
    }
}