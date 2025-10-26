using UnityEngine;

/// <summary>
/// ミノをスポーンさせる
/// </summary>
public class SpawnMino : MonoBehaviour
{
    [Header("スポーンさせるミノたち"), SerializeField]
    private GameObject[] minos_;

    [Header("次に生成するミノの表示位置Transform"),SerializeField]
    private Transform nextMinoViewPointTransform_;

    [Header("プレイヤー"),SerializeField]
    private Player player_;

    /// <summary>
    /// 次にスポーンするミノのGameObject
    /// </summary>
    private GameObject nextMino_;

    /// <summary>
    /// 初期スポーン
    /// </summary>
    public Mino InitSpawn()
    {
        // 一番最初のミノを生成
        var firstMino = Instantiate(
            minos_[Random.Range(0, minos_.Length)],
            transform.position,
            Quaternion.identity,
            transform
        );

        return firstMino.GetComponent<Mino>();
    }

    /// <summary>
    /// ミノをスポーン
    /// </summary>
    public Mino Spawn()
    {
        // 表示中のミノをスポーン位置に移動
        nextMino_.transform.position = transform.position;

        // ミノの親をスポナーにする
        nextMino_.transform.SetParent(transform);

        var mino = nextMino_.GetComponent<Mino>();
        // 新しいミノを追加
        player_.AddMino(mino);

        return nextMino_.GetComponent<Mino>();
    }

    /// <summary>
    /// 次にスポーンさせるミノを表示
    /// </summary>
    public Mino ViewNextMino()
    {
        // nextTetrimino_ミノを生成し格納
        nextMino_ = null;
        nextMino_ = Instantiate(
            minos_[Random.Range(0, minos_.Length)],
            nextMinoViewPointTransform_.position,
            Quaternion.identity,
            nextMinoViewPointTransform_
        );

        return nextMino_.GetComponent<Mino>();
    }
}
