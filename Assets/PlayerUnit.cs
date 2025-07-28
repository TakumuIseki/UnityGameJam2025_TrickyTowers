using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject[] tetrominoPrefabs; // テトリミノ種類のプレハブ

    private GameObject currentTetromino;

    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode rotateKey = KeyCode.UpArrow;
    public KeyCode dropKey = KeyCode.DownArrow;

    void Update()
    {
        if (currentTetromino == null) return;

        // 操作
        if (Input.GetKeyDown(leftKey)) currentTetromino.transform.position += Vector3.left;
        if (Input.GetKeyDown(rightKey)) currentTetromino.transform.position += Vector3.right;
        if (Input.GetKeyDown(rotateKey)) currentTetromino.transform.Rotate(0, 0, -90f);
        if (Input.GetKeyDown(dropKey)) currentTetromino.transform.position += Vector3.down * 2;
    }

    public void SpawnNewTetromino()
    {
        int index = Random.Range(0, tetrominoPrefabs.Length);
        GameObject tetro = Instantiate(tetrominoPrefabs[index], spawnPoint.position, Quaternion.identity, transform);
        currentTetromino = tetro;

        // テトリミノ自身に「着地したらPlayerUnitに通知する処理」が必要
        var controller = tetro.GetComponent<TetrominoController>();
        controller.owner = this;
    }

    public void OnTetrominoLanded()
    {
        currentTetromino = null;
        Invoke(nameof(SpawnNewTetromino), 0.5f); // 次のミノ出現
    }
}
