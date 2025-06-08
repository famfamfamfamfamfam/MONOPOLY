using UnityEngine;

[CreateAssetMenu(fileName = "NewSound", menuName = "Scriptable Objects/Sound")]
public class Sound : ScriptableObject
{
    public string soundName;
    public AudioClip theSound;
}
