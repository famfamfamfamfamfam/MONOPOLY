using UnityEngine;

public class SoundAdjustor : MonoBehaviour
{
    [SerializeField]
    AudioSource source;

    public void ChangeVolumeTo(float value)
    {
        source.volume = value;
    }
}
