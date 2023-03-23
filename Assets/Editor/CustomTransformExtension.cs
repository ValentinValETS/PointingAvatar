using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using UnityEngine.SocialPlatforms;

[CustomEditor(typeof(Transform))]
[CanEditMultipleObjects]
[ExecuteInEditMode]
public class CustomTransformExtension : Editor
{
    //Built-in editor
    Editor defaultEditor;

    Transform transform;

    bool localSpace = true;
    bool worldSpace = false;

    void OnEnable()
    {
        //Called when inspector is created
        defaultEditor = Editor.CreateEditor(targets, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
        transform = target as Transform;
    }

    void OnDisable()
    {
        //Prevents memory leakage
        MethodInfo disableMethod = defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        if (disableMethod != null)
        {
            disableMethod.Invoke(defaultEditor, null);
        }

        DestroyImmediate(defaultEditor);
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        if(transform.parent == null)
        {
            defaultEditor.OnInspectorGUI();
        }
        else
        {
            localSpace = Foldout("Local Space", localSpace);

            if (localSpace)
            {
                defaultEditor.OnInspectorGUI();
            }

            worldSpace = Foldout("World Space", worldSpace);
            if (worldSpace)
            {
                Vector3 worldPosition = transform.position;
                Quaternion worldRotation = transform.rotation;
                Vector3 worldScale = transform.lossyScale;

                //Display world position (but do not allow editing)
                EditorGUILayout.Vector3Field("World Position", worldPosition);
                //Display world rotation (but do not allow editing)
                EditorGUILayout.Vector3Field("World Rotation", worldRotation.eulerAngles);
                //Display world scale (but do not allow editing)
                EditorGUILayout.Vector3Field("World Scale", worldScale);

            }
        }
    }

    public static bool Foldout(string title, bool display)
    {
        var style = new GUIStyle("ShurikenModuleTitle");
        style.font = new GUIStyle(EditorStyles.label).font;
        style.border = new RectOffset(15, 7, 4, 4);
        style.fixedHeight = 22;
        style.contentOffset = new Vector2(20f, -2f);

        var rect = GUILayoutUtility.GetRect(16f, 22f, style);
        GUI.Box(rect, title, style);

        var e = Event.current;

        var toggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
        if (e.type == EventType.Repaint)
        {
            EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
        }

        if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
        {
            display = !display;
            e.Use();
        }

        return display;
    }
}