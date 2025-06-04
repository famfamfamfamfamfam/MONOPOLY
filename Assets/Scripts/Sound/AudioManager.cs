using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum CurrentScene : byte
    {
        Menu,
        GamePlay
    }
    [SerializeField]
    CurrentScene scene;
    [SerializeField]
    AudioClip[] backSounds;
    [SerializeField]
    AudioResourceSetter backSourceSetter;
    [SerializeField]
    AudioResourceSetter clickSFX_setter;
    [SerializeField]
    MonoBehaviour[] makeSoundInvokers;

    void Start()
    {
        switch (scene)
        {
            case CurrentScene.Menu:
                SetAudioAtStart(UnityEngine.Random.Range(0, backSounds.Length));
                break;
            case CurrentScene.GamePlay:
                if (DataManager.instance.shuffleMusicsPlay)
                {
                    SetAudioAtStart(UnityEngine.Random.Range(0, backSounds.Length));
                }
                else
                {
                    SetAudioAtStart(0);
                }
                break;
        }
        for (int i = 0; i < makeSoundInvokers.Length; i++)
        {
            if (makeSoundInvokers[i] is ISound invoker)
            {
                invoker.makeSound = () => clickSFX_setter.SetUpTheAudio();
            }
        }
    }
    void SetAudioAtStart(int index)
    {
        backSourceSetter.SetUpTheAudio(backSounds[index]);
    }


}

public interface ISound
{
    Action makeSound { get; set; }
}