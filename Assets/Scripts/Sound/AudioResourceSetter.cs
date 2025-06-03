using UnityEngine;

public class AudioResourceSetter : MonoBehaviour
{
    AudioSource source;
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void SetUpTheAudio(AudioClip clip = null)
    {
        if (clip != null)
        {
            source.resource = clip;
        }
        source.Play();
    }
}
