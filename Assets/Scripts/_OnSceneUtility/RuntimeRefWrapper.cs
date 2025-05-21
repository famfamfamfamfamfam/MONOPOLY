using System.Collections.Generic;
using UnityEngine;

public class RuntimeRefWrapper : MonoBehaviour
{
    [SerializeField]
    GameObject[] theRefs;

    public bool GetReference<T>(out List<T> result)
    {
        result = new List<T>();
        for (int i = 0; i < theRefs.Length; i++)
        {
            if (theRefs[i].TryGetComponent(out T component))
            {
                result.Add(component);
            }
        }
        return result.Count > 0;
    }
}

public interface INeedRefRuntime
{
    void Init(RuntimeRefWrapper refProvider);
}