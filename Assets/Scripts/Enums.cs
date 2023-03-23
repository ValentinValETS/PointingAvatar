namespace Assets.Scripts.Enums
{
    /// <summary>
    /// pointR is rest position.
    /// First letter is for shoulder, second letter is for elbow. 
    /// For shoulder, P (+) is for horizontal adduction (flexion) and M (-) is for horizontal abduction (extension), 30 degrees from the rest position.
    /// <see href="https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.acefitness.org%2Ffitness-certifications%2Face-answers%2Fexam-preparation-blog%2F3535%2Fmuscles-that-move-the-arm%2F&psig=AOvVaw0Cf4FJQGsjpvyBPjXL4AcA&ust=1678331928835000&source=images&cd=vfe&ved=0CBAQjRxqFwoTCOD7g-Wvy_0CFQAAAAAdAAAAABAd">Shoulder movement</see>
    /// For elbow, P (+) is for flexion and M (-) is for extension, 30 degrees from the rest position
    /// /// <see href="https://www.google.com/url?sa=i&url=https%3A%2F%2Ftommorrison.uk%2Fblog%2Fflexion--extension-in-detail&psig=AOvVaw1Lb-EVQRtDoNCWaFf7NlW4&ust=1678332370668000&source=images&cd=vfe&ved=0CBAQjRxqFwoTCND44rexy_0CFQAAAAAdAAAAABAE">Elbow movement</see>
    /// </summary>
    public enum EArmPosition
    {
        pointR,
        pointPP,
        pointMM,
        pointPM,
        pointMP
    }

    public enum ETargetHand
    {
        R,
        PP,
        MM,
        PM,
        MP,
    }

    public enum ETargetElbow
    {
        R,
        PM_PP,
        MM_MP
    }

    public enum EMuscleVibrationPin
    {
        Bicep = 9,
        Tricep,
        DeltoidePosterieur,
        DeltoideAnterieur
    }

    public enum EOffset
    {
        Default,
        Further,
        Nearer
    }

    public enum EBodyPart
    {
        hand,
        elbow,
        shoulder
    }

    public enum EDominantHand
    {
        Left,
        Right
    }

    public enum EMovementOffset
    {
        Congruent,
        Raccourcissement,
        Allongement
    }
}
