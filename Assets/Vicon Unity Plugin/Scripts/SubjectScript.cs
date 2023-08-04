using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using ViconDataStreamSDK.CSharp;


namespace UnityVicon
{
  public class SubjectScript : MonoBehaviour
  {
        public string SubjectName = "";
    
        private bool IsScaled = true;

        private float shoulderAngleX;
        private float elbowAngleX;
        private Quaternion shoulderRotation;
        private Quaternion elbowRotation;

        public ViconDataStreamClient Client;

        public float ShoulderAngleX { get => shoulderAngleX; set => shoulderAngleX = value; }
        public float ElbowAngleX { get => elbowAngleX; set => elbowAngleX = value; }
        public Quaternion ShoulderRotation { get => shoulderRotation; set => shoulderRotation = value; }
        public Quaternion ElbowRotation { get => elbowRotation; set => elbowRotation = value; }

        private void Awake()
        {
            shoulderAngleX = 0f;
            elbowAngleX = 0f;
            shoulderRotation = Quaternion.identity;
            elbowRotation = Quaternion.identity;
        }


        private void Start()
        {
            Client = GameObject.Find("ViconDataStreamPrefab").GetComponent<ViconDataStreamClient>();
        }

        public SubjectScript()
        {
        }

        void LateUpdate()
        {
          Output_GetSubjectRootSegmentName OGSRSN = Client.GetSubjectRootSegmentName(SubjectName);
          Transform Root = transform.root;
          //transform.root.rotation = Quaternion.Euler(-90f,180f,0f);
          FindAndTransform( Root, OGSRSN.SegmentName);
        }

        string strip(string BoneName)
        {
          if (BoneName.Contains(":"))
          {
            string[] results = BoneName.Split(':');
            return results[1];
          }
          return BoneName;
        }
        void FindAndTransform(Transform iTransform, string BoneName )
        {
          int ChildCount = iTransform.childCount;
          for (int i = 0; i < ChildCount; ++i)
          {
            Transform Child = iTransform.GetChild(i);
            if( strip( Child.name) == BoneName )
            { 
              ApplyBoneTransform(Child);
              TransformChildren(Child);
              break;
            }
            // if not finding root in this layer, try the children
            FindAndTransform(Child, BoneName);
          }
        }
    void TransformChildren(Transform iTransform )
    {
      int ChildCount = iTransform.childCount;
      for (int i = 0; i < ChildCount; ++i)
      {
        Transform Child = iTransform.GetChild(i);
        ApplyBoneTransform(Child);
        TransformChildren(Child);
      }
    }
      // map the orientation back for forward

    private void ApplyBoneTransform(Transform Bone)
    {
      string BoneName = strip(Bone.gameObject.name);
      // update the bone transform from the data stream
      Output_GetSegmentLocalRotationQuaternion ORot = Client.GetSegmentRotation(SubjectName, BoneName );
      if (ORot.Result == Result.Success)
      {
        // mapping back to default data stream axis
        //Quaternion Rot = new Quaternion(-(float)ORot.Rotation[2], -(float)ORot.Rotation[0], (float)ORot.Rotation[1], (float)ORot.Rotation[3]);
        Quaternion Rot = new Quaternion((float)ORot.Rotation[0], (float)ORot.Rotation[1], (float)ORot.Rotation[2], (float)ORot.Rotation[3]);
                // mapping right hand to left hand flipping x

                if (BoneName == "hips" || BoneName == "thigh.R" || BoneName == "thigh.L" || BoneName == "spine" || BoneName == "chest" || BoneName == "chest1")
                {
                    //DO NOTHING
                }
                else
                {

                    Bone.localRotation = new Quaternion(Rot.x, -Rot.y, -Rot.z, Rot.w);
                }
                
                // Fix the avatar to be in a seating position
                if (BoneName == "thigh.R")
                {
                    Bone.localRotation = Quaternion.Euler(-260f, 0f, 0f);
                }
                if (BoneName == "thigh.L")
                {
                    Bone.localRotation = Quaternion.Euler(-260f, 0f, 0f);
                }
                if (BoneName == "shin.R")
                {
                    Bone.localRotation = Quaternion.Euler(70f, 0f, 0f);
                }
                if (BoneName == "shin.L")
                {
                    Bone.localRotation = Quaternion.Euler(70f, 0f, 0f);
                }

                //Applies the rotations to the arms and forearms
                // AJOUTER IF DOMINANT HAND == ...
                if (BoneName == "upper_arm.R")
                {
                    Bone.localRotation = new Quaternion(Rot.x, -Rot.y, -Rot.z, Rot.w);
                }
                if (BoneName == "forearm.R")
                {
                    Bone.localRotation = new Quaternion(Rot.x, -Rot.y, -Rot.z, Rot.w);
                }
                if (BoneName == "upper_arm.L")
                {
                    Bone.localRotation = new Quaternion(Rot.x, -Rot.y, -Rot.z, Rot.w);
                }
                if (BoneName == "forearm.L")
                {
                    Bone.localRotation = new Quaternion(Rot.x, -Rot.y, -Rot.z, Rot.w);
                }
            }

      Output_GetSegmentLocalTranslation OTran;
      if (IsScaled)
      {
        OTran = Client.GetScaledSegmentTranslation(SubjectName, BoneName);
      }
      else
      {
        OTran = Client.GetSegmentTranslation(SubjectName, BoneName);
      }

      if (OTran.Result == Result.Success)
      {
        //Vector3 Translate = new Vector3(-(float)OTran.Translation[2] * 0.001f, -(float)OTran.Translation[0] * 0.001f, (float)OTran.Translation[1] * 0.001f);
        Vector3 Translate = new Vector3((float)OTran.Translation[0] * 0.001f, (float)OTran.Translation[1] * 0.001f, (float)OTran.Translation[2] * 0.001f);
                if (BoneName == "hips" || BoneName == "thigh.R" || BoneName == "thigh.L" || BoneName == "spine" || BoneName == "chest" || BoneName == "chest1")
                {
                    //DO NOTHING
                }
                else
                {

                    Bone.localPosition = new Vector3(-Translate.x, Translate.y, Translate.z);
                }
 

                //if (BoneName == "hips")
                //{
                //    Bone.localPosition = new Vector3(0f,0f,0f);
                //}
                //if (BoneName == "thigh.R")
                //{
                //    Bone.localPosition = new Vector3(0f, 0f, 0f);
                //}
                //if (BoneName == "thigh.L")
                //{
                //    Bone.localPosition = new Vector3(0f, 0f, 0f);
                //}
                //if (BoneName == "shin.R")
                //{
                //    Bone.localPosition = new Vector3(0f, 0f, 0f);
                //}
                //if (BoneName == "shin.L")
                //{
                //    Bone.localPosition = new Vector3(0f, 0f, 0f);
                //}
            }

      // If there's a scale for this subject in the datastream, apply it here.
      if (IsScaled)
      {
        Output_GetSegmentStaticScale OScale = Client.GetSegmentScale(SubjectName, BoneName);
        if (OScale.Result == Result.Success)
        {
          Bone.localScale = new Vector3((float)OScale.Scale[0], (float)OScale.Scale[1], (float)OScale.Scale[2]);
        }
      }
    }
  } //end of program
}// end of namespace

