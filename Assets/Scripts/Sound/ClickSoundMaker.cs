using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickSoundMaker : MonoBehaviour, IPointerDownHandler, ISound
{
    public Action makeSound { get; set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        makeSound();
    }
}
