using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGenerator : MonoBehaviour
{
    [Header("8Puzzle")]
    // パズルのボタンオブジェクトのPrefabをセットします。
    public GameObject puzzleButton8Prefab;

    // パズルピースオブジェクトのPrefabをセットします。
    public GameObject puzzlePiece8Prefab;

    [Header("15Puzzle")]
    // パズルのボタンオブジェクトのPrefabをセットします。
    public GameObject puzzleButton15Prefab;

    // パズルピースオブジェクトのPrefabをセットします。
    public GameObject puzzlePiece15Prefab;

    [Header("24Puzzle")]
    // パズルのボタンオブジェクトのPrefabをセットします。
    public GameObject puzzleButton24Prefab;

    // パズルピースオブジェクトのPrefabをセットします。
    public GameObject puzzlePiece24Prefab;

    [Header("Parent Object")]
    // パズルピースの親オブジェクトへの参照をセットします。
    public GameObject puzzlePieceParent;

    // パズルのボタンオブジェクトの親オブジェクトへの参照をセットします。
    public GameObject puzzleButtonParent;

    // パズルピースのスプライト画像のリストです。
    public List<Sprite> pieceSprites = new List<Sprite>();

    void Start()
    {

    }

    void Update()
    {

    }

    /// <Summary>
    /// パズルボタンを生成します。
    /// </Summary>
    /// <param name="buttonNum">ボタンの数(平方数で指定)</param>
    /// <param name="index">生成するボタンのインデックス</param>
    public GameObject GeneratePuzzleButtons(int buttonNum, int index)
    {
        // 1行に配置されるボタンの数を計算します。
        int buttonsInRow = (int)Mathf.Sqrt(buttonNum);

        // インスタンス化するPrefabを取得します。
        GameObject buttonPrefab = GetPuzzleButtonPrefab();

        // インスタンス化するボタンのサイズを取得します。
        RectTransform buttonRect = buttonPrefab.GetComponent<RectTransform>();
        float buttonWidth = buttonRect.rect.width;
        float buttonHeight = buttonRect.rect.height;

        // ボタンをインスタンス化します。
        GameObject buttonObj = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);

        // 生成したオブジェクトの名前を変更します。
        buttonObj.name = $"{buttonPrefab.name}{index}";

        // 生成したオブジェクトをボタンオブジェクトの親オブジェクトの下に配置します。
        buttonObj.transform.SetParent(puzzleButtonParent.transform);

        // ボタンを配置する位置を計算します。
        Vector2 buttonPos = GetPuzzlePartsPos(index, buttonsInRow, buttonWidth, buttonHeight);

        // 計算した位置を格納します。
        buttonObj.transform.localPosition = buttonPos;

        // 親オブジェクトの縮尺をセットします。
        buttonObj.transform.localScale = puzzleButtonParent.transform.localScale;

        // ボタンのIDとマネージャーオブジェクトへの参照をセットします。
        PuzzleButtonController controller = buttonObj.GetComponent<PuzzleButtonController>();
        controller.SetButtonId(index);
        controller.SetManagerReference(gameObject);

        return buttonObj;
    }

    /// <Summary>
    /// インデックスからボタンオブジェクトを配置する位置を計算して返します。
    /// </Summary>
    /// <param name="index">インスタンス化しようとしているボタンのインデックス</param>
    /// <param name="buttonsInRow">1行に配置されるボタンの数</param>
    /// <param name="width">ボタンの幅</param>
    /// <param name="height">ボタンの高さ</param>
    Vector2 GetPuzzlePartsPos(int index, int buttonsInRow, float width, float height)
    {
        Vector2 pos = Vector2.zero;

        // 基準点のX成分を計算します。
        float basePosX = -1.0f * (buttonsInRow - 1) / 2 * width;

        // その行でのインデックスを取得するため、剰余(割った余り)を計算します。(インデックスは0からカウント)
        int xCount = (index - 1) % buttonsInRow;

        // 行内インデックスをもとに、基準点からどれだけ離れているかを計算します。
        float offsetPosX = xCount * width;

        // 基準点にオフセットを加えたものを配置する位置のX成分としてセットします。
        pos.x = basePosX + offsetPosX;

        // 基準点のY成分を計算します。(1.0fをかけて明示的にfloatにしないと小数点以下が切り捨てられます)
        float basePosY = 1.0f * (buttonsInRow - 1) / 2 * height;

        // 何行目かを取得するため、商を計算します。(インデックスは0からカウント)
        int yCount = (index - 1) / buttonsInRow;

        // 行数インデックスをもとに、基準点からどれだけ離れているかを計算します。
        float offsetPosY = -1.0f * yCount * height;

        // 基準点にオフセットを加えたものを配置する位置のX成分としてセットします。
        pos.y = basePosY + offsetPosY;

        return pos;
    }

    /// <Summary>
    /// パズルピースを生成します。
    /// </Summary>
    /// <param name="buttonNum">ボタンの数(平方数で指定)</param>
    /// <param name="index">生成するピースのインデックス</param>
    public GameObject GeneratePuzzlePieces(int buttonNum, int index)
    {
        // インスタンス化するピースの数を計算します。
        int pieceNum = buttonNum - 1;

        // 1行に配置されるピースの数を計算します。
        int piecesInRow = (int)Mathf.Sqrt(buttonNum);

        // インスタンス化するPrefabを取得します。
        GameObject piecePrefab = GetPuzzlePiecePrefab();

        // インスタンス化するピースのサイズを取得します。
        RectTransform pieceRect = piecePrefab.GetComponent<RectTransform>();
        float pieceWidth = pieceRect.rect.width;
        float pieceHeight = pieceRect.rect.height;

        // ピースをインスタンス化します。
        GameObject pieceObj = Instantiate(piecePrefab, Vector3.zero, Quaternion.identity);

        // 生成したオブジェクトの名前を変更します。
        pieceObj.name = $"{piecePrefab.name}{index}";

        // 生成したオブジェクトをピースオブジェクトの親オブジェクトの下に配置します。
        pieceObj.transform.SetParent(puzzlePieceParent.transform);

        // ピースを配置する位置を計算します。
        Vector2 piecePos = GetPuzzlePartsPos(index, piecesInRow, pieceWidth, pieceHeight);

        // 計算した位置を格納します。
        pieceObj.transform.localPosition = piecePos;

        // 親オブジェクトの縮尺をセットします。
        pieceObj.transform.localScale = puzzlePieceParent.transform.localScale;

        // このピースにセットするスプライトを取得します。
        Sprite sprite = GetPieceSprite(index, piecesInRow);

        // ピースのIDとスプライトをセットします。
        PuzzlePieceController controller = pieceObj.GetComponent<PuzzlePieceController>();
        controller.SetUpPiece(index, sprite);

        return pieceObj;
    }

    /// <Summary>
    /// インデックスからピースにセットするスプライトを返します。
    /// </Summary>
    /// <param name="index">インスタンス化しようとしているピースのインデックス</param>
    /// <param name="buttonsInRow">1行に配置されるピースの数</param>
    Sprite GetPieceSprite(int index, int piecesInRow)
    {
        // 何行目かを取得するため、商を計算します。(インデックスは0からカウント)
        int rowCount = (index - 1) / piecesInRow;

        // スプライトリストの大きさを取得します。
        int spriteNum = pieceSprites.Count;

        // スプライトリストの大きさが0以下の場合、スプライトがセットされていないのでエラーを出力します。
        if (spriteNum <= 0)
        {
            Debug.LogError("PuzzleManagerにピースのスプライトがセットされていません。");
            return null;
        }

        // 行数がスプライトリストの大きさを超えないように、リストの大きさで割った剰余を使います。
        int spriteIndex = rowCount % spriteNum;

        // スプライトを返します。
        return pieceSprites[spriteIndex];
    }

    /// <Summary>
    /// パズルのモードに応じたボタンのPrefabを返します。
    /// </Summary>
    GameObject GetPuzzleButtonPrefab()
    {
        // デフォルトのPrefabとして8パズルのボタンをセットします。
        GameObject puzzleButtonPrefab = puzzleButton8Prefab;

        // 現在のモードを確認します。
        int currentMode = PuzzleModeHolder.GetModeId();

        // switch文で現在のモードによってPrefabを切り替えます。
        switch (currentMode)
        {
            case PuzzleModeHolder.Mode15Puzzle:
                puzzleButtonPrefab = puzzleButton15Prefab;
                break;
            case PuzzleModeHolder.Mode24Puzzle:
                puzzleButtonPrefab = puzzleButton24Prefab;
                break;
        }

        return puzzleButtonPrefab;
    }

    /// <Summary>
    /// パズルのモードに応じたピースのPrefabを返します。
    /// </Summary>
    GameObject GetPuzzlePiecePrefab()
    {
        // デフォルトのPrefabとして8パズルのピースをセットします。
        GameObject puzzlePiecePrefab = puzzlePiece8Prefab;

        // 現在のモードを確認します。
        int currentMode = PuzzleModeHolder.GetModeId();

        // switch文で現在のモードによってPrefabを切り替えます。
        switch (currentMode)
        {
            case PuzzleModeHolder.Mode15Puzzle:
                puzzlePiecePrefab = puzzlePiece15Prefab;
                break;
            case PuzzleModeHolder.Mode24Puzzle:
                puzzlePiecePrefab = puzzlePiece24Prefab;
                break;
        }

        return puzzlePiecePrefab;
    }
}


