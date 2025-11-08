using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DecidePlayerNum : MonoBehaviour
{
    [Header("プレイヤーカラー"),SerializeField]
    private Color[] playerColors_ = new Color[4];

    [Header("プレイヤー番号テキスト"),SerializeField]
    private TextMeshProUGUI playerNumText_;

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // 親オブジェクトの名前からプレイヤー番号(int)を取得
        var parentName = transform.parent.parent.name; // 例: "Player1"
        var playerNumStr = parentName.Replace("Player", ""); // "1"

        // プレイヤー番号を表示&カラー設定
        if (int.TryParse(playerNumStr, out int playerNum))
        {
            playerNumText_.text = $"{playerNum}P";
            playerNumText_.color = playerColors_[playerNum - 1];
        }
    }
}