using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Globalization;


public class CSVController : MonoBehaviour
{
    public string[] CSVNames;
    private string[] CSVPaths;
    //public FootTargetManager ftm1;
    public List<string> CSVValues;

    [HideInInspector]
    public List<string> errors { get; set; }

    public string DeformationSelected { get => deformationSelected; set => deformationSelected = value; }
    public string PreviousGameObject { get => previousGameObject; set => previousGameObject = value; }

    [HideInInspector]
    //public AvatarSceneManager asm1;
    //[HideInInspector]
    public bool isModeleGenerated;

    private string unityPath;
    private string dataPath;
    private string pythonPath;
    private string generatedFBXPath;
    private string templateMHMPath;
    private string generatedMHMPath;
    private string makeHumanPath;
    //private string makeHumanPath_nightly;
    private string modeleAssetPath;
    private string temporaryMakehumanPath = @"D:\ProgramFiles\MakeHuman_nightly\";

    private int indexCSV;

    private GameObject newAvatar;
    private GameObject newAvatar_original;
    public CSVValues listValuesCSV;
    private string FBXName;
    //private List<int> listeOrdreCondition;

    // private ConditionsController conditionController;
    private string deformationSelected;
    private string previousGameObject;
    private bool prefabGenerated;

    private void Awake()
    {
        unityPath = Application.dataPath;
        indexCSV = 0;

        dataPath = Application.streamingAssetsPath + "/Data/CSV/";
        generatedFBXPath = Application.streamingAssetsPath + "/Data/Generated_FBX/";
        modeleAssetPath = unityPath.Replace("/Assets", "/Assets/MakeHumanModels/");
        templateMHMPath = Application.streamingAssetsPath + "/Data/MHM_templates/";
        generatedMHMPath = Application.streamingAssetsPath + "/Data/Generated_MHM/";
        makeHumanPath = Application.streamingAssetsPath + "/MakeHuman/";
        CSVValues = new List<string>();
        errors = new List<string>();
        countCSVs();

        if (temporaryMakehumanPath.Substring(temporaryMakehumanPath.Length) != @"\") { temporaryMakehumanPath = temporaryMakehumanPath + @"\"; }

        CSVValues = readCSV();
        if (CSVValues == null || CSVValues.Count == 0) { errors.Add("CSV vide"); }
        else
        {
            checkCSVValues(CSVValues); //fill listValuesCSV
        }

        prefabGenerated = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Directory.EnumerateFiles(generatedFBXPath, "*.fbx", SearchOption.AllDirectories).Any())
        {
            if(!prefabGenerated)
            {
                UnityEngine.Debug.Log("Call for 5s");
                StartCoroutine(WaitForCopy());
                UnityEngine.Debug.Log("Call fini");
                prefabGenerated = true;
            }
        }
#endif
    }

    IEnumerator WaitForCopy()
    {
        //yield on a new YieldInstruction that waits for 3 seconds.
        yield return new WaitForSeconds(3f);
        CopyDir(generatedFBXPath, modeleAssetPath);
        yield return new WaitForSeconds(10f);
        InstantiateFBX();      
    }


#if UNITY_EDITOR
    void InstantiateFBX()
    {
        if (FBXName != null)
        {
            GameObject objToPrefab = AssetDatabase.LoadAssetAtPath("Assets/MakeHumanModels/" + FBXName.Replace(".fbx", "") + "/" + FBXName, typeof(GameObject)) as GameObject;
            newAvatar = UnityEngine.Object.Instantiate(objToPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0f, 0f, 0));
            newAvatar.name = objToPrefab.name;
            ViconDataStreamClient client = GameObject.Find("ViconDataStreamPrefab").GetComponent<ViconDataStreamClient>();
            ModifyRig modify = new ModifyRig();
            modify.CreateAvatar(client, newAvatar);
            newAvatar_original = UnityEngine.Object.Instantiate(objToPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0f, 0f, 0));
            newAvatar_original.name = objToPrefab.name;
            ModifyRig modify_original = new ModifyRig();
            modify_original.CreateAvatarOriginal(client,newAvatar_original);

            FBXName = null;
        }
        else { UnityEngine.Debug.Log("FBX non généré"); }
    }


    public void CopyDir(string sourcePath, string targetPath)
    {
        List<String> fbxFiles = Directory
                   .GetFiles(sourcePath, "*.*", SearchOption.AllDirectories).Where(name => !name.EndsWith(".meta")).ToList();

        foreach (string file in fbxFiles)
        {
            FileInfo mFile = new FileInfo(file);
            string movePath = targetPath + mFile.FullName.Replace(@"\", @"/").Replace(generatedFBXPath, "");
            FileInfo fileInfo = new FileInfo(movePath);
            if (mFile.Name.ToLower().Contains(".fbx"))
            {
                FBXName = mFile.Name;
            }
            // to remove name collisions
            if (new FileInfo(movePath).Exists == false)
            {
                UnityEngine.Debug.Log(movePath);
                System.IO.Directory.CreateDirectory(fileInfo.Directory.FullName);
                mFile.CopyTo(movePath);                
                //System.IO.File.Copy(file, movePath, true);
            }
        }

        foreach (var subDir in new DirectoryInfo(sourcePath).GetDirectories())
        {
            subDir.Delete(true);
        }
        AssetDatabase.Refresh();
    }
#endif


    void countCSVs()
    {
        CSVNames = Directory.GetFiles(dataPath, "*.csv", System.IO.SearchOption.TopDirectoryOnly).Select(Path.GetFileName).ToArray();
        CSVPaths = Directory.GetFiles(dataPath, "*.csv", System.IO.SearchOption.TopDirectoryOnly);
        for (int j = 0; j < Directory.GetFiles(dataPath, "*.csv", System.IO.SearchOption.TopDirectoryOnly).Length; j++)
        { CSVNames[j] = CSVNames[j].Substring(0, CSVNames[j].Length - 4); }

    }

    public void avatarChanged(int index)
    {
        indexCSV = index;
        CSVValues = readCSV();
        if (CSVValues == null || CSVValues.Count == 0) { errors.Add("CSV vide"); } else { checkCSVValues(CSVValues); }
    }

    private List<string> readCSV()
    {
        List<string> listValeurs = new List<string>();
        using (var reader = new StreamReader(@CSVPaths[indexCSV]))
        {

            reader.ReadLine(); // ignore header
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                for (int i = 0; i < values.Length; i++) { listValeurs.Add(values[i]); }
            }
        }
        return listValeurs;
    }

    public void checkCSVValues(List<string> valeurs)
    {
        if (valeurs[0] == null) { errors.Add("nom sujet manquant"); }
        else
        {
            if (valeurs[1] == null) { errors.Add("pied dominant manquant"); }
            else
            {
                if (valeurs[1] == "d")
                {
                    //ftm1.piedDominant = PiedDominant.Droit;
                }
                else if (valeurs[1] == "g")
                {
                    //ftm1.piedDominant = PiedDominant.Gauche;
                }
                else
                {
                    errors.Add("flag pied dominant non reconnu");
                }

            }
            if (valeurs.Count < 10) { errors.Add("parametres anthropometriques manquants"); }
            else
            {
                //listValuesCSV = new CSVValues(valeurs[0], valeurs[1], Convert.ToDouble(valeurs[2], CultureInfo.InvariantCulture), Convert.ToInt32(valeurs[3], CultureInfo.InvariantCulture), Convert.ToDouble(valeurs[4], CultureInfo.InvariantCulture), Convert.ToDouble(valeurs[5], CultureInfo.InvariantCulture),
                //    Convert.ToDouble(valeurs[6], CultureInfo.InvariantCulture), Convert.ToDouble(valeurs[7], CultureInfo.InvariantCulture), Convert.ToDouble(valeurs[8], CultureInfo.InvariantCulture), Convert.ToDouble(valeurs[9], CultureInfo.InvariantCulture), Convert.ToBoolean(valeurs[10], CultureInfo.InvariantCulture)
                //    , Convert.ToSingle(valeurs[11], CultureInfo.InvariantCulture), Convert.ToSingle(valeurs[12], CultureInfo.InvariantCulture), Convert.ToSingle(valeurs[13], CultureInfo.InvariantCulture), Convert.ToSingle(valeurs[14], CultureInfo.InvariantCulture));

                //if (listValuesCSV.modele3D == false)
                //{
                //    GenerateMHM();
                //    errors.Add("Modèle 3D manquant. Voulez-vous générer le modèle 3D de " + listValuesCSV.nom + " ?");
                //}

                listValuesCSV = new CSVValues(valeurs[0], valeurs[1], Convert.ToDouble(valeurs[2], CultureInfo.InvariantCulture), Convert.ToInt32(valeurs[3], CultureInfo.InvariantCulture), Convert.ToDouble(valeurs[4], CultureInfo.InvariantCulture), Convert.ToDouble(valeurs[5], CultureInfo.InvariantCulture),
                    Convert.ToDouble(valeurs[6], CultureInfo.InvariantCulture), Convert.ToDouble(valeurs[7], CultureInfo.InvariantCulture), Convert.ToDouble(valeurs[8], CultureInfo.InvariantCulture), Convert.ToDouble(valeurs[9], CultureInfo.InvariantCulture));

                GenerateMHM();
            }
        }
    }


    //FIX AU CAS OU MAKEHUMAN 1.1.1 ne fonctionne pas sur la machine, alors télécharger la dernière version de MakeHuman Nightly et modifier manuellement le chemin ici
    public void callMakeHuman()
    {
        prefabGenerated = false;
        if (File.Exists(temporaryMakehumanPath + @"makehuman\makehuman.py"))
        {

            modifyPathExportMakeHuman(@"makehuman\v1py3\");
            callProgramMakeHuman();

        }
        else
        {
            modifyPathExportMakeHuman(@"makehuman\v1\");

            StartCoroutine(callPortableMakeHuman());

            //callPortableMakeHuman();
        }

        isModeleGenerated = true;

        //listValuesCSV.modele3D = true;
    }

    // sometimes the Portable version does not work, so the last nightly version of MakeHuman should be installed on the computer beforehand
    public void callProgramMakeHuman()
    {
        using (Process process = new Process())
        {
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = Path.Combine(Environment.SystemDirectory, "cmd.exe");
            // Redirects the standard input so that commands can be sent to the shell.
            process.StartInfo.RedirectStandardInput = true;
            // Runs the specified command and exits the shell immediately.
            process.EnableRaisingEvents = true;
            process.Start();
            string newmakeHumanPath = makeHumanPath.Replace(@"\", @"/");
            string newgeneratedMHMPath = generatedMHMPath.Replace(@"\", @"/");
            process.StandardInput.WriteLine("cd " + newmakeHumanPath);
            string command = string.Concat(temporaryMakehumanPath + @"Python\python.exe ", temporaryMakehumanPath + @"makehuman\makehuman.py ", newgeneratedMHMPath, listValuesCSV.nom, ".mhm");
            UnityEngine.Debug.Log(command);
            process.StandardInput.WriteLine(command);
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();
            int ExitCode = process.ExitCode;
            if (ExitCode != 0) { UnityEngine.Debug.Log("Erreur dans le génération du modèle 3D : erreur " + ExitCode); } else { UnityEngine.Debug.Log("Paramètres anthropométriques envoyés à MakeHuman"); }
        }
    }

    private IEnumerator isMakeHumanRunning()
    {
        UnityEngine.Debug.Log("isMakeHumanRunning");
        System.IO.Directory.CreateDirectory(generatedFBXPath + listValuesCSV.nom);
        bool isMakeHumanRunning = Process.GetProcessesByName("makehuman").Length == 0;
        yield return new WaitUntil(() => isMakeHumanRunning);
    }

    private IEnumerator callPortableMakeHuman()
    {

        using (Process process = new Process())
        {
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.FileName = Path.Combine(Environment.SystemDirectory, "cmd.exe");
            // Redirects the standard input so that commands can be sent to the shell.
            process.StartInfo.RedirectStandardInput = true;
            // Runs the specified command and exits the shell immediately.
            process.EnableRaisingEvents = true;
            process.Start();
            string newmakeHumanPath = makeHumanPath.Replace(@"\", @"/");
            string newgeneratedMHMPath = generatedMHMPath.Replace(@"\", @"/");
            process.StandardInput.WriteLine("cd " + newmakeHumanPath);
            string command = string.Concat("makehuman.exe " + newgeneratedMHMPath, listValuesCSV.nom, ".mhm");
            UnityEngine.Debug.Log(command);
            process.StandardInput.WriteLine(command);
            process.StandardInput.WriteLine("exit");
            process.WaitForExit();
            int ExitCode = process.ExitCode;
            if (ExitCode != 0) { UnityEngine.Debug.Log("Erreur dans le génération du modèle 3D : erreur " + ExitCode); } else { UnityEngine.Debug.Log("Paramètres anthropométriques envoyés à MakeHuman"); }
        }
        yield return new WaitForSeconds(2);
        StartCoroutine(isMakeHumanRunning());
    }



    void GenerateMHM()
    {
        List<string> lines = null;
        string[] mhmfile = null;
        // 0=female , 1=male
        if (listValuesCSV.genre == 0)
        {
            mhmfile = System.IO.File.ReadAllLines(templateMHMPath + "female_template.mhm");
        }
        if (listValuesCSV.genre == 1)
        {
            mhmfile = System.IO.File.ReadAllLines(templateMHMPath + "male_template.mhm");
        }

        lines = new List<string>(mhmfile);
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Contains("tags"))
            {
                lines[i] = "tags " + listValuesCSV.nom;
            }
            if (lines[i].Contains("Height"))
            {
                lines[i] = lines[i].Substring(0, lines[i].Length - 8) + String.Format("{0:0.000000}", listValuesCSV.taille);
                lines[i] = lines[i].Replace(",", ".");
            }
            //if (lines[i].Contains("BreastSize"))
            //{
            //    lines[i] = lines[i].Substring(0, lines[i].Length - 8) + String.Format("{0:0.000000}", listValuesCSV.seins);
            //}
            if (lines[i].Contains("Age"))
            {
                lines[i] = lines[i].Substring(0, lines[i].Length - 8) + String.Format("{0:0.000000}", listValuesCSV.age);
                lines[i] = lines[i].Replace(",", ".");
            }
            //if (lines[i].Contains("neck-circ"))
            //{
            //    lines[i] = lines[i].Substring(0, lines[i].Length - 8) + String.Format("{0:0.000000}", listValuesCSV.couCirconference);
            //}
            //if (lines[i].Contains("bust-circ"))
            //{
            //    lines[i] = lines[i].Substring(0, lines[i].Length - 8) + String.Format("{0:0.000000}", listValuesCSV.busteCirconference);
            //}
            //if (lines[i].Contains("hips-circ"))
            //{
            //    lines[i] = lines[i].Substring(0, lines[i].Length - 8) + String.Format("{0:0.000000}", listValuesCSV.tailleCirconference);
            //}
            if (lines[i].Contains("Muscle"))
            {
                lines[i] = lines[i].Substring(0, lines[i].Length - 8) + String.Format("{0:0.000000}", listValuesCSV.muscle);
                lines[i] = lines[i].Replace(",", ".");
            }
            if (lines[i].Contains("Weight"))
            {
                lines[i] = lines[i].Substring(0, lines[i].Length - 8) + String.Format("{0:0.000000}", listValuesCSV.graisse);
                lines[i] = lines[i].Replace(",", ".");
            }

        }

        using (StreamWriter writer = new StreamWriter(generatedMHMPath + listValuesCSV.nom + ".mhm", false))
        {
            foreach (string line in lines)
            {
                writer.WriteLine(line);
            }
        }


    }

    void modifyPathExportMakeHuman(string dataMakeHumanPath)
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string subFolderPath = Path.Combine(path, dataMakeHumanPath);
        string documentsFile = subFolderPath + "settings.ini";
        string usedFile = documentsFile;
        if (!File.Exists(documentsFile))
        {
            usedFile = templateMHMPath + "settings.ini";
        }


        string[] settingsFile = System.IO.File.ReadAllLines(usedFile);

        List<string> lines = new List<string>(settingsFile);
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].Contains("exportdir"))
            {
                int index = lines[i].IndexOf(@""":");
                lines[i] = lines[i].Substring(0, index + 3) + @"""" + generatedFBXPath + listValuesCSV.nom + @""",";
            }
        }

        System.IO.Directory.CreateDirectory(subFolderPath);
        using (StreamWriter writer = new StreamWriter(documentsFile, false))
        {
            foreach (string line in lines)
            {
                writer.WriteLine(line);
            }
        }


    }

}

   

