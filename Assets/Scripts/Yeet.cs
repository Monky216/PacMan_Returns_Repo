using UnityEngine;
using UnityEditor;
using System.Collections;

public class Yeet : ScriptableWizard
{
    public bool copyValues = true;
    public GameObject useGameObject;
    public GameObject Replace;

    [MenuItem("Custom/Replace GameObjects")]

    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard("Replace GameObjects", typeof(GameObject), "Replace");
    }

    void OnWizardCreate()
    {
        Transform[] Replaces;
        Replaces = Replace.GetComponentsInChildren<Transform>();

        foreach (Transform t in Replaces)
        {
            GameObject newObject;
            newObject = (GameObject)EditorUtility.InstantiatePrefab(useGameObject);
            newObject.transform.position = t.position;
            newObject.transform.rotation = t.rotation;

            Destroy(t.gameObject);
        }
    }
}
