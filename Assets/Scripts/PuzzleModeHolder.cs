using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PuzzleModeHolder
{
    // 8パズルのモード名です。
    public const string Mode8PuzzleName = "8パズル";

    // 8パズルのモードIDです。
    public const int Mode8Puzzle = 0;

    // 8パズルで使うボタンの数です。
    public const int Mode8PuzzleButtons = 9;

    // 15パズルのモード名です。
    public const string Mode15PuzzleName = "15パズル";

    // 15パズルのモードIDです。
    public const int Mode15Puzzle = 1;

    // 15パズルで使うボタンの数です。
    public const int Mode15PuzzleButtons = 16;

    // 24パズルのモード名です。
    public const string Mode24PuzzleName = "24パズル";

    // 24パズルのモードIDです。
    public const int Mode24Puzzle = 2;

    // 24パズルで使うボタンの数です。
    public const int Mode24PuzzleButtons = 25;

    static int currentMode;

    public static void SetModeId(int mode)
    {
        currentMode = mode;
    }

    public static int GetModeId()
    {
        return currentMode;
    }

    public static string GetModeName()
    {
        // モード名を格納する変数をセットします。
        string modeName = Mode8PuzzleName;

        // 現在のモードに合わせてモード名を返します。
        switch (currentMode)
        {
            case Mode15Puzzle:
                modeName = Mode15PuzzleName;
                break;
            case Mode24Puzzle:
                modeName = Mode24PuzzleName;
                break;
        }

        return modeName;
    }

    public static int GetModeButtonNum()
    {
        // ボタンの数を格納する変数をセットします。
        int buttonNum = Mode8PuzzleButtons;

        // 現在のモードに合わせてボタンの数を返します。
        switch (currentMode)
        {
            case Mode15Puzzle:
                buttonNum = Mode15PuzzleButtons;
                break;
            case Mode24Puzzle:
                buttonNum = Mode24PuzzleButtons;
                break;
        }

        return buttonNum;
    }
}
