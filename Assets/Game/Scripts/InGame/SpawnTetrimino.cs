/// <summary>
/// テトリミノをスポーンさせるスクリプト。
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetrimino : MonoBehaviour
{
    [SerializeField]
    private GameObject[] Tetriminos; // テトリミノを格納する配列。

    void Start()
    {
        // 最初の1回はStartでスポーンさせる。
        Spawn();
    }

    /// <summary>
    /// テトリミノをスポーンさせる処理。
    /// </summary>
    public void Spawn()
    {
        // Tetriminosに格納されたオブジェクトの中からランダムで1つスポーンさせます。
        GameObject tetrimino = Instantiate(Tetriminos[Random.Range(0, Tetriminos.Length)], transform.position, Quaternion.identity);

        // テトリミノに親(スポナー)を伝える。
        var block = tetrimino.GetComponent<ControllableBlock>();
        block.SetSpawner(this);
    }
}
