using System;
using UnityEngine;
using UnityEngine.UI;

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
        if (scene == CurrentScene.Menu)
        {
            backSourceSetter.SetUpTheAudio(backSounds[UnityEngine.Random.Range(0, backSounds.Length)]);
        }
        for (int i = 0; i < makeSoundInvokers.Length; i++)
        {
            if (makeSoundInvokers[i] is ISound invoker)
            {
                invoker.makeSound = () => clickSFX_setter.SetUpTheAudio();
            }
        }
    }



}

public interface ISound
{
    Action makeSound { get; set; }
}