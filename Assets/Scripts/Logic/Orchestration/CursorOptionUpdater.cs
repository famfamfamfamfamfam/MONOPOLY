using System;
using UnityEngine;

public class CursorOptionUpdater : MonoBehaviour, ISound
{
    [SerializeField]
    CursorMover mover;

    int currentOption;

    public Action makeSound { get; set; }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            mover.MoveToRight();
            currentOption++;
            DataManager.instance.SetGamerPlayIndex(ref currentOption, mover.optionCount);
            makeSound();
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            mover.MoveToLeft();
            currentOption--;
            DataManager.instance.SetGamerPlayIndex(ref currentOption, mover.optionCount);
            makeSound();
        }
    }
}
