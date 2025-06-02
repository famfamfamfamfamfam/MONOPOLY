using UnityEngine;

public class CursorOptionUpdater : MonoBehaviour
{
    [SerializeField]
    CursorMover mover;

    int currentOption;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            mover.MoveToRight();
            currentOption++;
            DataManager.instance.SetGamerPlayIndex(ref currentOption, mover.optionCount);
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            mover.MoveToLeft();
            currentOption--;
            DataManager.instance.SetGamerPlayIndex(ref currentOption, mover.optionCount);
        }
    }
}
