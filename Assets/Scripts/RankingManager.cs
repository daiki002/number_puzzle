using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingManager : RankingBase
{
    [Header("8Puzzle")]
    public TextMeshProUGUI rank1stPuzzle8Text;
    public TextMeshProUGUI rank2ndPuzzle8Text;
    public TextMeshProUGUI rank3rdPuzzle8Text;
    public TextMeshProUGUI rank4thPuzzle8Text;
    public TextMeshProUGUI rank5thPuzzle8Text;
    List<TextMeshProUGUI> puzzle8TextList = new List<TextMeshProUGUI>();

    [Header("15Puzzle")]
    public TextMeshProUGUI rank1stPuzzle15Text;
    public TextMeshProUGUI rank2ndPuzzle15Text;
    public TextMeshProUGUI rank3rdPuzzle15Text;
    public TextMeshProUGUI rank4thPuzzle15Text;
    public TextMeshProUGUI rank5thPuzzle15Text;
    List<TextMeshProUGUI> puzzle15TextList = new List<TextMeshProUGUI>();

    [Header("24Puzzle")]
    public TextMeshProUGUI rank1stPuzzle24Text;
    public TextMeshProUGUI rank2ndPuzzle24Text;
    public TextMeshProUGUI rank3rdPuzzle24Text;
    public TextMeshProUGUI rank4thPuzzle24Text;
    public TextMeshProUGUI rank5thPuzzle24Text;
    List<TextMeshProUGUI> puzzle24TextList = new List<TextMeshProUGUI>();
    void Start()
    {
        SetUpLists();
    }

    void SetUpLists()
    {
        // 8パズルのランキングを表示するテキストのリストを作成します。
        puzzle8TextList = new List<TextMeshProUGUI>();
        puzzle8TextList.Add(rank1stPuzzle8Text);
        puzzle8TextList.Add(rank2ndPuzzle8Text);
        puzzle8TextList.Add(rank3rdPuzzle8Text);
        puzzle8TextList.Add(rank4thPuzzle8Text);
        puzzle8TextList.Add(rank5thPuzzle8Text);

        // 15パズルのランキングを表示するテキストのリストを作成します。
        puzzle15TextList = new List<TextMeshProUGUI>();
        puzzle15TextList.Add(rank1stPuzzle15Text);
        puzzle15TextList.Add(rank2ndPuzzle15Text);
        puzzle15TextList.Add(rank3rdPuzzle15Text);
        puzzle15TextList.Add(rank4thPuzzle15Text);
        puzzle15TextList.Add(rank5thPuzzle15Text);

        // 24パズルのランキングを表示するテキストのリストを作成します。
        puzzle24TextList = new List<TextMeshProUGUI>();
        puzzle24TextList.Add(rank1stPuzzle24Text);
        puzzle24TextList.Add(rank2ndPuzzle24Text);
        puzzle24TextList.Add(rank3rdPuzzle24Text);
        puzzle24TextList.Add(rank4thPuzzle24Text);
        puzzle24TextList.Add(rank5thPuzzle24Text);
    }

    public void SetRankingData()
    {
        // 8パズルのデータをセットします。
        SetRankingDataToText(PuzzleModeHolder.Mode8Puzzle, puzzle8TextList);

        // 15パズルのデータをセットします。
        SetRankingDataToText(PuzzleModeHolder.Mode15Puzzle, puzzle15TextList);

        // 24パズルのデータをセットします。
        SetRankingDataToText(PuzzleModeHolder.Mode24Puzzle, puzzle24TextList);
    }

    void SetRankingDataToText(int currentMode, List<TextMeshProUGUI> textObjList)
    {
        // モードごとの記録を格納するリストを作成します。
        List<float> dataList = new List<float>();

        // ランキングデータの保存タグのリストを作成します。
        List<string> tagList = GetModeTagList(currentMode);

        // 既存の記録を確認してリストに追加します。存在しない場合は、デフォルト値として-1を返します。
        foreach (string rankingTag in tagList)
        {
            float rankingData = PlayerPrefs.GetFloat(rankingTag, -1);
            dataList.Add(rankingData);
        }

        // タグリスト、データのリスト、テキストリストは順番を揃えているため、同じインデックスでアクセスします。
        for (int i = 0; i < dataList.Count; i++)
        {
            // 文字列に変換します。
            string dataString = GetTimerString(dataList[i]);

            // テキストにセットします。
            textObjList[i].SetText(dataString);
        }
    }
}
