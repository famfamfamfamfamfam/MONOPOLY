using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PivotAdjustor : ScriptableWizard
{
    public GameObject parent;
    public Vector3 offsetToPlus;

    Transform[] plusTransforms;

    private void OnWizardCreate()
    {
        plusTransforms = parent.GetComponentsInChildren<Transform>();
        foreach (Transform t in plusTransforms)
        {
            t.position += offsetToPlus;
        }
    }

    [MenuItem("Tools/My Wizard")]
    static void ShowWizard()
    {
        DisplayWizard<PivotAdjustor>("Transform Position", "Adjust");
    }
}