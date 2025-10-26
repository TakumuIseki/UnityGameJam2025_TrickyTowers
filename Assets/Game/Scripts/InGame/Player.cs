using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー
/// </summary>
public class Player : MonoBehaviour
{
    [Header("プレイヤー番号"),SerializeField]
    private int playerNum_;

    [Header("スポナー"),SerializeField]
    private SpawnMino spawnerMino_;

    /// <summary>
    /// スポーンさせたミノたち
    /// </summary>
    private List<Mino> minoList_ = new List<Mino>();

    /// <summary>
    /// 現在操作中のミノ
    /// </summary>
    private Mino currentMino_;

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // ミノを初期スポーン
        var initSpawnMino = spawnerMino_.InitSpawn();
        initSpawnMino.SetSpawner(spawnerMino_);
        AddMino(initSpawnMino);
        
        // 次のミノを表示（操作対象ではない）
        var nextSpawnMino = spawnerMino_.ViewNextMino();
        nextSpawnMino.SetSpawner(spawnerMino_);
        minoList_.Add(nextSpawnMino);
    }

    /// <summary>
    /// 落下操作開始
    /// </summary>
    public void StartControlFall()
    {
        // 落下操作ステートに変更
        currentMino_.ControlFallState();
    }

    /// <summary>
    /// 次のミノをスポーン
    /// </summary>
    private void SpawnNextMino()
    {
        var spawnMino = spawnerMino_.Spawn();
        spawnMino.SetSpawner(spawnerMino_);
        var nextSpawnMino = spawnerMino_.ViewNextMino();
        nextSpawnMino.SetSpawner(spawnerMino_);
    }

    /// <summary>
    /// 新しいミノを追加
    /// </summary>
    /// <param name="mino">追加するミノ</param>
    public void AddMino(Mino mino)
    {
        minoList_.Add(mino);
        currentMino_ = mino;
    }

    /// <summary>
    /// ミノを削除
    /// </summary>
    /// <param name="mino">削除するミノ</param>
    public void RemoveMino(Mino mino)
    {
        minoList_.Remove(mino);
        
        // 削除されたミノが現在のミノだった場合
        if (currentMino_ == mino)
        {
            currentMino_ = null;
        }
    }
}
