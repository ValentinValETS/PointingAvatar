using System;
using System.Collections;
using System.Collections.Generic;

public class CSVValues
{
    public string nom { get; set; }
    public string piedDominant { get; set; }
    public double age { get; set; }
    public int genre { get; set; }
    public double taille { get; set; }
    public double poids { get; set; }
    public double seins { get; set; }
    public double couCirconference { get; set; }
    public double busteCirconference { get; set; }
    public double tailleCirconference { get; set; }
    public double muscle { get; set; }
    public double graisse { get; set; }
    //public bool modele3D { get; set; }
    //public float longueurD { get; set; }
    //public float longueurG { get; set; }
    //public float longueurD_clone { get; set; }
    //public float longueurG_clone { get; set; }
    //public CSVValues(string _nom,string _piedDominant,double _age, int _genre, double _taille, double _poids, double _seins, double _couCirconference, double _busteCirconference,double _tailleCirconference,bool _modele3D,float _longueurD, float _longueurG, float _longueurD_clone, float _longueurG_clone)
    //{
    //    nom = _nom;
    //    piedDominant = _piedDominant;
    //    age = _age;
    //    genre = _genre;
    //    taille = _taille;
    //    poids = _poids;
    //    seins = _seins;
    //    couCirconference = _couCirconference;
    //    busteCirconference = _busteCirconference;
    //    tailleCirconference = _tailleCirconference;
    //    modele3D = _modele3D;
    //    longueurD = _longueurD;
    //    longueurG = _longueurG;
    //    longueurD_clone = _longueurD_clone;
    //    longueurG_clone = _longueurG_clone;

    //    determineMorphology();
    //    InterpolateMHMValues();

    //}

    public CSVValues(string _nom, string _piedDominant, double _age, int _genre, double _taille, double _poids, double _seins, double _couCirconference, double _busteCirconference, double _tailleCirconference)
    {
        nom = _nom;
        piedDominant = _piedDominant;
        age = _age;
        genre = _genre;
        taille = _taille;
        poids = _poids;
        seins = _seins;
        couCirconference = _couCirconference;
        busteCirconference = _busteCirconference;
        tailleCirconference = _tailleCirconference;

        determineMorphology();
        InterpolateMHMValues();

    }

    void determineMorphology()
    {
        double taileIMC = (taille / 100) * (taille / 100);
        double IMC = poids / taileIMC;
        if (IMC < 18.5) { muscle = 0.2; graisse = 0.7; }
        else if (IMC < 25) { muscle = 0.5; graisse = 1; }
        else if (IMC < 30) { muscle = 0.2; graisse = 1.4; }
        else if (IMC >= 30) { muscle = 0; graisse = 1.5; }

        this.graisse = (this.graisse*100 -50) / (150-50);
    }

    void InterpolateMHMValues()
    {
        double MIN_AGE = 0;
        double MID_AGE = 25;
        double MAX_AGE = 90;

        if (this.age < MIN_AGE || this.age > MAX_AGE)
        {
            //ERROR
        }
        if (this.age < MID_AGE)
        {
            this.age = (this.age - MIN_AGE) / ((MAX_AGE - MIN_AGE) * 2);
        }
        else
        {
            this.age = ((this.age - MID_AGE) / ((MAX_AGE - MID_AGE) * 2)) + 0.5;
        }

        //double HAUTEUR_FEMME = 108;
        //double HAUTEUR_HOMME = 109;

        if (this.genre==0)
        {
            this.taille = (this.taille - 120) / (200-120);
        }
        else
        {
            this.taille = (this.taille - 135) / (215 - 135);
        }

        // FEMME
        // 124<Hauteur<232
        // 25<Cou<35
        // 72<Buste<102
        // 64<Taille<84
        // 89<Hanche<117

        //HOMME
        // 138<Hauteur<247
        // 34<Cou<44
        // 86<Buste<117
        // 70<Taille<91
        // 87<Hanche<116

    }

}
