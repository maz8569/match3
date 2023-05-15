using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RecipeSO))]
public class RecipeSOEditor : Editor
{
    RecipeSO comp;

    public void OnEnable()
    {
        comp = (RecipeSO)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        comp.RecipeName = EditorGUILayout.TextField("Name", comp.RecipeName);

        comp.Sprite = (Sprite)EditorGUILayout.ObjectField(comp.Sprite, typeof(Sprite), false, GUILayout.Width(64f), GUILayout.Height(64f));

        EditorUtility.SetDirty(comp);
    }

}
