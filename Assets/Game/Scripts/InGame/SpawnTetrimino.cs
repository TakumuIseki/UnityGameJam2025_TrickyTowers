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
        // Tetriminosに格納されたオブジェクトの中からランダムで1つスポーンさせます。
        Instantiate(Tetriminos[Random.Range(0, Tetriminos.Length)], transform.position, Quaternion.identity);
    }

    void Update()
    {

    }
}
