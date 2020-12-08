using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <Summary>
/// タイトルシーンの管理を行うスクリプトです。
/// </Summary>
public class TitleManager : ManagerBase
{
    [Header("Main")]
    // スタートボタンのButtonコンポーネントへの参照を保持します。
    public Button startButton;

    // ランキングボタンのButtonコンポーネントへの参照を保持します。
    public Button rankingButton;

    // ヘルプボタンのButtonコンポーネントへの参照を保持します。
    public Button helpButton;

    [Header("Mode Select")]
    // モード選択パネルへの参照を保持します。
    public GameObject modeSelectPanelObj;

    // モード選択パネルにある8パズルボタンのButtonコンポーネントへの参照を保持します。
    public Button mode8Button;

    // モード選択パネルにある15パズルボタンのButtonコンポーネントへの参照を保持します。
    public Button mode15Button;

    // モード選択パネルにある24パズルボタンのButtonコンポーネントへの参照を保持します。
    public Button mode24Button;

    // モード選択パネルにある戻るボタンのButtonコンポーネントへの参照を保持します。
    public Button backButton;

    [Header("Ranking")]
    // ランキングパネルへの参照を保持します。
    public GameObject rankingPanelObj;

    // ランキングパネルにある戻るボタンのButtonコンポーネントへの参照を保持します。
    public Button rankingBackButton;

    // RankingManagerへの参照を保持します。
    public RankingManager rankingManager;

    [Header("Help")]
    // ヘルプパネルへの参照を保持します。
    public GameObject helpPanelObj;

    // ヘルプパネルにあるストーリーの情報への参照を保持します。
    public GameObject helpStoryObj;

    // ヘルプパネルにあるルールの情報への参照を保持します。
    public GameObject helpRuleObj;

    // ヘルプパネルにある操作の情報への参照を保持します。
    public GameObject helpOperationObj;

    // ヘルプパネルにあるストーリーボタンのButtonコンポーネントへの参照を保持します。
    public Button storyButton;

    // ヘルプパネルにあるルールボタンのButtonコンポーネントへの参照を保持します。
    public Button ruleButton;

    // ヘルプパネルにある操作ボタンのButtonコンポーネントへの参照を保持します。
    public Button operationButton;

    // ヘルプパネルにある戻るボタンのButtonコンポーネントへの参照を保持します。
    public Button helpBackButton;

    // 画面を隠すマスク画像への参照をセットします。
    public GameObject screenMask;

    public GameObject TitelText;

    void Start()
    {
        StartCoroutine(StartProcess());
    }

    void Update()
    {

    }

    /// <Summary>
    /// メイン画面のボタンの状態を一括で変更する処理です。
    /// </Summary>
    void SetMainButtonStates(bool isActive)
    {
        startButton.interactable = isActive;
        rankingButton.interactable = isActive;
        helpButton.interactable = isActive;
    }

    /// <Summary>
    /// モード選択画面のボタンの状態を一括で変更する処理です。
    /// </Summary>
    void SetModeSelectButtonStates(bool isActive)
    {
        mode8Button.interactable = isActive;
        mode15Button.interactable = isActive;
        mode24Button.interactable = isActive;
        backButton.interactable = isActive;
    }

    /// <Summary>
    /// ヘルプ画面のボタンの状態を一括で変更する処理です。
    /// </Summary>
    ///

    void SetHelpButtonStates(bool isActive)
    {
        storyButton.interactable = isActive;
        ruleButton.interactable = isActive;
        operationButton.interactable = isActive;
        helpBackButton.interactable = isActive;
    }
    /// <Summary>
    /// ヘルプ画面の各コンテンツの状態を一括で変更する処理です。
    /// </Summary>
    void SetHelpContentStates(bool isActive)
    {
        helpStoryObj.SetActive(isActive);
        helpRuleObj.SetActive(isActive);
        helpOperationObj.SetActive(isActive);
    }

    /// <Summary>
    /// タイトルシーンが開始する時の一連の処理です。
    /// </Summary>
    IEnumerator StartProcess()
    {
        // 画像のアルファ(透明度)の値を定義します。
        float maskAlpha = 0f;

        // フェードのアニメーションにかかる時間を定義します。
        float animTime = 0.5f;

        // 画面をフェードインさせます。
        yield return StartCoroutine(FadeAnimation(screenMask, maskAlpha, animTime));

        // メイン画面のボタンを押せるようにします。
        SetMainButtonStates(true);
    }

    /// <Summary>
    /// スタートボタンが押された時の処理です。
    /// </Summary>
    public void OnPressedStartButton()
    {
        // メイン画面のボタンを押せないようにします。
        SetMainButtonStates(false);

        // モード選択パネルを表示します。
        modeSelectPanelObj.SetActive(true);

        // モード選択画面のボタンを押せるようにします。
        SetModeSelectButtonStates(true);

        TitelText.SetActive(false);
    }

    /// <Summary>
    /// モード選択画面の8パズルボタンが押された時の処理です。
    /// </Summary>
    public void OnPressedMode8Button()
    {
        // モード選択画面のボタンを押せないようにします。
        SetModeSelectButtonStates(false);

        // モードをセットします。
        PuzzleModeHolder.SetModeId(PuzzleModeHolder.Mode8Puzzle);

        // パズルシーンに移動する処理を開始します。
        StartCoroutine(SwitchSceneProcess(screenMask, SceneName.Game));
    }

    /// <Summary>
    /// モード選択画面の15パズルボタンが押された時の処理です。
    /// </Summary>
    public void OnPressedMode15Button()
    {
        // モード選択画面のボタンを押せないようにします。
        SetModeSelectButtonStates(false);

        // モードをセットします。
        PuzzleModeHolder.SetModeId(PuzzleModeHolder.Mode15Puzzle);

        // パズルシーンに移動する処理を開始します。
        StartCoroutine(SwitchSceneProcess(screenMask, SceneName.Game));
    }

    /// <Summary>
    /// モード選択画面の24パズルボタンが押された時の処理です。
    /// </Summary>
    public void OnPressedMode24Button()
    {
        // モード選択画面のボタンを押せないようにします。
        SetModeSelectButtonStates(false);

        // モードをセットします。
        PuzzleModeHolder.SetModeId(PuzzleModeHolder.Mode24Puzzle);

        // パズルシーンに移動する処理を開始します。
        StartCoroutine(SwitchSceneProcess(screenMask, SceneName.Game));
    }

    /// <Summary>
    /// モード選択画面の戻るボタンが押された時の処理です。
    /// </Summary>
    public void OnPressedBackButton()
    {
        // モード選択画面のボタンを押せないようにします。
        SetModeSelectButtonStates(false);

        // モード選択パネルを非表示にします。
        modeSelectPanelObj.SetActive(false);

        // メイン画面のボタンを押せるようにします。
        SetMainButtonStates(true);
    }

    /// <Summary>
    /// ランキングボタンが押された時の処理です。
    /// </Summary>
    public void OnPressedRankingButton()
    {
        // メイン画面のボタンを押せないようにします。
        SetMainButtonStates(false);

        // ランキングパネルを表示します。
        rankingPanelObj.SetActive(true);

        // RankingManagerで情報をセットします。
        rankingManager.SetRankingData();

        // ランキング画面のボタンを押せるようにします。
        rankingBackButton.interactable = true;
    }

    public void OnPressedHelpButton()
    {
        // メイン画面のボタンを押せないようにします。
        SetMainButtonStates(false);

        // ヘルプパネルを表示します。
        helpPanelObj.SetActive(true);

        // ヘルプ画面のボタンを押せるようにします。
        SetHelpButtonStates(true);

        // ストーリーパネルを表示します。
        OnPressedStoryButton();
    }

    public void OnPressedStoryButton()
    {
        // 一度ヘルプ画面のボタンを押せるようにします。
        SetHelpButtonStates(true);

        // ストーリーボタンを押せないようにします。
        storyButton.interactable = false;

        // ヘルプ画面の表示内容を非表示にします。
        SetHelpContentStates(false);

        // ストーリーパネルを表示します。
        helpStoryObj.SetActive(true);
    }

    public void OnPressedRuleButton()
    {
        // 一度ヘルプ画面のボタンを押せるようにします。
        SetHelpButtonStates(true);

        // ルールボタンを押せないようにします。
        ruleButton.interactable = false;

        // ヘルプ画面の表示内容を非表示にします。
        SetHelpContentStates(false);

        // ルールパネルを表示します。
        helpRuleObj.SetActive(true);
    }

    public void OnPressedOperationButton()
    {
        // 一度ヘルプ画面のボタンを押せるようにします。
        SetHelpButtonStates(true);

        // 操作ボタンを押せないようにします。
        operationButton.interactable = false;

        // ヘルプ画面の表示内容を非表示にします。
        SetHelpContentStates(false);

        // 操作パネルを表示します。
        helpOperationObj.SetActive(true);
    }

    /// <Summary>
    /// ヘルプ画面の戻るボタンが押された時の処理です。
    /// </Summary>
    public void OnPressedHelpBackButton()
    {
        // ヘルプ画面のボタンを押せないようにします。
        SetHelpButtonStates(false);

        // モード選択パネルを非表示にします。
        helpPanelObj.SetActive(false);

        // メイン画面のボタンを押せるようにします。
        SetMainButtonStates(true);
    }

    /// <Summary>
    /// ランキング画面の戻るボタンが押された時の処理です。
    /// </Summary>
    public void OnPressedRankingBackButton()
    {
        // ランキング画面のボタンを押せないようにします。
        rankingBackButton.interactable = false;

        // ランキングパネルを非表示にします。
        rankingPanelObj.SetActive(false);

        // メイン画面のボタンを押せるようにします。
        SetMainButtonStates(true);
    }
}
