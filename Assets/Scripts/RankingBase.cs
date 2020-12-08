using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RankingBase : MonoBehaviour
{
    protected string GetTimerString(float time)
    {
        // タイマーに設定する文字列を保持する変数を定義します。
        string timeString = "00:00:00.00";

        // データが存在している場合は、そのデータを文字列に変換します。
        if (time >= 0f)
        {
            // 文字列を生成するため、TimeSpanのインスタンスを作成します。
            TimeSpan ts = TimeSpan.FromSeconds(time);

            // TimeSpanのインスタンスからタイマー文字列を生成します。
            timeString = ts.ToString(@"hh\:mm\:ss\.ff");
        }

        return timeString;
    }

    protected List<string> GetModeTagList(int currentMode)
    {
        // ランキングデータの保存タグのリストを作成します。
        List<string> tagList = new List<string>();

        switch (currentMode)
        {
            case PuzzleModeHolder.Mode15Puzzle:
                tagList.Add(SaveTag.PlayRecord15Puzzle1st);
                tagList.Add(SaveTag.PlayRecord15Puzzle2nd);
                tagList.Add(SaveTag.PlayRecord15Puzzle3rd);
                tagList.Add(SaveTag.PlayRecord15Puzzle4th);
                tagList.Add(SaveTag.PlayRecord15Puzzle5th);
                break;
            case PuzzleModeHolder.Mode24Puzzle:
                tagList.Add(SaveTag.PlayRecord24Puzzle1st);
                tagList.Add(SaveTag.PlayRecord24Puzzle2nd);
                tagList.Add(SaveTag.PlayRecord24Puzzle3rd);
                tagList.Add(SaveTag.PlayRecord24Puzzle4th);
                tagList.Add(SaveTag.PlayRecord24Puzzle5th);
                break;
            default:
                tagList.Add(SaveTag.PlayRecord8Puzzle1st);
                tagList.Add(SaveTag.PlayRecord8Puzzle2nd);
                tagList.Add(SaveTag.PlayRecord8Puzzle3rd);
                tagList.Add(SaveTag.PlayRecord8Puzzle4th);
                tagList.Add(SaveTag.PlayRecord8Puzzle5th);
                break;
        }

        return tagList;
    }
}
