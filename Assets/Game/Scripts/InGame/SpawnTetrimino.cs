/// <summary>
/// テトリミノをスポーンさせるスクリプト。
/// </summary>
using UnityEngine;

public class SpawnTetrimino : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tetriminos_; // テトリミノを格納する配列。

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
        GameObject tetrimino = Instantiate(
            tetriminos_[Random.Range(0, tetriminos_.Length)],
            transform.position,
            Quaternion.identity
        );

        // テトリミノに親(スポナー)を伝える。
        var block = tetrimino.GetComponent<ControllableBlock>();
        block.SetSpawner(this);
    }
}
