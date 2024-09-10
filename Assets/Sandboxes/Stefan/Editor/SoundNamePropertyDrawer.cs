using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SoundName))]
public class SoundNamePropertyDrawer : PropertyDrawer
{
    SoundNamesContainer _soundNamesContainer;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if(_soundNamesContainer == null)
            _soundNamesContainer = Resources.Load("SoundNames") as SoundNamesContainer;

        SerializedProperty nameProperty = property.FindPropertyRelative("Name");
        SerializedProperty indexProperty = property.FindPropertyRelative("index");

        EditorGUI.BeginProperty(position, label, property);
        if (position.Contains(Input.mousePosition))
            Debug.Log("in");
        indexProperty.intValue = EditorGUI.Popup(position, indexProperty.intValue, _soundNamesContainer.Names);
        nameProperty.stringValue = _soundNamesContainer.Names[indexProperty.intValue];
        
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
