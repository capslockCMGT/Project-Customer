using UnityEditor;
using UnityEngine;

public class SoundNamesWindow : EditorWindow
{
    [MenuItem("Stefan/SoundNames")]
    public static void ShowWindow()
    {
        GetWindow<SoundNamesWindow>("SoundNames");
    }

    public string[] Names = new string[0];

    private void OnGUI()
    {
        SoundNamesContainer container = Resources.Load("SoundNames") as SoundNamesContainer;

        if(container == null)
        {
            EditorGUILayout.LabelField("There is no SoundNamesContainer Scriptable Object called SoundNames in Resources folder!");
            return;
        }
        SerializedObject so = new (container);
        SerializedProperty stringsProperty = so.FindProperty("Names");

        EditorGUILayout.PropertyField(stringsProperty, true);
        so.ApplyModifiedProperties(); 
    }
}
