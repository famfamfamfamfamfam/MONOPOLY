using System;
using UnityEngine;
using UnityEngine.Events;

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
    Sound[] backSounds;
    [SerializeField]
    AudioResourceSetter backSourceSetter;
    [SerializeField]
    AudioResourceSetter clickSFX_setter;
    [SerializeField]
    MonoBehaviour[] makeSoundInvokers;

    [SerializeField]
    UnityEvent<string> onUpdateSong;

    int currentSongIndex = -1;
    Sound tmp;

    void Start()
    {
        switch (scene)
        {
            case CurrentScene.Menu:
                SetUpAudio(UnityEngine.Random.Range(0, backSounds.Length));
                break;
            case CurrentScene.GamePlay:
                SetVolumes();
                PlayAudioOnModes(true);
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

    void SetUpAudio(int index)
    {
        backSourceSetter.SetUpTheAudio(backSounds[index].theSound);
    }

    void SetVolumes()
    {
        SoundAdjustor[] volumeAdjustors = gameObject.GetComponentsInChildren<SoundAdjustor>();
        for (int i = 0; i < volumeAdjustors.Length; i++)
        {
            if (volumeAdjustors[i].gameObject == backSourceSetter.gameObject)
            {
                volumeAdjustors[i].ChangeVolumeTo(DataManager.instance.backSoundsVolume);
                continue;
            }
            if (volumeAdjustors[i].gameObject == clickSFX_setter.gameObject)
            {
                volumeAdjustors[i].ChangeVolumeTo(DataManager.instance.SFX_volume);
            }
        }
    }

    public void PlayAudioOnModes(bool atStart)
    {
        if (DataManager.instance.shuffleMusicsPlay)
        {
            currentSongIndex = atStart ? UnityEngine.Random.Range(0, backSounds.Length) : UnityEngine.Random.Range(1, backSounds.Length);
            tmp = backSounds[0];
            backSounds[0] = backSounds[currentSongIndex];
            backSounds[currentSongIndex] = tmp;
            currentSongIndex = 0;
        }
        else
        {
            currentSongIndex = currentSongIndex >= backSounds.Length ? 0 : currentSongIndex + 1;
        }
        if (currentSongIndex >= backSounds.Length)
        {
            currentSongIndex %= backSounds.Length;
        }
        SetUpAudio(currentSongIndex);
        onUpdateSong?.Invoke(backSounds[currentSongIndex].soundName);
    }

    void Update()
    {
        if (scene == CurrentScene.Menu)
        {
            return;
        }

        if (!backSourceSetter.IsPlaying())
        {
            PlayAudioOnModes(false);
        }
    }

}

public interface ISound
{
    Action makeSound { get; set; }
}