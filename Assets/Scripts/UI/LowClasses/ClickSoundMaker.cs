using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickSoundMaker : MonoBehaviour, IPointerClickHandler, ISound
{
    public Action makeSound { get; set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        makeSound();
    }
}