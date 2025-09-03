/// <summary>
/// テトリミノをスポーンさせるスクリプト。
/// </summary>
using UnityEngine;

public class TetriminoSpawnerComponent : MonoBehaviour
{
    [Header("スポーンさせるテトリミノ"), SerializeField]
    private GameObject[] tetriminos_;

    private Transform playerUnit_;              // 自身の親（PlayerUnit）を格納する。
    private Transform nextTetriminoViewPoint_;  // 同じPlayerUnitに属する、次に生成するテトリミノを表示する場所を格納する。

    void Start()
    {
        SetupPlayerUnit();
        SetupNextTetriminoViewPoint();

        // 最初の1回はStartで表示＆スポーンさせる。
        ViewNextTetrimino();
        SpawnTetrimino();
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

    /// <summary>
    /// NextTetriminoViewPointをセットする。
    /// </summary>
    private void SetupNextTetriminoViewPoint()
    {
        // 同じPlayerUnitに属するNextTetriminoViewPointを検索。
        nextTetriminoViewPoint_ = playerUnit_.Find("NextTetriminoViewPoint");

        // NextTetriminoViewPointが見つからない場合は警告。
        if (nextTetriminoViewPoint_ == null)
        {
            Debug.LogError($"NextTetriminoViewPointが見つかりません。");
        }
    }
    private GameObject nextTetrimino_;

    /// <summary>
    /// テトリミノをスポーンさせる処理。
    /// </summary>
    public void SpawnTetrimino()
    {
        // 表示中のテトリミノをスポーン位置に移動。
        nextTetrimino_.transform.position = transform.position;

        // テトリミノの親をスポナーにする。
        nextTetrimino_.transform.SetParent(transform);

        // テトリミノの操作を有効化。
        var controller = nextTetrimino_.GetComponent<TetriminoControllerComponent>();
        controller.SetSpawner(this);
        controller.enabled = true;

        // 次のテトリミノを生成して表示。
        ViewNextTetrimino();
    }

    /// <summary>
    /// 次にスポーンさせるテトリミノを表示。
    /// </summary>
    private void ViewNextTetrimino()
    {
        // nextTetrimino_にテトリミノを生成して格納。
        nextTetrimino_ = Instantiate(tetriminos_[Random.Range(0, tetriminos_.Length)],
            nextTetriminoViewPoint_.position, Quaternion.identity);

        // nextTetrimino_をNextTetriminoViewPointの子に設定。
        nextTetrimino_.transform.SetParent(nextTetriminoViewPoint_);

        // 表示用なので操作は無効化。
        var controller = nextTetrimino_.GetComponent<TetriminoControllerComponent>();
        controller.enabled = false;
    }
}
