/// <summary>
/// 積み上げた高さを計算し、表示するスクリプト。
/// </summary>
using TMPro;
using UnityEngine;

public class TowerHeightManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI heightText_;   // 高さ表示UI。

    [SerializeField] private GameObject cart_;              // カートをアタッチ。

    private float globalMaxY_ = 0f;                         // このテトリミノの最も高い頂点のy座標を格納する。

    void Update()
    {
        CalcMaxHeightOfAllTetriminos();

        // 高さを表示。
        heightText_.text = $"高さ: {(int)globalMaxY_}";
    }

    /// <summary>
    /// 各テトリミノの最も高い頂点を比較し、
    /// 全テトリミノの中から最も高い頂点のy座標を計算する。
    /// </summary>
    private void CalcMaxHeightOfAllTetriminos()
    {
        // 初期化。
        globalMaxY_ = 0f;

        // Towerタグの付いたテトリミノを取得。
        GameObject[] tetriminos = GameObject.FindGameObjectsWithTag("Tower");

        foreach (GameObject tetrimino in tetriminos)
        {
            // テトリミノのコンポーネントを取得。
            var minoHeight = tetrimino.GetComponent<TetriminoHeightCalculator>();

            // カートのコンポーネントを取得。
            var cartHeight = cart_.GetComponent<CartHeightCalculator>();

            // カート基準でテトリミノの高さを算出。
            float topY = minoHeight.maxY_ - cartHeight.maxY_;

            // 今の最高高度を上回ったら上書き。
            if (topY > globalMaxY_)
            {
                globalMaxY_ = topY;
            }
        }
    }
}
