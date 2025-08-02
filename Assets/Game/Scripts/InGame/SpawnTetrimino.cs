/// <summary>
/// テトリミノをスポーンさせるスクリプト。
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetrimino : MonoBehaviour
{
    public GameObject[] Tetriminos; // テトリミノを格納する配列。

    void Start()
    {
        Spawn();
    }

    void Update()
    {

    }

    public void Spawn()
    {
        // Tetriminosに格納されたオブジェクトの中からランダムで1つスポーンさせます。
        GameObject tetrimino = Instantiate(Tetriminos[Random.Range(0, Tetriminos.Length)], transform.position, Quaternion.identity);

        ControllableBlock block = tetrimino.GetComponent<ControllableBlock>();
        block.SetSpawner(this);
    }
}
