using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class PuzzleManager : ManagerBase, IButtonNotify
{
    // パズルを隠すマスク画像への参照をセットします。
    public GameObject pazzleMask;

    // 画面を隠すマスク画像への参照を保持します。
    public GameObject screenMask;

    // ボーズパネルへの参照をセットします。
    public GameObject pausePanelObj;

    // ギブアップパネルへの参照をセットします。
    public GameObject giveupPanelObj;

    // リザルトパネルへの参照をセットします。
    public GameObject resultPanelObj;

    // リザルトパネルのタイマーを表示するテキストへの参照を保持します。
    public TextMeshProUGUI resultTimerText;

    // リザルトパネルのベストタイムを表示するテキストへの参照を保持します。
    public TextMeshProUGUI resultBestTimeText;

    // 記録を更新したときに表示するテキストオブジェクトへの参照を保持します。
    public GameObject resultNewRecordText;

    // 現在のパズルモードを表示するテキストへの参照を保持します。
    public TextMeshProUGUI modeText;

    // ポーズボタンのButtonコンポーネントへの参照を保持します。
    public Button pauseButton;

    // 再開ボタンのButtonコンポーネントへの参照を保持します。
    public Button resumeButton;

    // ギブアップボタンのButtonコンポーネントへの参照を保持します。
    public Button giveupButton;

    // ギブアップパネルにあるタイトルへボタンのButtonコンポーネントへの参照を保持します。
    public Button goTitleButton;

    // ギブアップパネルにあるキャンセルボタンのButtonコンポーネントへの参照を保持します。
    public Button cancelButton;

    // リザルトパネルにあるOKボタンのButtonコンポーネントへの参照を保持します。
    public Button okButton;

    // PuzzleDataManagerへの参照を保持するフィールドです。
    public PuzzleDataManager puzzleDataManager;

    // TimerManagerスクリプトへの参照を保持します。
    public TimeManager timeManager;

    // パズルピースの移動にかかる時間を保持するフィールドです。
    public float pieceAnimTime = 0.1f;

    void Start()
    {
        // 画面のボタンの状態をセットアップします。
        SetButtonInactive();

        // モードの確認を行います。
        CheckPuzzleMode();

        // パズルボタンのセットアップを行います。
        puzzleDataManager.SetUpPuzzleButtons();

        // パズルピースのセットアップを行います。
        puzzleDataManager.SetUpPuzzlePieces();

        // パズルボタンを押せないようにします。
        SetAllButtonInactive();

        // 現在のスロットIDとピースIDの辞書を表示します。
        // puzzleDataManager.ShowSlotState();

        // 並べ替えのプロセスを開始します。
        StartCoroutine(ShuffleProcess());
    }

    void Update()
    {

    }

    /// <Summary>
    /// スロットの中身を並び替える一連の動作を定義します。
    /// </Summary>
    IEnumerator ShuffleProcess()
    {
        // 画像のアルファ(透明度)の値を定義します。
        float maskAlpha = 0f;

        // フェードのアニメーションにかかる時間を定義します。
        float animTime = 0.5f;

        // 画面マスク画像をフェードアウトさせます。
        yield return StartCoroutine(FadeAnimation(screenMask, maskAlpha, animTime));

        // 完成形のパズルを表示させるため、処理を待ちます。
        yield return new WaitForSeconds(animTime);

        // スロットの中身を並べ替えます。
        yield return StartCoroutine(puzzleDataManager.ShuffleSlot());

        // 画像のアルファ(透明度)の値を定義します。
        maskAlpha = 1.0f;

        // パズルピースのマスク画像を表示します。
        yield return StartCoroutine(FadeAnimation(pazzleMask, maskAlpha, animTime));

        // ピースオブジェクトをシャッフル後の位置にセットします。
        puzzleDataManager.MovePiecePos();

        // 演出用に1秒待ちます。
        yield return new WaitForSeconds(1.0f);

        // マスク画像を非表示にします。
        maskAlpha = 0f;
        yield return StartCoroutine(FadeAnimation(pazzleMask, maskAlpha, animTime));

        // ゲームの開始処理を行います。
        StartGameProcess();

        // 空きスロットの周辺のボタンを有効化します。
        CheckAroundEmptySlot();
    }

    /// <Summary>
    /// ゲーム開始の処理を行います。
    /// </Summary>
    void StartGameProcess()
    {
        // タイマーを開始します。
        timeManager.StartTimer();

        // ポーズボタンを押せるようにします。
        pauseButton.interactable = true;

        // ギブアップボタンを押せるようにします。
        giveupButton.interactable = true;
    }

    /// <Summary>
    /// 全てのボタンを一律で押せないようにします。
    /// </Summary>
    void SetAllButtonInactive()
    {
        // ボタン辞書を取得します。
        Dictionary<int, GameObject> buttonIdObjDict = puzzleDataManager.GetButtonIdObjDict();

        // ボタン辞書に含まれるボタンオブジェクトを押せないようにします。
        foreach (KeyValuePair<int, GameObject> pair in buttonIdObjDict)
        {
            // イベントを通知するメソッドを実行します。
            NotifyButtonInactivateEvent(pair.Value);
        }
    }

    /// <Summary>
    /// ボタンを押せないようにするイベントを対象のオブジェクトに通知します。
    /// </Summary>
    /// <param name="targetObj">対象のオブジェクト</param>
    void NotifyButtonInactivateEvent(GameObject targetObj)
    {
        ExecuteEvents.Execute<IButtonStateNotify>(
                        target: targetObj,
                        eventData: null,
                        functor: CallButtonInactivateEvent
                        );
    }

    /// <Summary>
    /// ボタンを押せないようにするイベントを通知するメソッドです。
    /// </Summary>
    void CallButtonInactivateEvent(IButtonStateNotify inf, BaseEventData eventData)
    {
        inf.SetButtonState(false);
    }

    /// <Summary>
    /// 空きスロットの周辺のボタンを押せるようにします。
    /// </Summary>
    void CheckAroundEmptySlot()
    {
        // 有効化するボタンIDのリストを作成します。
        List<GameObject> activateList = puzzleDataManager.GetAroundEmptySlotID();

        // 候補となったボタンオブジェクトに対して、ボタンを有効化するイベントを通知します。
        foreach (GameObject obj in activateList)
        {
            // オブジェクトがnullでない場合に処理を行います。
            if (obj != null)
            {
                // 対象のボタンオブジェクトにイベントを通知します。
                NotifyButtonActivateEvent(obj);
            }
        }
    }

    /// <Summary>
    /// ボタンを押せるようにするイベントを対象のオブジェクトに通知します。
    /// </Summary>
    /// <param name="targetObj">対象のオブジェクト</param>
    void NotifyButtonActivateEvent(GameObject targetObj)
    {
        ExecuteEvents.Execute<IButtonStateNotify>(
                        target: targetObj,
                        eventData: null,
                        functor: CallButtonActivateEvent
                        );
    }

    /// <Summary>
    /// ボタンを押せるようにするイベントを通知するメソッドです。
    /// </Summary>
    void CallButtonActivateEvent(IButtonStateNotify inf, BaseEventData eventData)
    {
        inf.SetButtonState(true);
    }

    /// <Summary>
    /// ボタンが押されたことを通知されたときの処理です。
    /// </Summary>
    /// <param name="buttonId">押されたボタンのID</param>
    public void OnPressedPuzzleButton(int buttonId)
    {
        // 全てのパズルボタンを押せないようにします。
        SetAllButtonInactive();

        // ポーズボタン、ギブアップボタンを押せないようにします。
        pauseButton.interactable = false;
        giveupButton.interactable = false;

        // ボタンが押された後の処理を開始します。
        StartCoroutine(PressedButtonProcess(buttonId));
    }

    /// <Summary>
    /// ボタンが押された後の一連の処理を定義します。
    /// </Summary>
    /// <param name="buttonId">押されたボタンのID</param>
    IEnumerator PressedButtonProcess(int buttonId)
    {
        // 移動させるオブジェクトを取得します。
        GameObject targetObj = puzzleDataManager.GetMoveTargetObject(buttonId);

        // ボタンIDとオブジェクトに含まれていないIDが通知された場合は、対象がnullになるためエラーを出力します。
        if (targetObj == null)
        {
            // エラーメッセージを出力します。
            Debug.LogError($"不正なID : {buttonId} が通知されたため、オブジェクトが取得できませんでした。");

            // 処理を抜けます。
            yield break;
        }

        // 移動前の位置を対象のピースオブジェクトから取得します。
        Vector2 initPos = targetObj.transform.localPosition;

        // 移動先の位置を取得します。
        Vector2 targetPos = puzzleDataManager.GetMoveTargetPosition();

        // 押されたボタンに対応するパズルピースを空きスロットの位置に移動させます。(移動を待ち合わせます。)
        yield return StartCoroutine(MovePuzzleAnimation(targetObj, initPos, targetPos, pieceAnimTime));

        // 移動後、スロット辞書のピースの位置を入れ替えます。
        puzzleDataManager.SwapPieceSlot(buttonId);

        // (デバッグ用)コンソールにスロット辞書の中身を出力します。
        // puzzleDataManager.ShowSlotState();

        // パズルが完成したことを確認した場合、完成時の処理を呼びます。
        if (puzzleDataManager.CheckPuzzleCompleted())
        {
            // 完成時の処理を呼んでコルーチンを抜けます。
            CompletedProcess();
            yield break;
        }

        // 新しい位置でボタンの状態を確認します。
        CheckAroundEmptySlot();

        // ポーズボタン、ギブアップボタンを押せるようにします。
        pauseButton.interactable = true;
        giveupButton.interactable = true;
    }

    /// <Summary>
    /// 引数で渡されたピースオブジェクトを移動させるアニメーション処理です。
    /// </Summary>
    /// <param name="pieceObj">移動させるピースオブジェクト</param>
    /// <param name="initPos">移動前のピースの位置</param>
    /// <param name="targetPos">移動後のピースの位置</param>
    /// <param name="animTime">フェードにかかる時間</param>
    IEnumerator MovePuzzleAnimation(GameObject pieceObj, Vector2 initPos, Vector2 targetPos, float animTime)
    {
        // アニメーションの完了予定時刻を取得します。
        float finishTime = Time.time + animTime;

        while (true)
        {
            // 完了予定時刻までの残り時間を取得します。
            float remainingTime = finishTime - Time.time;

            // 完了予定時刻を過ぎている場合、または対象のゲームオブジェクトがnullの場合は処理を中断します。
            if (remainingTime <= 0)
            {
                break;
            }

            // フェードにかかる時間に対する経過時間の割合を計算します。
            float rate = 1 - Mathf.Clamp01(remainingTime / animTime);

            // 変化前と変化後の位置から割合に応じた位置に移動させます。
            pieceObj.transform.localPosition = Vector2.Lerp(initPos, targetPos, rate);

            // 次のフレームまで処理を待ちます。
            yield return null;
        }

        // 最終的な位置をセットします。
        pieceObj.transform.localPosition = targetPos;
    }

    /// <Summary>
    /// パズルが完成した時の処理です。
    /// </Summary>
    void CompletedProcess()
    {
        // タイマーを止めます。
        timeManager.PauseTimer();

        // ボタンを押せないようにします。
        pauseButton.interactable = false;
        giveupButton.interactable = false;

        // プレイタイムを記録します。
        timeManager.RecordPlayTime();

        // パズル完成時のコルーチンを開始します。
        StartCoroutine(PuzzleCompleted());
    }

    /// <Summary>
    /// パズルが完成した時の時間制御が必要な処理です。
    /// </Summary>
    IEnumerator PuzzleCompleted()
    {
        // 完成した画面を0.5秒表示します。
        yield return new WaitForSeconds(0.5f);

        // リザルト画面を表示します。
        resultPanelObj.SetActive(true);

        // 今回の記録をTimeManagerから取得します。
        string playTime = timeManager.GetPlayTime();
        string bestTime = timeManager.GetBestTime();

        // 記録を更新したかどうかのフラグをセットします。
        bool isNewRecord = timeManager.IsNewRecord();

        // テキストオブジェクトに文字列をセットします。
        resultTimerText.SetText(playTime);
        resultBestTimeText.SetText(bestTime);

        // 記録を更新した場合は記録更新のテキストを表示します。
        resultNewRecordText.SetActive(isNewRecord);

        // 記録を1秒表示したあと、ボタンを押せるようにします。
        yield return new WaitForSeconds(1.0f);
        okButton.interactable = true;
    }

    /// <Summary>
    /// ゲーム開始時には画面内のボタンが押せないようにします。
    /// </Summary>
    void SetButtonInactive()
    {
        // ポーズボタンを押せないようにします。
        pauseButton.interactable = false;

        // 再開ボタンを押せないようにします。
        resumeButton.interactable = false;

        // ギブアップボタンを押せないようにします。
        giveupButton.interactable = false;

        // タイトルへボタンを押せないようにします。
        goTitleButton.interactable = false;

        // キャンセルボタンを押せないようにします。
        cancelButton.interactable = false;
    }

    /// <Summary>
    /// タイトル画面で選択したモードの確認を行います。
    /// </Summary>
    void CheckPuzzleMode()
    {
        // 現在のモードを画面に表示します。
        modeText.SetText(PuzzleModeHolder.GetModeName());

        // 現在のモードでのボタン数をセットします。
        int puzzleButtonNum = PuzzleModeHolder.GetModeButtonNum();

        // ボタン数をPuzzleDataManagerにセットします。
        puzzleDataManager.SetPuzzleButtonNum(puzzleButtonNum);
    }

    /// <Summary>
    /// ポーズボタンが押された時の処理です。
    /// </Summary>
    public void OnPressedPauseButton()
    {
        // ボタンを連打できないようにします。
        pauseButton.interactable = false;
        giveupButton.interactable = false;

        // ポーズパネルを表示します。
        pausePanelObj.SetActive(true);

        // タイマーを停止します。
        timeManager.PauseTimer();

        // 再開ボタンを押せるようにします。
        resumeButton.interactable = true;
    }

    /// <Summary>
    /// 再開ボタンが押された時の処理です。
    /// </Summary>
    public void OnPressedResumeButton()
    {
        // ボタンを連打できないようにします。
        resumeButton.interactable = false;

        // ポーズパネルを非表示にします。
        pausePanelObj.SetActive(false);

        // タイマーを再開します。
        timeManager.StartTimer();

        // ポーズボタンを押せるようにします。
        pauseButton.interactable = true;
        giveupButton.interactable = true;
    }

    /// <Summary>
    /// ギブアップボタンが押された時の処理です。
    /// </Summary>
    public void OnPressedGiveupButton()
    {
        // ボタンを連打できないようにします。
        pauseButton.interactable = false;
        giveupButton.interactable = false;

        // ギブアップパネルを表示します。
        giveupPanelObj.SetActive(true);

        // タイマーを停止します。
        timeManager.PauseTimer();

        // タイトルへボタン、キャンセルボタンを押せるようにします。
        goTitleButton.interactable = true;
        cancelButton.interactable = true;
    }

    /// <Summary>
    /// タイトルへボタンが押された時の処理です。
    /// </Summary>
    public void OnPressedGoTitleButton()
    {
        // 画面内のボタンを連打できないようにします。
        goTitleButton.interactable = false;
        cancelButton.interactable = false;

        // タイトルシーンに戻る処理を開始します。
        StartCoroutine(SwitchSceneProcess(screenMask, SceneName.Title));
    }

    /// <Summary>
    /// キャンセルボタンが押された時の処理です。
    /// </Summary>
    public void OnPressedCancelButton()
    {
        // 画面内のボタンを連打できないようにします。
        goTitleButton.interactable = false;
        cancelButton.interactable = false;

        // ギブアップパネルを非表示にします。
        giveupPanelObj.SetActive(false);

        // タイマーを再開します。
        timeManager.StartTimer();

        // ポーズボタンとギブアップボタンを押せるようにします。
        pauseButton.interactable = true;
        giveupButton.interactable = true;
    }

    /// <Summary>
    /// リザルトパネルのOKボタンが押された時の処理です。
    /// </Summary>
    public void OnPressedOkButton()
    {
        // 画面内のボタンを連打できないようにします。
        okButton.interactable = false;

        // タイトルシーンに戻る処理を開始します。
        StartCoroutine(SwitchSceneProcess(screenMask, SceneName.Title));
    }
}
