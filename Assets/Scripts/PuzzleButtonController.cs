using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzleButtonController : MonoBehaviour, IButtonStateNotify
{
    // このスクリプトがアタッチされたオブジェクトのボタンIDです。
    int buttonId;

    GameObject managerObj;

    public Button buttonComp;

    public void SetButtonState(bool isActive)
    {
        // ボタンの状態をセットします。
        buttonComp.interactable = isActive;
    }

    public void SetButtonId(int id)
    {
        buttonId = id;
    }

    public void OnPressedPuzzleButton()
    {
        // 連打を防ぐため、このボタンを押せないようにします。
        SetButtonState(false);

        // ボタンのIDをマネージャーに通知します。
        NotifyButtonActivateEvent(managerObj);
    }

    public void SetManagerReference(GameObject obj)
    {
        // マネージャーオブジェクトへの参照をセットします。
        managerObj = obj;
    }


    void NotifyButtonActivateEvent(GameObject targetObj)
    {
        ExecuteEvents.Execute<IButtonNotify>(
                        target: targetObj,
                        eventData: null,
                        functor: CallButtonPressedEvent
                        );
    }

    void CallButtonPressedEvent(IButtonNotify inf, BaseEventData eventData)
    {
        inf.OnPressedPuzzleButton(buttonId);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
