using UnityEngine;
using UnityEngine.UI;

public class SliderDataWrapper : MonoBehaviour
{
    [SerializeField]
    Slider backMusics;
    [SerializeField]
    Slider sfx;

    void Start()
    {
        DataManager.instance.SetBackSoundsVolume(backMusics.value);
        DataManager.instance.SetSoundEffectsVolume(sfx.value);
        gameObject.SetActive(false);
    }
}
