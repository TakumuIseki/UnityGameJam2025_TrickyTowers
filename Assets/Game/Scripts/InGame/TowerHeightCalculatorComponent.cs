/// <summary>
/// 積み上げた高さを計算し、表示するスクリプト。
/// </summary>
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerHeightCalculatorComponent : MonoBehaviour
{
    [Header("高さ表示UI"),SerializeField]
    private TextMeshProUGUI heightText_;

    private CartHeightCalculatorComponent cartHeight_;   // カートの高さを管理するコンポーネント。

    private float globalMaxY_ = 0f;             // このテトリミノの最も高い頂点のy座標を格納する。
    private GameObject[] tetriminos_;            //Towerタグの付いたテトリミノを格納する。
    private Transform playerUnit_;

    private void Start()
    {
        SetupPlayerUnit();
        FindCartHeightCalculatorComponent();

        // tetoriminosがnullにならないよう最初に呼び出す。
        FindTetriminosWithTagTower();
    }

    void Update()
    {
        CalcMaxHeightOfAllTetriminos();

        // 高さを表示。
        heightText_.text = $"たかさ : {(int)globalMaxY_}";
    }

    /// <summary>
    /// 同じPlayerUnitに属しているオブジェクトの中から、
    /// Towerタグの付いたテトリミノを探し、tetriminosに格納する。。
    /// </summary>
    public void FindTetriminosWithTagTower()
    {
        // 同じPlayerUnitに属する全てのオブジェクトを取得。
        var allChildren = playerUnit_.GetComponentsInChildren<Transform>(true);

        // テトリミノのリストを作成。
        List<GameObject> localTetriminos = new List<GameObject>();

        // タワータグのついたオブジェクトをリストに追加。
        foreach (var child in allChildren)
        {
            if (child.CompareTag("Tower"))
            {
                localTetriminos.Add(child.gameObject);
            }
        }

        // リストを配列に変換。
        tetriminos_ = localTetriminos.ToArray();
    }

    /// <summary>
    /// PlayerUnitをセットする。
    /// </summary>
    private void SetupPlayerUnit()
    {
        // 自身の親（PlayerUnit）をセット。
        playerUnit_ = transform.parent;

        // 自身の親（PlayerUnit）が見つからない場合は警告。
        if (playerUnit_ == null)
        {
            Debug.LogError($"PlayerUnitが見つかりません。親子関係を確認してください。");
        }
    }

    private void FindCartHeightCalculatorComponent()
    {
        // 自分が属しているPlayerUnitの中から、
        // CartHeightCalculatorComponentを取得してセット。
        cartHeight_ = playerUnit_.GetComponentInChildren<CartHeightCalculatorComponent>();

        // CartHeightCalculatorComponentが見つからない場合は警告。
        if (cartHeight_ == null)
        {
            Debug.LogError($"CartHeightCalculatorComponentが見つかりません。" +
                $"同じPlayerUnit内でアタッチされているか確認してください。");
        }
    }

    /// <summary>
    /// 各テトリミノの最も高い頂点を比較し、
    /// 全テトリミノの中から最も高い頂点のy座標を計算する。
    /// </summary>
    private void CalcMaxHeightOfAllTetriminos()
    {
        // 初期化。
        globalMaxY_ = 0f;

        foreach (GameObject tetrimino in tetriminos_)
        {
            // テトリミノが消えた瞬間Findがかかる前にこの処理が走り、
            // 消えたテトリミノの高さを算出しようとしてnullを返すので、
            // このnullを無視する。
            if (tetrimino == null)
            {
                return;
            }

            // テトリミノのコンポーネントを取得。
            var minoHeight = tetrimino.GetComponent<TetriminoHeightCalculatorComponent>();

            // カート基準でテトリミノの高さを算出。
            float topY = minoHeight.TopY - cartHeight_.TopY;

            // 今の最高高度を上回ったら上書き。
            globalMaxY_ = Mathf.Max(globalMaxY_, topY);
        }
    }
}
