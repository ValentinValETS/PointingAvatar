using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts.Enums;
using System.Globalization;

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

        int numColumns = 15;
        int tableSize = data.Length / numColumns - 1;
        experimentalTrialList.experimentalTrials = new ExperimentalTrial[tableSize];

        for (int i = 0; i < tableSize; i++)

        {
            float f;
            int l;
            bool b;
            experimentalTrialList.experimentalTrials[i] = new ExperimentalTrial();
            experimentalTrialList.experimentalTrials[i].TargetsHandPattern = data[numColumns * (i + 1)];
            bool.TryParse(data[numColumns * (i + 1) + 1].ToString(CultureInfo.InvariantCulture), out b);
            experimentalTrialList.experimentalTrials[i].SendSignalVibration = b;
            int.TryParse(data[numColumns * (i + 1) + 2], NumberStyles.Any,CultureInfo.InvariantCulture, out l);
            experimentalTrialList.experimentalTrials[i].SignalRepetitions = l;
            Debug.Log(experimentalTrialList.experimentalTrials[i].SignalRepetitions);
            float.TryParse(data[numColumns * (i + 1) + 3], NumberStyles.Any, CultureInfo.InvariantCulture, out f);
            experimentalTrialList.experimentalTrials[i].DelayGo = f;
            float.TryParse(data[numColumns * (i + 1) + 4], NumberStyles.Any, CultureInfo.InvariantCulture, out f);
            experimentalTrialList.experimentalTrials[i].DelayStay = f;
            float.TryParse(data[numColumns * (i + 1) + 5], NumberStyles.Any, CultureInfo.InvariantCulture, out f);
            experimentalTrialList.experimentalTrials[i].DelayGoBack = f;
            bool.TryParse(data[numColumns * (i + 1) + 6].ToString(CultureInfo.InvariantCulture), out b);
            experimentalTrialList.experimentalTrials[i].ShowChronometer = b;
            bool.TryParse(data[numColumns * (i + 1) + 7].ToString(CultureInfo.InvariantCulture), out b);
            experimentalTrialList.experimentalTrials[i].ShowBlackScreen = b;
            bool.TryParse(data[numColumns * (i + 1) + 8].ToString(CultureInfo.InvariantCulture), out b);
            experimentalTrialList.experimentalTrials[i].IsAvatarHumanControlled = b;
            float.TryParse(data[numColumns * (i + 1) + 9], NumberStyles.Any, CultureInfo.InvariantCulture, out f);
            experimentalTrialList.experimentalTrials[i].ElbowAngleOffset = f;
            float.TryParse(data[numColumns * (i + 1) + 10], NumberStyles.Any, CultureInfo.InvariantCulture, out f);
            experimentalTrialList.experimentalTrials[i].ShoulderAngleOffset = f;
            experimentalTrialList.experimentalTrials[i].Name = data[numColumns * (i + 1) + 11];
            bool.TryParse(data[numColumns * (i + 1) + 12].ToString(CultureInfo.InvariantCulture), out b);
            experimentalTrialList.experimentalTrials[i].Automatic = b;
            experimentalTrialList.experimentalTrials[i].MovementOffset = experimentalTrialList.experimentalTrials[i].getMovementOffset(data[numColumns * (i + 1) + 13]);
            float.TryParse(data[numColumns * (i + 1) + 14], NumberStyles.Any, CultureInfo.InvariantCulture, out f);
            experimentalTrialList.experimentalTrials[i].Factor = f;
        }
    }
}