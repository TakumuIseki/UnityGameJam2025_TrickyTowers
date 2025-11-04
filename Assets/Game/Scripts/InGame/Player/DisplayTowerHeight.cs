using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// タワーの積み上げた高さを計算し表示
/// </summary>
public class DisplayTowerHeight : MonoBehaviour
{
    [Header("高さ表示UI"),SerializeField]
    private TextMeshProUGUI heightText_;

    [Header("プレイヤーユニット"),SerializeField]
    private Transform player_;

    [Header("カートの高さ計算コンポーネント"),SerializeField]
    private CalcCartHeight cartHeight_;

    /// <summary>
    /// このミノの最も高い頂点のy座標
    /// </summary>
    private float globalMaxY_ = 0f;

    /// <summary>
    /// Towerタグの付いミノ
    /// </summary>
    private GameObject[] minos_;

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // minosがnullにならないよう最初に呼び出す
        FindMinosWithTag();
    }

    /// <summary>
    /// Update
    /// </summary>
    void Update()
    {
        FindMinosWithTag();
        CalcMaxHeightOfAllMinos();

        // 高さを表示。
        heightText_.text = $"たかさ : {(int)globalMaxY_}";
    }

    /// <summary>
    /// 同じプレイヤーユニットに属しているオブジェクトの中から指定したタグの付いたオブジェクト(ミノ)を探す
    /// </summary>
    public void FindMinosWithTag()
    {
        // 同じプレイヤーに属する全てのオブジェクトを取得
        var allChildren = player_.GetComponentsInChildren<Transform>(true);

        // ミノのリストを作成
        var localMinos = new List<GameObject>();

        // タワータグのついたオブジェクトをリストに追加
        foreach (var child in allChildren)
        {
            if (child.CompareTag("Tower"))
            {
                localMinos.Add(child.gameObject);
            }
        }

        // リストを配列に変換
        minos_ = localMinos.ToArray();
    }

    /// <summary>
    /// 各ミノの最も高い頂点を比較し、全ミノの中から最も高い頂点のy座標を計算
    /// </summary>
    private void CalcMaxHeightOfAllMinos()
    {
        // 初期化
        globalMaxY_ = 0f;

        foreach (var mino in minos_)
        {
            if (!mino)
            {
                return;
            }

            // ミノのコンポーネントを取得
            var minoHeight = mino.GetComponent<CalcMinoHeight>();

            // カート基準でミノの高さを算出
            //float topY = minoHeight.TopY - cartHeight_.TopY;
            float topY = minoHeight.TopY;

            // 今の最高高度を上回ったら上書き。
            globalMaxY_ = Mathf.Max(globalMaxY_, topY);
        }
    }
}
