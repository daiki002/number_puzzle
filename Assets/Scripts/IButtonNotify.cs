using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IButtonNotify : IEventSystemHandler
{
    // ボタンが押されたことをマネージャーに通知します。
    void OnPressedPuzzleButton(int buttonId);
}
