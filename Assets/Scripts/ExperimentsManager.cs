using Assets.Scripts.Enums;
using DitzelGames.FastIK;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;
using MuscleVibrations = System.Collections.Generic.List<Assets.Scripts.Enums.EMuscleVibrationPin>;
using UnityVicon;
using RootMotion.FinalIK;

public class ExperimentalTrial
{
    public string name { private set; get; }
    public Pattern<ETargetHand> targetsHandPattern { private set; get; }
    public bool sendSignalVibration { private set; get; }
    public Pattern<MuscleVibrations> musclesPattern { private set; get; }
    public float delayGo { private set; get; }
    public float delayStay { private set; get; }
    public float delayGoBack { private set; get; }
    public bool showChronometer { private set; get; }
    public bool showBlackScreen { private set; get; }
    public bool isAvatarHumanControlled { private set; get; }
    public EOffset offset { private set; get; }
    public float elbowAngleOffset { private set; get; }
    public float shoulderAngleOffset { private set; get; }
    public bool isAutomatic { private set; get; }
    public EMovementOffset movementOffset { private set; get; }
    public int signalRepetitions { private set; get; }
    public float factor { private set; get; }

    public ExperimentalTrial(string name = "Default", Pattern<ETargetHand> targetsHandPattern = null, bool sendSignalVibration = false, Pattern<MuscleVibrations> musclesPattern = null,
        float delayGo = 3.0f, float delayStay = 1.0f, float delayGoBack = 2.0f, bool showChronometer = false, bool showBlackScreen = false, bool isAvatarHumanControlled = true, int signalRepetitions = 0,
        EOffset offset = EOffset.Default, float elbowAngleOffset = 0f, float shoulderAngleOffset = 0f, bool isAutomatic = false, EMovementOffset movementOffset = EMovementOffset.Congruent, float factor = 0.0f)
    {
        this.name = name;
        this.targetsHandPattern = targetsHandPattern;
        this.sendSignalVibration = sendSignalVibration;
        this.musclesPattern = musclesPattern;
        this.delayGo = delayGo;
        this.delayStay = delayStay;
        this.delayGoBack = delayGoBack;
        this.showChronometer = showChronometer;
        this.showBlackScreen = showBlackScreen;
        this.isAvatarHumanControlled = isAvatarHumanControlled;
        this.signalRepetitions = signalRepetitions;
        this.offset = offset;
        this.shoulderAngleOffset = shoulderAngleOffset;
        this.elbowAngleOffset = elbowAngleOffset;
        this.isAutomatic = isAutomatic;
        this.movementOffset = movementOffset;
        this.factor = factor;
    }
}

public class ExperimentsManager : MonoBehaviour
{
    #region ExperimentalTrialsTest
    private ExperimentalTrial Incarnation1 = new ExperimentalTrial
    (
        targetsHandPattern: new Pattern<ETargetHand>(new List<ETargetHand>() { ETargetHand.PP, ETargetHand.MM, ETargetHand.PM, ETargetHand.MP }),
        name: "Incarnation1",
        showChronometer: true,
        delayGo: 3
    );

    private ExperimentalTrial Perception1 = new ExperimentalTrial
    (
        musclesPattern: new Pattern<MuscleVibrations>(new List<MuscleVibrations>()
        {
            new MuscleVibrations() { EMuscleVibrationPin.Bicep, EMuscleVibrationPin.DeltoidePosterieur },
            new MuscleVibrations() { EMuscleVibrationPin.Tricep, EMuscleVibrationPin.DeltoideAnterieur },
            new MuscleVibrations() { EMuscleVibrationPin.Bicep, EMuscleVibrationPin.DeltoideAnterieur },
            new MuscleVibrations() { EMuscleVibrationPin.Tricep, EMuscleVibrationPin.DeltoidePosterieur },
            new MuscleVibrations() { EMuscleVibrationPin.Tricep, EMuscleVibrationPin.Bicep, EMuscleVibrationPin.DeltoideAnterieur, EMuscleVibrationPin.DeltoidePosterieur }
        }),
        showBlackScreen: true,
        name: "Perception1"
    );

    private ExperimentalTrial Perception2 = new ExperimentalTrial
    (
        targetsHandPattern: new Pattern<ETargetHand>(new List<ETargetHand>() { ETargetHand.PP, ETargetHand.PM, ETargetHand.MM, ETargetHand.MP }),
        isAvatarHumanControlled: false,
        name: "Perception2"
    );

    private ExperimentalTrial Perception3 = new ExperimentalTrial
    (
        targetsHandPattern: new Pattern<ETargetHand>(new List<ETargetHand>() { ETargetHand.PP, ETargetHand.PM, ETargetHand.MM, ETargetHand.MP }),
        musclesPattern: new Pattern<MuscleVibrations>(new List<MuscleVibrations>()
        {
            new MuscleVibrations() { EMuscleVibrationPin.Bicep, EMuscleVibrationPin.DeltoidePosterieur },
            new MuscleVibrations() { EMuscleVibrationPin.Tricep, EMuscleVibrationPin.DeltoideAnterieur },
            new MuscleVibrations() { EMuscleVibrationPin.Bicep, EMuscleVibrationPin.DeltoideAnterieur },
            new MuscleVibrations() { EMuscleVibrationPin.Tricep, EMuscleVibrationPin.DeltoidePosterieur }
        }),
        isAvatarHumanControlled: false,
        name: "Perception3"
    );

    private ExperimentalTrial Perception4 = new ExperimentalTrial
    (
        targetsHandPattern: new Pattern<ETargetHand>(new List<ETargetHand>() { ETargetHand.PP, ETargetHand.PM, ETargetHand.MM, ETargetHand.MP }),
        musclesPattern: new Pattern<MuscleVibrations>(new List<MuscleVibrations>()
        {
            new MuscleVibrations() { EMuscleVibrationPin.Tricep, EMuscleVibrationPin.DeltoideAnterieur },
            new MuscleVibrations() { EMuscleVibrationPin.Bicep, EMuscleVibrationPin.DeltoidePosterieur },
            new MuscleVibrations() { EMuscleVibrationPin.Tricep, EMuscleVibrationPin.DeltoidePosterieur },
            new MuscleVibrations() { EMuscleVibrationPin.Bicep, EMuscleVibrationPin.DeltoideAnterieur }
        }),
        isAvatarHumanControlled: false,
        name: "Perception4"
    );

    private ExperimentalTrial Perception5 = new ExperimentalTrial
    (
        targetsHandPattern: new Pattern<ETargetHand>(new List<ETargetHand>() { ETargetHand.PP, ETargetHand.PM, ETargetHand.MM, ETargetHand.MP }),
        musclesPattern: new Pattern<MuscleVibrations>(new List<MuscleVibrations>()
        {
            new MuscleVibrations() { EMuscleVibrationPin.Tricep, EMuscleVibrationPin.Bicep, EMuscleVibrationPin.DeltoideAnterieur, EMuscleVibrationPin.DeltoidePosterieur },
            new MuscleVibrations() { EMuscleVibrationPin.Tricep, EMuscleVibrationPin.Bicep, EMuscleVibrationPin.DeltoideAnterieur, EMuscleVibrationPin.DeltoidePosterieur },
            new MuscleVibrations() { EMuscleVibrationPin.Tricep, EMuscleVibrationPin.Bicep, EMuscleVibrationPin.DeltoideAnterieur, EMuscleVibrationPin.DeltoidePosterieur },
            new MuscleVibrations() { EMuscleVibrationPin.Tricep, EMuscleVibrationPin.Bicep, EMuscleVibrationPin.DeltoideAnterieur, EMuscleVibrationPin.DeltoidePosterieur }
        }),
        isAvatarHumanControlled: false,
        name: "Perception5"
    );
    #endregion

    public GameObject TargetsHand;
    public GameObject TargetsElbow;
    public OffsetOptions offsetOptions;
    //public RotationOffsetOptions rotationOffsetOptions;
    public FactorOffsetOptions factorOffsetOptions;

    public VRRigParent VRArmRig;

    public TextMeshProUGUI delayText;
    public Chronometer chronometer;

    private float timerTargets = 0.0f;
    private float timerVibrations = 0.0f;
    private float timerChronometer = 0.0f;
    private float timerLabViewSignal = 0.0f;
    private float timerMoveAvatar = 0.0f;
    private float timerSaveData = 0.0f;
    private float delayBeforeStart = 4.0f;
    private int currentChronometerPhase = 1;
    private int currentMoveAvatarPhase = 1;
    private bool labViewSignalSent = false;
    private int currentSignalRepetition = 0;

    private bool isDoingExperienceTrial = false;
    private bool isPaused = false;
    private bool isThresholdReached = false;

    private GameObject HandIK;
    private GameObject ElbowIK;

    private ExperimentalTrial selectedExperimentTrial;
    private MuscleVibrations selectedVibrations;
    private Transform selectedTargetHand = null;
    private Transform selectedTargetElbow = null;

    private List<ExperimentalTrial> experimentalTrials;

    //same as contructor ? - Valentin
    public static ExperimentsManager Instance { get; private set; }

    public SubjectScript viconData;
    public TargetCalibration targetCalib;
    private float startedShoulderAngle;
    private float startedElbowAngle;


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
        if(ExperimentalTrialsCSVReader.Instance is not null && ExperimentalTrialsCSVReader.Instance.experimentalTrialList.experimentalTrials.Length > 0)
        {
            setExperimentalTrials();
        }
        else
        {
            experimentalTrials = new List<ExperimentalTrial>() { Incarnation1, Perception1, Perception2, Perception3, Perception4, Perception5 };
        }

        if (DominantHandPicker.Instance != null)
        {
            //Hand = DominantHandPicker.Instance.HandAvatar;
            HandIK = DominantHandPicker.Instance.HandIK;
            ElbowIK = DominantHandPicker.Instance.ElbowIK;

        }
        else
        {
            Debug.LogError("DominantHandPicker is null in ExperimentManager.cs!");
        }

    }

    // Main gameplay loop
    void Update()
    {

        // Check if Space key is released and not currently doing an experimental trial
        if (Input.GetKeyUp(KeyCode.Space) && !isDoingExperienceTrial)
        {
            // Set up a new experimental trial and flag as currently doing an experimental trial
            SetNewExperimentalTrial(0);
            isDoingExperienceTrial = true;
        }

        if (!shouldExecuteExperimentalTrial())
            return;

        // Delay countdown before starting an experiment trial
        if (delayBeforeStart > 1.05f)
        {
            ShowCountdownText();
            return;
        }
        else if (delayText.gameObject.activeSelf)
        {
            // Hide countdown text when delay time is up
            delayText.gameObject.SetActive(false);

            // Trial has started, we set the targets for visual help
            if (selectedExperimentTrial.targetsHandPattern is not null)
                selectTargetsPositions(selectedExperimentTrial.targetsHandPattern);
        }

        if (IsNextRepetitionNextFrame())
        {
            isThresholdReached = false;

            // When the next repetition set is empty, this means the current experiment trial has ended. We need to wait for key.space to set new experiment trial
            if (selectedExperimentTrial is null)
                return;

            // Wait for input before continuing the next repetition in Experimental trial.
            if (!isPaused && !selectedExperimentTrial.isAutomatic)
            {
                // Display a message to prompt the user for input
                delayText.gameObject.SetActive(true);
                delayText.text = "En attente...";
                isPaused = true;

                // Wait for user input
                StartCoroutine(WaitForInput());
                return;
            }


        }



        // Checks whether the distance between the controller and the rested position has reached the threshold for executing the experimental trial.
        // Possible TODO: Consider adding a bool variable in experimental_trials.csv to indicate whether a distance threshold is required, a float variable
        // to specify the distance threshold value and a bool variable for non selected
        // targets to still show as long as threshold is not reached.
        if (IsThresholdReached())
            return;

        isThresholdReached = true;

        // Execute the current experiment trial
        ExecuteExperimentTrial();
    }

    /// <summary>
    /// This method sets the experimental trials by reading the data from an external CSV file and creating a list of ExperimentalTrial objects.
    /// For each experimental trial, the target hand pattern is parsed into a list of ETargetHand enums.
    /// If there is no target hand pattern, the pattern is set to null.
    /// The signal repetitions are set based on the number of targets in the target hand pattern, or the value provided in the CSV file.
    /// The experimental trial properties are then added to a new ExperimentalTrial object and added to the list of experimental trials.
    /// </summary>
    void setExperimentalTrials()
    {
        this.experimentalTrials = new List<ExperimentalTrial>();

        ExperimentalTrialsCSVReader.ExperimentalTrialsList experimentalTrials = ExperimentalTrialsCSVReader.Instance.experimentalTrialList;

        foreach(ExperimentalTrialsCSVReader.ExperimentalTrial experimentalTrial in experimentalTrials.experimentalTrials)
        {
            List<ETargetHand> targetHandList = new List<ETargetHand>();
            string[] targetHandPatternStrList = experimentalTrial.TargetsHandPattern.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);

            foreach(string targetHandPatternStr in targetHandPatternStrList)
            {
                switch (targetHandPatternStr)
                {
                    case "++":
                        targetHandList.Add(ETargetHand.PP);
                        break;
                    case "--":
                        targetHandList.Add(ETargetHand.MM);
                        break;
                    case "+-":
                        targetHandList.Add(ETargetHand.PM);
                        break;
                    case "-+":
                        targetHandList.Add(ETargetHand.MP);
                        break;
                }
            }
            this.experimentalTrials.Add(new ExperimentalTrial
            (
                name: experimentalTrial.Name,
                targetsHandPattern: targetHandList.Count > 0 ? new Pattern<ETargetHand>(targetHandList) : null,
                sendSignalVibration: experimentalTrial.SendSignalVibration,
                delayGo: experimentalTrial.DelayGo,
                delayStay: experimentalTrial.DelayStay,
                delayGoBack: experimentalTrial.DelayGoBack,
                showChronometer: experimentalTrial.ShowChronometer,
                isAvatarHumanControlled: experimentalTrial.IsAvatarHumanControlled,
                elbowAngleOffset: experimentalTrial.ElbowAngleOffset,
                shoulderAngleOffset: experimentalTrial.ShoulderAngleOffset,
                signalRepetitions: targetHandList.Count > 0 && experimentalTrial.SendSignalVibration ? targetHandList.Count : experimentalTrial.SignalRepetitions,
                isAutomatic: experimentalTrial.Automatic,
                movementOffset: experimentalTrial.MovementOffset,
                factor: experimentalTrial.Factor
            ));
        }
    }

    /// <summary>
    /// Executes a pattern of muscle vibrations on the arm.
    /// The vibration pattern is defined by a FIFO (First-In-First-Out) pattern structure.
    /// </summary>
    /// <param name="pattern">The pattern to execute.</param>
    /// <remarks>
    /// This method executes the pattern of muscle vibrations on the arm.
    /// The method uses a timer to track the delay between the Go, Stay, and Go Back phases of the pattern.
    /// It activates the selected vibration pattern and deactivates the previous one.
    /// Unfortunately, the labview system waits for only a signal from arduino, so this method is obsolete right now.
    /// </remarks>
    void ExecuteVibrationsPattern(Pattern<MuscleVibrations> pattern)
    {
        if (selectedVibrations == null)
        {
            selectedVibrations = pattern.FIFO();
            ArduinoManager.Instance.activateVibrations(selectedVibrations);
        }

        timerVibrations += Time.deltaTime;

        if (timerVibrations >= (selectedExperimentTrial.delayGo + selectedExperimentTrial.delayStay + selectedExperimentTrial.delayGoBack))
        {
            ArduinoManager.Instance.deactivateVibrations();
            selectedVibrations = pattern.FIFO();
            ArduinoManager.Instance.activateVibrations(selectedVibrations);
            timerVibrations = 0;
        }
    }

    /// <summary>
    /// This method selects target positions for a given hand pattern.
    /// It disables the previous hand and elbow positions, selects a new hand target based on the input pattern,
    /// selects a corresponding elbow target, and applies rotation offsets to the selected targets.
    /// </summary>
    /// <param name="pattern">The hand pattern for which to select target positions.</param>
    void selectTargetsPositions(Pattern<ETargetHand> pattern)
    {
        DisableTargetsHand();
        DisableTargetsElbow();
        ETargetHand selectedTargetHand = pattern.FIFO();
        SelectTargetHand(selectedTargetHand);
        ETargetElbow selectedTargetElbow;

        switch (selectedTargetHand)
        {
            case ETargetHand.R:
                SelectTargetElbow(ETargetElbow.R);
                selectedTargetElbow = ETargetElbow.R;          
                break;
            case ETargetHand.PM:
            case ETargetHand.PP:
                SelectTargetElbow(ETargetElbow.PM_PP);
                selectedTargetElbow = ETargetElbow.PM_PP;
                break;
            case ETargetHand.MM:
            case ETargetHand.MP:
                SelectTargetElbow(ETargetElbow.MM_MP);
                selectedTargetElbow = ETargetElbow.MM_MP;
                break;
            default:
                SelectTargetElbow(ETargetElbow.R);
                selectedTargetElbow = ETargetElbow.R;
                break;
        }
        //rotationOffsetOptions.setRotationOffsetsOnBones(selectedTargetHand, selectedTargetElbow, selectedExperimentTrial);

    }

    

    /// <summary>
    /// IK MODE: Moves the avatar's wrist and elbow to the positions defined by the start and end vectors for a specified amount of time
    /// </summary>
    void MoveAvatarCinematicMode(GameObject handIK, GameObject elbowIK, Vector3 startHand, Vector3 startElbow, Vector3 endHand, Vector3 endElbow)
    {
        
        switch (currentMoveAvatarPhase)
        {
            case 1:
 

                currentMoveAvatarPhase++;
                break;
            case 2:
                timerMoveAvatar += Time.deltaTime;
                handIK.transform.position = Vector3.Lerp(startHand, endHand, timerMoveAvatar / selectedExperimentTrial.delayGo);
                elbowIK.transform.position = Vector3.Lerp(startElbow, endElbow, timerMoveAvatar / selectedExperimentTrial.delayGo);




                if (timerMoveAvatar >= selectedExperimentTrial.delayGo)
                {
                    timerMoveAvatar = 0;
                    currentMoveAvatarPhase++;
                }
                break;
            case 3:
                timerMoveAvatar += Time.deltaTime;

                if (timerMoveAvatar >= selectedExperimentTrial.delayStay)
                {
                    timerMoveAvatar = 0;
                    currentMoveAvatarPhase++;
                }
                break;
            case 4:
                timerMoveAvatar += Time.deltaTime;
                handIK.transform.position = Vector3.Lerp(endHand, startHand, timerMoveAvatar / selectedExperimentTrial.delayGoBack);
                elbowIK.transform.position = Vector3.Lerp(endElbow, startElbow, timerMoveAvatar / selectedExperimentTrial.delayGoBack);

   

                if (timerMoveAvatar >= selectedExperimentTrial.delayGoBack)
                {
                    timerMoveAvatar = 0;
                    currentMoveAvatarPhase = 1;
                }
                break;
        }
    }

    //void MoveAvatarCinematicMode(GameObject handIK, GameObject elbowIK, Vector3 startHand, Vector3 startElbow, Vector3 endHand, Vector3 endElbow)
    //{

    //    switch (currentMoveAvatarPhase)
    //    {
    //        case 1:
    //            Vector3 segment_upperArm_actual = DominantHandPicker.Instance.VirtualShoulderPosition.transform.position - DominantHandPicker.Instance.VirtualElbowPosition.transform.position;
    //            Vector3 segment_upperArm_desired = DominantHandPicker.Instance.VirtualShoulderPosition.transform.position - startElbow;
    //            float beginShoulderAngle = Mathf.Abs(Vector3.Angle(segment_upperArm_actual, segment_upperArm_desired));
    //            DominantHandPicker.Instance.VirtualShoulderPosition.transform.Rotate(-beginShoulderAngle, 0, 0);
    //            //startedShoulderAngle = DominantHandPicker.Instance.VirtualShoulderPosition.transform.rotation.x;
    //            segment_upperArm_actual = DominantHandPicker.Instance.VirtualShoulderPosition.transform.position - DominantHandPicker.Instance.VirtualElbowPosition.transform.position;
    //            segment_upperArm_desired = DominantHandPicker.Instance.VirtualShoulderPosition.transform.position - endElbow;
    //            startedShoulderAngle = Mathf.Abs(Vector3.Angle(segment_upperArm_actual, segment_upperArm_desired));

    //            Vector3 segment_Arm_actual = DominantHandPicker.Instance.VirtualElbowPosition.transform.position - DominantHandPicker.Instance.VirtualHandPosition.transform.position;
    //            Vector3 segment_Arm_desired = DominantHandPicker.Instance.VirtualElbowPosition.transform.position - startHand;
    //            float beginElbowAngle = Mathf.Abs(Vector3.Angle(segment_Arm_actual, segment_Arm_desired));
    //            DominantHandPicker.Instance.VirtualElbowPosition.transform.Rotate(+beginElbowAngle, 0, 0);
    //            //startedElbowAngle = DominantHandPicker.Instance.VirtualElbowPosition.transform.rotation.x;
    //            segment_Arm_actual = DominantHandPicker.Instance.VirtualElbowPosition.transform.position - DominantHandPicker.Instance.VirtualHandPosition.transform.position;
    //            segment_Arm_desired = DominantHandPicker.Instance.VirtualElbowPosition.transform.position - endHand;
    //            startedElbowAngle = Mathf.Abs(Vector3.Angle(segment_Arm_actual, segment_Arm_desired));

    //            Debug.DrawLine(DominantHandPicker.Instance.VirtualShoulderPosition.transform.forward, startElbow, Color.yellow, 4f);

    //            //DominantHandPicker.Instance.VirtualShoulderPosition.transform.LookAt(startElbow);
    //            // DominantHandPicker.Instance.VirtualElbowPosition.transform.LookAt(startHand);

    //            //Debug.DrawLine(DominantHandPicker.Instance.VirtualShoulderPosition.transform.position, DominantHandPicker.Instance.VirtualElbowPosition.transform.position, Color.red, 2.5f);


    //            Debug.DrawLine(DominantHandPicker.Instance.VirtualElbowPosition.transform.position, DominantHandPicker.Instance.VirtualHandPosition.transform.position, Color.red, 2.5f);
    //            Debug.DrawLine(DominantHandPicker.Instance.VirtualElbowPosition.transform.position, startHand, Color.yellow, 4f);

    //            currentMoveAvatarPhase++;
    //            break;
    //        case 2:
    //            timerMoveAvatar += Time.deltaTime;
    //            //handIK.transform.position = Vector3.Lerp(startHand, endHand, timerMoveAvatar / selectedExperimentTrial.delayGo);
    //            //elbowIK.transform.position = Vector3.Lerp(startElbow, endElbow, timerMoveAvatar / selectedExperimentTrial.delayGo);

    //            //Faire un Lerp de angle X jusqu'à ce Vector3.Angle=0;
    //            //Calcul angle entre segments upper_real et upper_desired
    //            segment_upperArm_actual = DominantHandPicker.Instance.VirtualShoulderPosition.transform.position - DominantHandPicker.Instance.VirtualElbowPosition.transform.position;
    //            segment_upperArm_desired = DominantHandPicker.Instance.VirtualShoulderPosition.transform.position - endElbow;
    //            float currentAngle = Vector3.Angle(segment_upperArm_actual, segment_upperArm_desired);

    //            // Calculate the target angle (0 degrees)
    //            float targetAngle = 0f;
    //            // Interpolate between the starting and target angles using Lerp
    //            float angle = Mathf.Lerp(startedShoulderAngle, targetAngle, timerMoveAvatar / selectedExperimentTrial.delayGo);
    //            // Rotate the transform around the common point
    //            DominantHandPicker.Instance.VirtualShoulderPosition.transform.RotateAround(DominantHandPicker.Instance.VirtualShoulderPosition.transform.position, Vector3.up, angle - currentAngle);
    //            DominantHandPicker.Instance.VirtualShoulderPosition.transform.Rotate(-(angle - currentAngle), 0, 0);
    //            //DominantHandPicker.Instance.VirtualShoulderPosition.transform.LookAt(endElbow);


    //            //Calcul angle entre segments upper_real et upper_desired
    //            segment_Arm_actual = DominantHandPicker.Instance.VirtualElbowPosition.transform.position - DominantHandPicker.Instance.VirtualHandPosition.transform.position;
    //            segment_Arm_desired = DominantHandPicker.Instance.VirtualElbowPosition.transform.position - endHand;
    //            currentAngle = Vector3.Angle(segment_Arm_actual, segment_Arm_desired);
    //            // Interpolate between the starting and target angles using Lerp
    //            angle = Mathf.Lerp(startedElbowAngle, targetAngle, timerMoveAvatar / selectedExperimentTrial.delayGo);
    //            // Rotate the transform around the common point
    //            DominantHandPicker.Instance.VirtualElbowPosition.transform.RotateAround(DominantHandPicker.Instance.VirtualElbowPosition.transform.position, Vector3.up, angle - currentAngle);
    //            DominantHandPicker.Instance.VirtualElbowPosition.transform.Rotate(-(angle - currentAngle), 0, 0);
    //            //DominantHandPicker.Instance.VirtualShoulderPosition.transform.LookAt(endHand);



    //            if (timerMoveAvatar >= selectedExperimentTrial.delayGo)
    //            {
    //                timerMoveAvatar = 0;
    //                currentMoveAvatarPhase++;
    //            }
    //            break;
    //        case 3:
    //            timerMoveAvatar += Time.deltaTime;

    //            if (timerMoveAvatar >= selectedExperimentTrial.delayStay)
    //            {
    //                timerMoveAvatar = 0;
    //                currentMoveAvatarPhase++;
    //            }
    //            break;
    //        case 4:
    //            timerMoveAvatar += Time.deltaTime;
    //            handIK.transform.position = Vector3.Lerp(endHand, startHand, timerMoveAvatar / selectedExperimentTrial.delayGoBack);
    //            elbowIK.transform.position = Vector3.Lerp(endElbow, startElbow, timerMoveAvatar / selectedExperimentTrial.delayGoBack);

    //            segment_upperArm_actual = DominantHandPicker.Instance.VirtualShoulderPosition.transform.position - DominantHandPicker.Instance.VirtualElbowPosition.transform.position;
    //            segment_upperArm_desired = DominantHandPicker.Instance.VirtualShoulderPosition.transform.position - endElbow;
    //            currentAngle = Vector3.Angle(segment_upperArm_actual, segment_upperArm_desired);

    //            // Calculate the target angle (0 degrees)
    //            targetAngle = 0f;
    //            // Interpolate between the starting and target angles using Lerp
    //            angle = Mathf.Lerp(targetAngle, startedShoulderAngle, timerMoveAvatar / selectedExperimentTrial.delayGo);
    //            // Rotate the transform around the common point
    //            //DominantHandPicker.Instance.VirtualShoulderPosition.transform.RotateAround(DominantHandPicker.Instance.VirtualShoulderPosition.transform.position, Vector3.up, angle - currentAngle);
    //            DominantHandPicker.Instance.VirtualShoulderPosition.transform.Rotate(-(angle - currentAngle), 0, 0);


    //            //Calcul angle entre segments upper_real et upper_desired
    //            segment_Arm_actual = DominantHandPicker.Instance.VirtualElbowPosition.transform.position - DominantHandPicker.Instance.VirtualHandPosition.transform.position;
    //            segment_Arm_desired = DominantHandPicker.Instance.VirtualElbowPosition.transform.position - endHand;
    //            currentAngle = Vector3.Angle(segment_Arm_actual, segment_Arm_desired);
    //            // Interpolate between the starting and target angles using Lerp
    //            angle = Mathf.Lerp(targetAngle, startedElbowAngle, timerMoveAvatar / selectedExperimentTrial.delayGo);
    //            // Rotate the transform around the common point
    //            //DominantHandPicker.Instance.VirtualElbowPosition.transform.RotateAround(DominantHandPicker.Instance.VirtualElbowPosition.transform.position, Vector3.up, angle - currentAngle);
    //            DominantHandPicker.Instance.VirtualElbowPosition.transform.Rotate(-(angle - currentAngle), 0, 0);

    //            if (timerMoveAvatar >= selectedExperimentTrial.delayGoBack)
    //            {
    //                timerMoveAvatar = 0;
    //                currentMoveAvatarPhase = 1;
    //            }
    //            break;
    //    }
    //}

    /// <summary>
    /// Executes a pattern of targets for the hand and elbow according to the provided pattern object.
    /// The method selects the target positions and sets the rotation offsets for the current trial.
    /// The pattern is repeated until the total time exceeds the sum of the delays defined for the trial.
    /// </summary>
    /// <param name="pattern">The pattern object containing the sequence of hand targets to follow</param>
    void ExecuteTargetsPattern(Pattern<ETargetHand> pattern)
    {
        if (selectedTargetHand is null)
        {
            selectTargetsPositions(pattern);
        }

        timerTargets += Time.deltaTime;

        if (timerTargets >= (selectedExperimentTrial.delayGo + selectedExperimentTrial.delayStay + selectedExperimentTrial.delayGoBack))
        {
            selectTargetsPositions(pattern);
            timerTargets = 0;
        }
    }

    /// <summary>
    /// Executes the chronometer based on the current phase of the trial, updating the timer and showing/hiding the chronometer as necessary.
    /// </summary>
    /// <remarks>
    /// This method is used to show the chronometer to the user during the experiment, displaying the time elapsed during each phase of the trial.
    /// The chronometer is displayed during the first two phases of the trial and hidden during the third.
    /// The timer is updated during each phase, and when the timer reaches the end of the phase, the chronometer is either hidden or reset depending on the phase.
    /// </remarks>
    void ExecuteChronometer()
    {
        if (!chronometer.gameObject.activeSelf && currentChronometerPhase < 3)
            chronometer.Show();

        switch (currentChronometerPhase)
        {
            case 1:
                timerChronometer += Time.deltaTime;
                chronometer.setTimer(timerChronometer / selectedExperimentTrial.delayGo);

                if (timerChronometer >= selectedExperimentTrial.delayGo)
                {
                    chronometer.CleanTimer();
                    timerChronometer = 0;
                    currentChronometerPhase++;
                }
                break;
            case 2:
                timerChronometer += Time.deltaTime;

                if (timerChronometer >= selectedExperimentTrial.delayStay)
                {
                    chronometer.Hide();
                    timerChronometer = 0;
                    currentChronometerPhase++;
                }
                break;
            case 3:
                timerChronometer += Time.deltaTime;

                if(timerChronometer >= selectedExperimentTrial.delayGoBack)
                {
                    timerChronometer = 0;
                    currentChronometerPhase = 1;
                }
                break;
        }
    }

    /// <summary>
    /// Executes a LabView signal to mark the beginning and end of an experimental trial. 
    /// The signal is sent only once per trial, and the timer is used to keep track of 
    /// when to stop sending the signal.
    /// </summary>
    /// <exception cref="ExperimentalTrialCompletedException">
    /// Thrown when the signal has been sent the specified number of repetitions
    /// </exception>
    void ExecuteLabViewSignal()
    {
        if (!labViewSignalSent)
        {
            ArduinoManager.Instance.sendLabViewSignal();
            labViewSignalSent = true;
            currentSignalRepetition++;
        }
        
        timerLabViewSignal += Time.deltaTime;

        if (timerLabViewSignal >= (selectedExperimentTrial.delayGo + selectedExperimentTrial.delayStay + selectedExperimentTrial.delayGoBack))
        {
            labViewSignalSent = false;
            timerLabViewSignal = 0f;

            if(currentSignalRepetition >= selectedExperimentTrial.signalRepetitions)
                throw new ExperimentalTrialCompletedException();
        }
    }

    /// <summary>
    /// Disables all targets for the hand except for the rested position.
    /// </summary>
    void DisableTargetsHand()
    {
        foreach (Transform target in TargetsHand.transform)
        {
            // We skip the rested position
            if (target != TargetsHand.transform.GetChild(0))
                target.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Disables all elbow targets except for the rested position.
    /// </summary>
    void DisableTargetsElbow()
    {
        foreach (Transform target in TargetsElbow.transform)
        {
            // We skip the rested position
            if (target != TargetsElbow.transform.GetChild(0))
                target.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Selects a target for the hand and sets it as active, and sets the target as the current target for rotation offsets.
    /// Also plays an audio source attached to the target.
    /// </summary>
    /// <param name="target">The enum representing the chosen target</param>
    void SelectTargetHand(ETargetHand target)
    {
        selectedTargetHand = TargetsHand.transform.Find(target.ToString());
        //rotationOffsetOptions.currentTargetHandSelected = selectedTargetHand.gameObject;
        selectedTargetHand.gameObject.SetActive(true);
    }

    /// <summary>
    /// Selects the target elbow based on the given enum value, sets it as the currently selected target elbow, and activates it.
    /// </summary>
    /// <param name="elbow">The enum value representing the target elbow to be selected.</param>
    void SelectTargetElbow(ETargetElbow elbow)
    {
        selectedTargetElbow = TargetsElbow.transform.Find(elbow.ToString());
        //rotationOffsetOptions.currentTargetElbowSelected = selectedTargetElbow.gameObject;
        selectedTargetElbow.gameObject.SetActive(true);
    }

    /// <summary>
    /// Sets the screen color to black if isBlind is true, otherwise clears the screen to show the game.
    /// </summary>
    /// <param name="isBlind">A boolean value that indicates whether the screen should be set to black or not.</param>
    void SetScreenColor(bool isBlind)
    {
        if(isBlind)
            SteamVR_Fade.View(Color.black, 1);
        else
            SteamVR_Fade.View(Color.clear, 1);
    }

    /// <summary>
    /// Set the Avatar's inverse kinematics (IK) based on whether the Avatar is human-controlled or not.
    /// </summary>
    /// <param name="isAvatarHumanControlled">A boolean flag indicating whether the Avatar is human-controlled or not.</param>
    void SetAvatarIK(bool isAvatarHumanControlled)
    {
        if (!isAvatarHumanControlled)
        {
            viconData.enabled = false;
            //DominantHandPicker.Instance.VirtualElbowPosition.GetComponent<ArmIK>().enabled = false;
            factorOffsetOptions.gameObject.SetActive(false);
            DominantHandPicker.Instance.VirtualElbowPosition.GetComponent<ArmIK>().enabled = true;

            //DominantHandPicker.Instance.VirtualHandPosition.transform.parent.GetComponent<FastIKFabric>().enabled = false;
            //DominantHandPicker.Instance.VirtualHandPosition.GetComponent<FastIKFabric>().ChainLength = 2;
        }
        else
        {
            viconData.enabled = true;
            factorOffsetOptions.gameObject.SetActive(true);
            DominantHandPicker.Instance.VirtualElbowPosition.GetComponent<ArmIK>().enabled = false;
            //DominantHandPicker.Instance.VirtualHandPosition.GetComponent<FastIKFabric>().ChainLength = 1;
            //DominantHandPicker.Instance.VirtualHandPosition.GetComponent<FastIKFabric>().enabled = false;
            //DominantHandPicker.Instance.VirtualHandPosition.transform.parent.GetComponent<FastIKFabric>().enabled = true;
            //DominantHandPicker.Instance.VirtualHandPosition.GetComponent<FastIKFabric>().enabled = true;


        }
    }

    void SetOffset(EOffset offset)
    {
        offsetOptions.offset = offset;
    }

    void MoveAvatar1Bone(GameObject wrist, Vector3 start, Vector3 end, float lerpValue)
    {
        wrist.transform.position = Vector3.Lerp(start, end, lerpValue);
    }





    /// <summary>
    /// Sets the currently selected experimental trial by its index in the experimentalTrials list.
    /// If there are no more trials left, it logs a message indicating that the experiment is finished.
    /// </summary>
    void SetNewExperimentalTrial(int index)
    {
        if (experimentalTrials.Count == 0)
        {
            Debug.Log("Experience finished!");
            return;
        }
            
        selectedExperimentTrial = experimentalTrials[index];

        // We now use the elbow and the hand for moving the avatar when it is not human controlled
        SetAvatarIK(selectedExperimentTrial.isAvatarHumanControlled);
        //VRArmRig.enabled = selectedExperimentTrial.isAvatarHumanControlled;

        factorOffsetOptions.factor = selectedExperimentTrial.factor;
        SetScreenColor(selectedExperimentTrial.showBlackScreen);
        SetOffset(selectedExperimentTrial.offset);

        Debug.Log(selectedExperimentTrial.name);
    }

    /// <summary>
    /// Saves data in Vicon system at the appropriate time during the experiment trial.
    /// </summary>
    private void SaveViconData()
    {
        // As long as the timerSaveData is below the delay go, we save the vicon data
        if (timerSaveData <= selectedExperimentTrial.delayGo)
        {
            // TODO: implement save data in Vicon
            Debug.Log("Saving data!");
        }

        timerSaveData += Time.deltaTime;

        if (timerSaveData >= (selectedExperimentTrial.delayGo + selectedExperimentTrial.delayStay + selectedExperimentTrial.delayGoBack)) 
        {
            timerSaveData = 0;
        }
    }

    /// <summary>
    /// Resets all variables and objects to their initial state.
    /// </summary>
    private void Reset()
    {
        SetAvatarIK(true);
        SetScreenColor(false);
        SetOffset(EOffset.Default);
        ArduinoManager.Instance.deactivateVibrations();
        selectedExperimentTrial = null;
        selectedTargetHand = null;
        selectedTargetElbow = null;
        selectedVibrations = null;
        timerTargets = 0f;
        timerVibrations = 0f;
        timerLabViewSignal = 0f;
        timerChronometer = 0f;
        timerMoveAvatar = 0f;
        timerSaveData = 0f;
        chronometer.Hide();
        labViewSignalSent = false;
        isThresholdReached = false;
        currentChronometerPhase = 1;
        currentSignalRepetition = 0;
        currentMoveAvatarPhase = 1;
        delayBeforeStart = 4.0f;
        delayText.text = "Essai commence dans ... secondes ";
        viconData.enabled = true;
        DominantHandPicker.Instance.VirtualElbowPosition.GetComponent<ArmIK>().enabled = false;
        factorOffsetOptions.gameObject.SetActive(false);
    }
    
    bool shouldExecuteExperimentalTrial()
    {
        return isDoingExperienceTrial && selectedExperimentTrial is not null && !isPaused;
    }

    void ShowCountdownText()
    {
        // Display countdown text and decrease delay time
        delayText.gameObject.SetActive(true);
        delayBeforeStart -= Time.deltaTime;
        delayText.text = "Essai commence dans " + (int)delayBeforeStart + " secondes.";
    }

    bool IsNextRepetitionNextFrame()
    {
        float totalRepetitionTime = (selectedExperimentTrial.delayGo + selectedExperimentTrial.delayStay + selectedExperimentTrial.delayGoBack);
        return timerLabViewSignal + Time.deltaTime >= totalRepetitionTime || timerTargets + Time.deltaTime >= totalRepetitionTime || timerVibrations + Time.deltaTime >= totalRepetitionTime;
    }

    bool IsThresholdReached()
    {
        // TODO: maybe set this value in .csv file?
        const float thresholdValue = 0.1f;
        return Vector3.Distance(DominantHandPicker.Instance.RealHandPosition.transform.position, TargetsHand.transform.Find("R").position) < thresholdValue && selectedExperimentTrial.targetsHandPattern is not null && selectedExperimentTrial.isAvatarHumanControlled && !isThresholdReached;
    }



    /// <summary>
    /// Executes the current experiment trial, which consists of various patterns and actions to be executed in a specific order.
    /// </summary>
    void ExecuteExperimentTrial()
    {
        try
        {
            if (selectedExperimentTrial.targetsHandPattern is not null)
                ExecuteTargetsPattern(selectedExperimentTrial.targetsHandPattern);

            if (selectedExperimentTrial.musclesPattern is not null)
                ExecuteVibrationsPattern(selectedExperimentTrial.musclesPattern);

            if (selectedExperimentTrial.sendSignalVibration)
                ExecuteLabViewSignal();

            if (!selectedExperimentTrial.isAvatarHumanControlled && selectedTargetHand is not null)
            {                                
               MoveAvatarCinematicMode(HandIK, ElbowIK, TargetsHand.transform.Find("R").position, TargetsElbow.transform.Find("R").position, selectedTargetHand.position, selectedTargetElbow.position);
            }

            if (chronometer is not null && selectedExperimentTrial.showChronometer)
                ExecuteChronometer();

            SaveViconData();
        }
        catch (ExperimentalTrialCompletedException)
        {
            // Remove the completed experiment trial from the list and reset game state
            experimentalTrials.RemoveAt(0);
            Reset();
            isDoingExperienceTrial = false;
        }
    }

    /// <summary>
    /// This coroutine waits for the user to press the space key.
    /// Once the key is pressed, it continues with the experiment.
    /// It hides the delay text and resumes the experiment.
    /// If the experiment trial is completed, it removes it from the list and resets the experiment.
    /// It is used as a coroutine so that it can pause the execution of the experiment until the space key is pressed,
    /// and also to allow other coroutines to continue running while waiting for the user input.
    /// </summary>
    private IEnumerator WaitForInput()
    {
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));

        // The user has pressed the space key, continue with the experiment
        delayText.text = "";
        delayText.gameObject.SetActive(false);
        isPaused = false;

        // We make sure to execute the frame that was paused
        ExecuteExperimentTrial();

        yield break;
    }
}
