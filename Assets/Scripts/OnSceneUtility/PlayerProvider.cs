using System.Collections.Generic;
using UnityEngine;

public class PlayerProvider : MonoBehaviour, INeedRefRuntime
{
    [SerializeField]
    PlayerManager playerManager;

    public void Init(RuntimeRefWrapper refProvider)
    {
        if (refProvider.GetReference(out List<PlayerController> result))
        {
            if (result.Count == 1)
            {
                playerManager.AddRef(result[0].playerIndex, result[0]);
            }
            else
            {
                Debug.LogAssertion("Cannot get the reference cause of not only one element");
            }
        }
    }
}
