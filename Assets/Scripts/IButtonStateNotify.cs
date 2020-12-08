using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IButtonStateNotify : IEventSystemHandler
{
    // ボタンの状態をセットするイベントを通知します。
    void SetButtonState(bool isActive);
}
