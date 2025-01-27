using UnityEngine;
using UnityEditor;

public class TurnOnInstancing : ScriptableWizard
{
    void OnWizardCreate()
    {
        string[] strings = AssetDatabase.FindAssets("t:Material");
        for (int i = 0; i < strings.Length; i++)
        {
            Material m = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(strings[i]));
            m.enableInstancing = true;
            AssetDatabase.SaveAssetIfDirty(m);
        }
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/My Wizard/Materials Instancing")]
    static void ShowWizard()
    {
        DisplayWizard<TurnOnInstancing>("Turn on Instancing", "Turn on");
    }
}
