using UnityEditor;
using UnityEngine;
using System.Collections;


[CustomEditor(typeof(MonoRestreamAPI))]
[CanEditMultipleObjects]
public class EditorRestreamAPI : Editor
{
    SerializedProperty damageProp;

    
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        
    }

    
}