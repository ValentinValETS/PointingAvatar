using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityVicon;

public class ModifyRig : MonoBehaviour
{
#if UNITY_EDITOR

    public GameObject originalRig;
    private Transform[] limbs;
    private string unityPath;
    private string destinationPath;


    public void CreateAvatar(ViconDataStreamClient client,GameObject originalRig)
    {
        unityPath = Application.dataPath;
        destinationPath = unityPath.Replace("/Assets", "/Assets/MakeHumanModels/" + originalRig.name + "/");
        System.IO.Directory.CreateDirectory(destinationPath);
        string prefabPath = destinationPath + originalRig.name + ".prefab";
        limbs = originalRig.GetComponentsInChildren<Transform>();
        ChangeNames(client,originalRig);
        originalRig.tag = "Avatar";
        SubjectScript subjectScript = originalRig.AddComponent<SubjectScript>();
        subjectScript.Client = client;
        subjectScript.SubjectName = "Pegasus_S";
        PrefabUtility.SaveAsPrefabAssetAndConnect(originalRig, prefabPath, InteractionMode.UserAction);
        Debug.Log("Rig Modèle 3D créé dans : " + prefabPath);
    }

    public void CreateAvatarOriginal(ViconDataStreamClient client, GameObject originalRig)
    {
        unityPath = Application.dataPath;
        destinationPath = unityPath.Replace("/Assets", "/Assets/MakeHumanModels/" + originalRig.name + "/");
        System.IO.Directory.CreateDirectory(destinationPath);
        string prefabPath = destinationPath + originalRig.name + "_original" + ".prefab";
        limbs = originalRig.GetComponentsInChildren<Transform>();
        foreach (Transform limb in limbs)
        {
            if (limb.gameObject.GetComponent<SkinnedMeshRenderer>() != null)
            {
                limb.gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            }
        }
        originalRig.tag = "Original";
        SubjectScript subjectScript = originalRig.AddComponent<SubjectScript>();
        subjectScript.Client = client;
        subjectScript.SubjectName = "Pegasus_S";

        PrefabUtility.SaveAsPrefabAssetAndConnect(originalRig, prefabPath, InteractionMode.UserAction);
    }



    private void ChangeNames(ViconDataStreamClient client ,GameObject originalRig)
    {
        foreach (Transform limb in limbs)
        {
            limb.gameObject.layer = LayerMask.NameToLayer("Body");

            if (limb.gameObject.name == "Unity compliant skeleton")
            {
     
            }
            if (limb.gameObject.name == "hips")
            {

            }

            if (limb.gameObject.name == "spine")
            {

            }

            if (limb.gameObject.name == "head")
            {

            }

            if (limb.gameObject.name == "thigh.L")
            {

            }
            if (limb.gameObject.name == "shin.L")
            {

            }
            if (limb.gameObject.name == "foot.L")
            {

                
            }

            if (limb.gameObject.name == "upper_arm.L")
            {
                
            }
            if (limb.gameObject.name == "forearm.L")
            {
                limb.gameObject.AddComponent<ArmIK>();
            }
            if (limb.gameObject.name == "hand.L")
            {
  
            }


            if (limb.gameObject.name == "thigh.R")
            {

            }
            if (limb.gameObject.name == "shin.R")
            {

            }
            if (limb.gameObject.name == "foot.R")
            {

              
            }
 
            if (limb.gameObject.name == "upper_arm.R")
            {

            }
            if (limb.gameObject.name == "forearm.R")
            {
                limb.gameObject.AddComponent<ArmIK>();
            }
            if (limb.gameObject.name == "hand.R")
            {
  
            }
                        
        }



    }

#endif
}
