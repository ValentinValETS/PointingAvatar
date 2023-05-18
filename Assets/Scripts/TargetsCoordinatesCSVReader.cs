using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class TargetsCoordinatesCSVReader : MonoBehaviour
{
    //Récupère les données de positions du bras, dominance, x et z position et les stocke dans une classe Coordinate
    public static TargetsCoordinatesCSVReader Instance { get; private set; }
    public CoordinateList coordinatesList = new CoordinateList();

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

    void ReadCSV()
    {
        TextAsset textAssetData = Resources.Load<TextAsset>("visualisation_combinaison"); // Replace "filename" with the name of your CSV file without the ".csv" extension

        string[] data = textAssetData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);

        const int numColumns = 5;
        int tableSize = data.Length / numColumns - 1;
        coordinatesList.coordinates = new Coordinate[tableSize];

        for (int i = 0; i < tableSize; i++)

        {
            coordinatesList.coordinates[i] = new Coordinate();

            coordinatesList.coordinates[i].ArmPosition = data[numColumns * (i + 1)];
            coordinatesList.coordinates[i].BodyPart = data[numColumns * (i + 1) + 1];
            coordinatesList.coordinates[i].x = float.Parse(data[numColumns * (i + 1) + 2], CultureInfo.InvariantCulture);
            coordinatesList.coordinates[i].z = float.Parse(data[numColumns * (i + 1) + 3], CultureInfo.InvariantCulture);
            coordinatesList.coordinates[i].dominantHand = int.Parse(data[numColumns * (i + 1) + 4], CultureInfo.InvariantCulture);
        }

    }

    public (float x, float z) getCoordinates(string ArmPosition, string BodyPart)
    {
        float x = 0f;
        float z = 0f;

        for (int i = 0; i < coordinatesList.coordinates.Length; i++)
        {
            Coordinate coordinate = coordinatesList.coordinates[i];
            if (coordinate.ArmPosition.Equals(ArmPosition) && coordinate.BodyPart.Equals(BodyPart))
            {
                x = coordinate.x;
                z = coordinate.z;
                break;
            }
        }
        return (x, z);
    }
}

[System.Serializable]
public class Coordinate
{
    public string ArmPosition;
    public string BodyPart;
    public float x;
    public float z;
    public int dominantHand;
}
[System.Serializable]

public class CoordinateList
{
    public Coordinate[] coordinates;
}