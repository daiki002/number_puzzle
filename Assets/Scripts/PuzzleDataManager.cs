using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDataManager : MonoBehaviour
{

    // スロットIDと、その位置にいるピースのIDを対応付ける辞書を作成します。
    Dictionary<int, int> slotDict = new Dictionary<int, int>();

    // ピースIDとピースのゲームオブジェクトを対応付ける辞書を作成します。
    Dictionary<int, GameObject> pieceIdObjDict = new Dictionary<int, GameObject>();

    // ボタンIDとボタンのゲームオブジェクトを対応付ける辞書を作成します。
    Dictionary<int, GameObject> buttonIdObjDict = new Dictionary<int, GameObject>();

    // PuzzleGeneratorへの参照を保持するフィールドです。
    public PuzzleGenerator puzzleGenerator;

    // パズルボタンの数を保持するフィールドです。
    int puzzleButtonNum = 9;

    // 空きスロットを意味する数字を定義します。
    const int EmptySlot = 0;

    void Start()
    {

    }

    void Update()
    {

    }

    /// <Summary>
    /// パズルボタンの数をフィールドにセットします。
    /// </Summary>
    public void SetPuzzleButtonNum(int buttonNum)
    {
        puzzleButtonNum = buttonNum;
    }

    /// <Summary>
    /// ボタンIDとオブジェクトの対応を管理する辞書を取得します。
    /// </Summary>
    public Dictionary<int, GameObject> GetButtonIdObjDict()
    {
        return buttonIdObjDict;
    }

    /// <Summary>
    /// パズルボタンのセットアップを行います。
    /// </Summary>
    public void SetUpPuzzleButtons()
    {
        // 必要な数のボタンをインスタンス化します。
        for (int i = 1; i <= puzzleButtonNum; i++)
        {
            // ボタンをインスタンス化します。
            GameObject buttonObj = puzzleGenerator.GeneratePuzzleButtons(puzzleButtonNum, i);

            // ボタンのIDをスロットIDとして追加します。
            slotDict.Add(i, EmptySlot);

            // ボタンとオブジェクトの辞書に値をセットします。
            buttonIdObjDict.Add(i, buttonObj);
        }
    }

    /// <Summary>
    /// パズルピースのセットアップを行います。
    /// </Summary>
    public void SetUpPuzzlePieces()
    {
        // インスタンス化するピースの数を計算します。
        int pieceNum = puzzleButtonNum - 1;

        // 必要な数のピースをインスタンス化します。
        for (int i = 1; i <= pieceNum; i++)
        {
            // ピースをインスタンス化します。
            GameObject pieceObj = puzzleGenerator.GeneratePuzzlePieces(puzzleButtonNum, i);

            // スロットIDが辞書に含まれていることを確認します。
            if (slotDict.ContainsKey(i))
            {
                // 対応するスロットIDにピースIDをセットします。
                slotDict[i] = i;
            }

            // ピースIDとゲームオブジェクトの対応付けを辞書にセットします。
            pieceIdObjDict.Add(i, pieceObj);
        }
    }

    /// <Summary>
    /// スロットの中身を並べ替えます。
    /// </Summary>
    public IEnumerator ShuffleSlot()
    {
        // 無限ループを防ぐため、1フレームあたりの抽選回数をカウントします。
        int counter = 0;

        // 1フレームあたりの抽選回数を定義します。
        int shuffleInFrame = 10;

        while (true)
        {
            // Randomを使用して複数回並べ替えます。
            int shuffleNum = 5;
            for (int i = 0; i < shuffleNum; i++)
            {
                // スロットの各IDにアクセスしてランダムに入れ替えを行います。
                for (int id = 1; id <= puzzleButtonNum; id++)
                {
                    // ランダムなIDを取得します。
                    int randomIndex = Random.Range(1, puzzleButtonNum);

                    // スロット辞書の内容を指定されたIDのものと入れ替えます。
                    SwapSlotId(id, randomIndex);
                }
            }

            // 入れ替えた後のスロットが完成状態でなく、かつ解ける配置であればwhileループを抜けます。
            if (!CheckPuzzleCompleted() && CheckIfPuzzleSolvable())
            {
                break;
            }

            // カウンターを増やします。
            counter++;

            // フレームあたりの抽選回数を超えた場合はそのフレームで処理を行わないようにします。
            if (counter >= shuffleInFrame)
            {
                counter = 0;
                yield return null;
            }
        }
    }

    /// <Summary>
    /// 並べ替え後のスロットの位置にピースを移動させます。
    /// </Summary>
    public void MovePiecePos()
    {
        // スロットIDからピースIDを取得し、対応するオブジェクトの位置を変更します。
        foreach (KeyValuePair<int, int> pair in slotDict)
        {
            // ピースIDが0の場合は空きスロットなので処理を行わず次のスロットに進みます。
            if (pair.Value == EmptySlot)
            {
                continue;
            }

            // ボタン辞書からゲームオブジェクトを取得し、その位置を取得します。
            int buttonId = pair.Key;
            GameObject buttonObj = buttonIdObjDict[buttonId];
            Vector2 pos = buttonObj.transform.localPosition;

            // 取得した位置をピースオブジェクトにセットします。
            int pieceId = pair.Value;
            GameObject pieceObj = pieceIdObjDict[pieceId];
            pieceObj.transform.localPosition = pos;
        }
    }

    /// <Summary>
    /// スロットの中身が順番に並び替えられたことを確認します。そろった場合はTrueを返します。
    /// </Summary>
    public bool CheckPuzzleCompleted()
    {
        // スロットの内容を全て確認していきます。
        for (int i = 1; i < puzzleButtonNum; i++)
        {
            // スロットのIDとピースのIDが一致していない場合はfalseを返してメソッドを抜けます。
            if (i != slotDict[i])
            {
                return false;
            }
        }

        // 全て一致していた場合にTrueを返します。
        return true;
    }

    /// <Summary>
    /// スロット辞書の内容を入れ替えます。
    /// </Summary>
    /// <param name="initId">交換するスロットのID</param>
    /// <param name="targetId">交換されるスロットのID</param>
    void SwapSlotId(int initId, int targetId)
    {
        // 入れ替え先のスロットにあるピースのIDを一時的に保存します。
        int temp = slotDict[initId];

        // 入れ替え先のスロットに確認中のスロットにあるピースIDをセットします。
        slotDict[initId] = slotDict[targetId];

        // 保存した入れ替え先のピースIDを確認中のスロットにセットします。
        slotDict[targetId] = temp;
    }

    /// <Summary>
    /// 空きスロットの周辺のボタンを押せるようにします。
    /// </Summary>
    public List<GameObject> GetAroundEmptySlotID()
    {
        // 空きスロットの周りにあるボタンについて、上、左、右、下の順で確認します。

        // 空きスロットのボタンIDを取得します。
        int emptyId = FindEmptySlotId();

        // 1行に含まれるボタンの数を取得します。
        int buttonsInRow = (int)Mathf.Sqrt(puzzleButtonNum);

        // 有効化するボタンIDのリストを作成します。
        List<int> activateList = new List<int>();

        // 上スロットのIDが1以上の場合に処理を行います。
        int targetId = emptyId - buttonsInRow;
        if (targetId >= 1)
        {
            activateList.Add(targetId);
        }

        // 左スロットの確認を行います。emptyIdをbuttonsInRowで割った時、余りが1なら左のボタンが存在しません。
        if (emptyId % buttonsInRow != 1)
        {
            targetId = emptyId - 1;
            activateList.Add(targetId);
        }

        // 右スロットの確認を行います。emptyIdをbuttonsInRowで割った時、余りが0なら右のボタンが存在しません。
        if (emptyId % buttonsInRow != 0)
        {
            targetId = emptyId + 1;
            activateList.Add(targetId);
        }

        // 下スロットのIDがpuzzleButtonNum以下の場合に処理を行います。
        targetId = emptyId + buttonsInRow;
        if (targetId <= puzzleButtonNum)
        {
            activateList.Add(targetId);
        }

        // ボタンIDからオブジェクトのリストを作成します。
        List<GameObject> targetButtonList = new List<GameObject>();

        foreach (int id in activateList)
        {
            // ボタンのIDとオブジェクトの辞書にデータが含まれているか確認します。
            if (buttonIdObjDict.ContainsKey(id))
            {
                // 辞書からオブジェクトを取得します。
                GameObject targetObj = buttonIdObjDict[id];

                // オブジェクトをリストに追加します。
                targetButtonList.Add(targetObj);
            }
        }

        return targetButtonList;
    }

    /// <Summary>
    /// 空きスロットとなっているボタンIDを取得します。
    /// </Summary>
    int FindEmptySlotId()
    {
        foreach (KeyValuePair<int, int> pair in slotDict)
        {
            // スロット辞書に含まれるデータの中で、ピースIDが空きスロットのものになっているスロットIDを見つけます。
            if (pair.Value == EmptySlot)
            {
                return pair.Key;
            }
        }

        // 空きスロットが見つからなかった場合はこれまでの処理に問題があるのでエラーを出力します。
        Debug.LogError("空きスロットが見つかりませんでした。");
        return 0;
    }

    /// <Summary>
    /// 押されたボタンのIDに対応するGameObjectを取得します。
    /// </Summary>
    public GameObject GetMoveTargetObject(int buttonId)
    {
        // 押されたボタンの位置にいるピースのオブジェクトを取得します。
        int targetPieceId = slotDict[buttonId];

        // 移動させるオブジェクトを取得します。
        GameObject targetObj = null;
        if (pieceIdObjDict.ContainsKey(targetPieceId))
        {
            // 辞書からオブジェクトを取得します。
            targetObj = pieceIdObjDict[targetPieceId];
        }

        return targetObj;
    }

    /// <Summary>
    /// ピースの移動先の位置を取得します。
    /// </Summary>
    public Vector2 GetMoveTargetPosition()
    {
        // 移動先の位置を空きスロットのIDから取得します。
        int emptyId = FindEmptySlotId();

        // 移動先の位置を格納する変数を定義します。
        Vector2 targetPos = Vector2.zero;

        if (buttonIdObjDict.ContainsKey(emptyId))
        {
            // 辞書から位置を取得します。
            targetPos = buttonIdObjDict[emptyId].transform.localPosition;
        }

        return targetPos;
    }

    /// <Summary>
    /// スロット辞書内のピースの位置を入れ替えます。
    /// </Summary>
    public void SwapPieceSlot(int buttonId)
    {
        // 移動先の位置を空きスロットのIDから取得します。
        int emptyId = FindEmptySlotId();

        // 移動後、スロット辞書のピースの位置を入れ替えます。
        SwapSlotId(emptyId, buttonId);
    }

    /// <Summary>
    /// パズルが解ける配置かどうか確認します。
    /// 空きスロットの移動回数の偶奇、交換回数の偶奇が異なると、パズルは解けません。
    /// </Summary>
    bool CheckIfPuzzleSolvable()
    {
        // 空きスロットの最短距離の移動回数を計算します。
        int emptyMoveNum = GetShortestMoveNum();

        // 偶数か奇数かの計算を行います。2で割った余りが0なら偶数、1なら奇数です。
        int emptyParity = emptyMoveNum % 2;

        // 交換回数を計算します。
        int exchangeNum = GetExchangeNum();

        // 交換回数が負の値であればfalseを返します。
        if (exchangeNum < 0)
        {
            Debug.LogError("スロット辞書が正しく生成されませんでした。");
            return false;
        }

        // 交換回数について偶数か奇数かの計算を行います。
        int exchangeParity = exchangeNum % 2;

        // どちらも偶数、またはどちらも奇数である場合にパズルが解けるのでTrueを返します。
        if (emptyParity == exchangeParity)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <Summary>
    /// 空きスロットが右下に移動するまでの最短距離を計算します。
    /// </Summary>
    int GetShortestMoveNum()
    {
        // 最短距離となる移動回数を保持する変数を定義します。
        int shortest = 0;

        // 1行に配置されるピースの数を計算します。
        int piecesInRow = (int)Mathf.Sqrt(puzzleButtonNum);

        // 空きスロットのIDを取得します。
        int emptyId = FindEmptySlotId();

        // 横方向の移動回数を計算します。
        int currentColumn = (emptyId - 1) % piecesInRow;
        int targetColumn = (puzzleButtonNum - 1) % piecesInRow;
        int columnMove = targetColumn - currentColumn;

        // 縦方向の移動回数を計算します。
        int currentRow = (emptyId - 1) / piecesInRow;
        int targetRow = (puzzleButtonNum - 1) / piecesInRow;
        int rowMove = targetRow - currentRow;

        // 移動回数を合計します。
        shortest = columnMove + rowMove;

        return shortest;
    }

    /// <Summary>
    /// 並べ替えられたスロット辞書から、完成状態になるまでの交換回数を計算します。
    /// ゲーム的な並べ替えではなく、配列内の並べ替え回数が対象です。
    /// </Summary>
    int GetExchangeNum()
    {
        // スロット辞書そのものの内容を変えないように、辞書の内容をコピーします。
        Dictionary<int, int> copyDict = new Dictionary<int, int>();
        foreach (KeyValuePair<int, int> pair in slotDict)
        {
            copyDict.Add(pair.Key, pair.Value);
        }

        // 交換回数を保持する変数を定義します。
        int exchangeNum = 0;

        // スロット1から値を確認し、正しい位置になるように交換を行います。
        for (int i = 1; i < puzzleButtonNum; i++)
        {
            // スロットのIDとピースIDが等しい場合は交換を行わず次に進みます。
            if (i == copyDict[i])
            {
                continue;
            }

            // スロットのIDと等しいピースIDを辞書内から探し、そのインデックスを取得します。
            int targetKey = 0;
            foreach (KeyValuePair<int, int> pair in copyDict)
            {
                if (i == pair.Value)
                {
                    targetKey = pair.Key;
                }
            }

            // 辞書の値を交換し、交換回数を増やします。
            if (targetKey != 0)
            {
                // 交換先の値を保持します。
                int temp = copyDict[targetKey];

                // 確認対象のキーの値を交換先にセットします。
                copyDict[targetKey] = copyDict[i];

                // 保持している値を確認対象のキーにセットします。
                copyDict[i] = temp;

                // 交換回数を増やします。
                exchangeNum++;
            }
            else
            {
                // 交換対象のピースがない場合はスロット辞書の作成でエラーがある可能性があるため、処理を止めて-1を返します。
                return -1;
            }
        }

        // 交換回数を返します。
        return exchangeNum;
    }

    // /// <Summary>
    // /// 現在のスロットの中身をコンソールに表示します。(デバッグ用メソッド)
    // /// </Summary>
    // public void ShowSlotState()
    // {
    //     // 出力用の文字列を生成するStringBuilderオブジェクトを定義します。
    //     System.Text.StringBuilder sb = new System.Text.StringBuilder();

    //     // スロット辞書の中身をStringBuilderオブジェクトに加えていきます。
    //     foreach (KeyValuePair<int, int> pair in slotDict)
    //     {
    //         sb.Append("スロットID : ")
    //             .Append(pair.Key)
    //             .Append(" => ピースID : ")
    //             .Append(pair.Value)
    //             .Append(" / ");
    //     }

    //     // 文字列をコンソールに出力します。
    //     Debug.Log(sb.ToString());
    // }
}
