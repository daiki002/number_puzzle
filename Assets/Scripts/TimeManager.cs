using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeManager : RankingBase
{

    // タイマーを表示するテキストへの参照を保持します。
    public TextMeshProUGUI timerText;

    // ベストタイムを表示するテキストへの参照を保持します。
    public TextMeshProUGUI bestTimeText;

    // ゲーム開始からの経過時間です。
    float playTime;

    // ランキング1位の時間です。
    float bestTimeData;

    // タイマーがカウント中かどうかのフラグです。
    bool isCounting;

    void Start()
    {
        // タイマーの表示を初期化します。
        InitializeTimer();

        // ベストタイムを読み込んでテキストにセットします。
        SetUpBestTime();
    }

    void Update()
    {
        // カウント中フラグがTrueの場合にタイマー増加の処理を行います。
        if (isCounting)
        {
            UpdateTimer();
        }
    }

    /// <Summary>
    /// タイマーの値と表示を初期化します。
    /// </Summary>
    void InitializeTimer()
    {
        // タイマーの値を初期化します。
        playTime = 0f;

        // タイマーに表示する文字列を生成します。
        string timeString = GetTimerString(playTime);

        // タイマーの表示を初期化します。
        timerText.SetText(timeString);
    }

    /// <Summary>
    /// タイマーを計算し、テキストに表示します。
    /// </Summary>
    void UpdateTimer()
    {
        // 前フレームからの経過時間をタイマーに加算します。
        playTime += Time.deltaTime;

        // TimeSpanのインスタンスからタイマー文字列を生成します。
        string timeString = GetTimerString(playTime);

        // TextMesh Proのオブジェクトに文字列をセットします。
        timerText.SetText(timeString);
    }

    /// <Summary>
    /// タイマーのカウントを開始します。
    /// </Summary>
    public void StartTimer()
    {
        isCounting = true;
    }

    /// <Summary>
    /// タイマーのカウントを停止します。
    /// </Summary>
    public void PauseTimer()
    {
        isCounting = false;
    }

    /// <Summary>
    /// タイマーの値をPlayerPrefsに保存します。
    /// </Summary>
    public void RecordPlayTime()
    {
        // 既存の記録を保存するリストを作成します。
        List<float> recordList = new List<float>();

        // 現在のモードを確認します。
        int currentMode = PuzzleModeHolder.GetModeId();

        // ランキングデータの保存タグのリストを作成します。
        List<string> tagList = GetModeTagList(currentMode);

        // 既存の記録を確認してリストに追加します。存在しない場合は、デフォルト値として-1を返します。
        foreach (string rankingTag in tagList)
        {
            float rankingData = PlayerPrefs.GetFloat(rankingTag, -1);
            recordList.Add(rankingData);
        }

        // // デバッグ用: 既存の記録をコンソールに表示します。
        // ShowRecords(tagList);

        // 新しい記録を保存するインデックスを定義します。
        int insertIndex = -1;

        // 既存の記録のうち上位の記録から比較を行っていきます。
        for (int i = 0; i < recordList.Count; i++)
        {
            // 確認中の既存のデータが負の値であればその位置に保存します。
            if (recordList[i] < 0f)
            {
                insertIndex = i;
                break;
            }

            // 新しい記録と既存の記録の比較を行い、新しい記録がより短い時間であればその位置のインデックスを保存します。
            if (playTime < recordList[i])
            {
                insertIndex = i;
                break;
            }
        }

        // 既存の記録を上回った場合はリストにデータを挿入します。
        if (insertIndex != -1)
        {
            recordList.Insert(insertIndex, playTime);
            // Debug.Log($"今回の記録は {insertIndex + 1}位 でした。");
        }
        else
        {
            // Debug.Log($"今回の記録は ランク外 でした。");
        }

        // リストの上位5つのデータを保存します。
        for (int i = 0; i < tagList.Count; i++)
        {
            PlayerPrefs.SetFloat(tagList[i], recordList[i]);
        }
    }

    /// <Summary>
    /// PlayerPrefsからベストタイムを読み込みテキストにセットします。
    /// </Summary>
    void SetUpBestTime()
    {
        // 現在のモードのランキング1位のタグを取得します。
        string rankingTag = GetModeBestTimeTag();

        // ランキングから1位のデータを取得します。
        bestTimeData = PlayerPrefs.GetFloat(rankingTag, -1);

        // ベストタイムに設定する文字列を保持する変数を定義します。
        string timeString = GetTimerString(bestTimeData);

        // ベストタイム用のテキストに文字列をセットします。
        bestTimeText.SetText(timeString);
    }

    /// <Summary>
    /// 今回のゲームの記録を文字列で取得します。
    /// </Summary>
    public string GetPlayTime()
    {
        // タイマー文字列を生成して戻り値として返します。
        return GetTimerString(playTime);
    }

    /// <Summary>
    /// ランキング1位の記録を文字列で取得します。
    /// </Summary>
    public string GetBestTime()
    {
        // タイマー文字列を生成して戻り値として返します。
        return GetTimerString(bestTimeData);
    }

    /// <Summary>
    /// ランキング1位の記録を更新したかどうかを確認します。(Trueで更新)
    /// </Summary>
    public bool IsNewRecord()
    {
        // ベストタイムがない場合や、今回の記録がランキング1位の記録より早かったらTrueを返します。
        if (bestTimeData < 0)
        {
            return true;
        }
        else if (playTime < bestTimeData)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <Summary>
    /// 現在のモードから読み込むランキングのタグを取得します。
    /// </Summary>
    string GetModeBestTimeTag()
    {
        // タグ名を保持する変数を定義します。
        string rankingTag = SaveTag.PlayRecord8Puzzle1st;

        // 現在のモードを確認します。
        int currentMode = PuzzleModeHolder.GetModeId();

        // switch文で現在のモードによってタグを切り替えます。
        switch (currentMode)
        {
            case PuzzleModeHolder.Mode15Puzzle:
                rankingTag = SaveTag.PlayRecord15Puzzle1st;
                break;
            case PuzzleModeHolder.Mode24Puzzle:
                rankingTag = SaveTag.PlayRecord24Puzzle1st;
                break;
        }

        return rankingTag;
    }

    // /// <Summary>
    // /// デバッグ用: 既存の記録をコンソールに表示します。
    // /// </Summary>
    // void ShowRecords(List<string> tagList)
    // {
    //     foreach (string rankingTag in tagList)
    //     {
    //         float rankingData = PlayerPrefs.GetFloat(rankingTag, -1);

    //         string timeString = GetTimerString(rankingData);

    //         Debug.Log($"{rankingTag} のデータは {timeString}");
    //     }
    // }
}
