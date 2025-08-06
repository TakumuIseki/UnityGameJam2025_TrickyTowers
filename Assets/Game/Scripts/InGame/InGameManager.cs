/// <summary>
/// インゲームの進行を管理するスクリプト。
/// </summary>
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    //[SerializeField] private GameObject countDown_;
    //[SerializeField] private SpawnTetrimino spawnManager;
    //[SerializeField] private GameTimer timer;

    //[SerializeField] private float gameDuration = 60f;

    void Start()
    {
        // StartCoroutine(GameFlowCoroutine());
    }

    //private IEnumerator GameFlowCoroutine()
    //{
    //    countDown_.SetActive(true);

    //    // 1. カウントダウン表示
    //    yield return StartCoroutine(uiController.ShowCountdownCoroutine(3));

    //    // 2. テトリミノスポナー開始（コンポーネント有効化）
    //    spawnManager.enabled = true;
    //    spawnManager.StartSpawning();

    //    // 3. ゲームタイマー開始
    //    timer.StartTimer(gameDuration);

    //    // 4. 制限時間待機
    //    yield return new WaitForSeconds(gameDuration);

    //    // 5. ゲーム終了処理
    //    spawnManager.StopSpawning();
    //    spawnManager.enabled = false;
    //    timer.StopTimer();

    //    // 6. スポーン済みのテトリミノをすべて無効化
    //    DisableAllTetriminos();

    //    // 7. ゲームオーバーUI表示
    //    uiController.ShowGameOver();
    //}

    //private void DisableAllTetriminos()
    //{
    //    TetriminoController[] tetriminos = FindObjectsOfType<TetriminoController>();
    //    foreach (var tetrimino in tetriminos)
    //    {
    //        tetrimino.enabled = false;
    //    }
    //}
}
