using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemSO))]
public class ItemSOEditor : Editor
{
    ItemSO comp;

    public void OnEnable()
    {
        comp = (ItemSO)target;
    }

    public override void OnInspectorGUI()
    {
        comp.ItemName = EditorGUILayout.TextField("Name", comp.ItemName);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            comp.Sprite = (Sprite)EditorGUILayout.ObjectField(comp.Sprite, typeof(Sprite), false, GUILayout.Width(64f), GUILayout.Height(64f));
        }
        EditorGUILayout.EndHorizontal();

        EditorUtility.SetDirty(comp);
    }

}
