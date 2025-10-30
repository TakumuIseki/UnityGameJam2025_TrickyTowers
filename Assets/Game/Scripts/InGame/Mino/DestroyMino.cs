using UnityEngine;

/// <summary>
/// ミノの破棄機能
/// </summary>
public class DestroyMino : MonoBehaviour
{
    /// <summary>
    /// 破棄するか
    /// </summary>
    private bool _isDestroy => transform.position.y < MinoConst.DESTROY_Y_THRESHOLD;

    /// <summary>
    /// プレイヤー
    /// </summary>
    private Player player_;

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        // ミノのy座標が一定以下になったら破棄
        if(_isDestroy)
        {
            Destroy();
        }
    }

    /// <summary>
    /// プレイヤーを取得
    /// </summary>
    private void FindAndAssignPlayer()
    {
        // 自分が属しているPlayerを取得(ミノ→スポナー→プレイヤー)
        var player = transform.parent.parent;

        // プレイヤーユニットからPlayerコンポーネントを取得
        player_ = player.GetComponent<Player>();
    }

    /// <summary>
    /// ミノを破棄
    /// </summary>
    private void Destroy()
    {
        // プレイヤーのリストからミノを削除
        var mino = GetComponent<Mino>();
        FindAndAssignPlayer();
        player_.RemoveMino(mino);

        // テトリミノにタワータグが付いていたら
        if (gameObject.tag == MinoConst.TOWER_TAG)
        {
            // Towerタグを持ったテトリミノのリストを更新
            //calcTowerHeight_.FindTetriminosWithTagTower();
        }
        else
        {
            // 次のテトリミノをスポーン
            //spawner_.Spawn();
        }

        Destroy(gameObject);
    }
}
