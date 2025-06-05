using System.Collections.Generic;
using UnityEngine;

public class RuntimeRefWrapper : MonoBehaviour
{
    [SerializeField]
    List<GameObject> theRefs;

    public bool GetReference<T>(out List<T> result)
    {
        result = new List<T>();
        for (int i = 0; i < theRefs.Count; i++)
        {
            if (theRefs[i].TryGetComponent(out T component))
            {
                result.Add(component);
            }
        }
        return result.Count > 0;
    }

    public void AddRefToWrapper(GameObject obj)
    {
        theRefs.Add(obj);
    }
}

public interface INeedRefRuntime
{
    void Init(RuntimeRefWrapper refProvider);
}