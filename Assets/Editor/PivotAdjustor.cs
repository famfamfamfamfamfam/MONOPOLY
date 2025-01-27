using UnityEngine;
using UnityEditor;

public class PivotAdjustor : ScriptableWizard
{
    public GameObject parent;
    public Vector3 offsetToPlus;

    Transform[] plusTransforms;

    private void OnWizardCreate()
    {
        parent.transform.position = Vector3.zero;
        plusTransforms = parent.GetComponentsInChildren<Transform>();
        foreach (Transform t in plusTransforms)
        {
            t.position += offsetToPlus;
        }
    }

    [MenuItem("Tools/My Wizard/Transform Position Add Value")]
    static void ShowWizard()
    {
        DisplayWizard<PivotAdjustor>("Transform Position", "Adjust");
    }
}