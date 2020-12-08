using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PuzzlePieceController : MonoBehaviour
{
    public TextMeshProUGUI pieceNumber;


    public void SetUpPiece(int pieceId, Sprite sp)
    {
        // ピースの番号をテキストにセットします。
        pieceNumber.SetText(pieceId.ToString());

        // ピースのスプライト画像をセットします。
        Image pieceImage = gameObject.GetComponent<Image>();
        pieceImage.sprite = sp;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
