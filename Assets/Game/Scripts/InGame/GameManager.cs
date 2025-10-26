using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームマネージャー
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("カウントダウン"),SerializeField]
    private CountDown countDown_;

    [Header("制限時間タイマー"),SerializeField]
    private GameLimitTimer gameLimitTimer_;

    [Header("プレイヤーユニットたち"),SerializeField]
    private Player[] players_;

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        GameFlowTask().Forget();
    }

    /// <summary>
    /// ゲームフロータスク
    /// </summary>
    /// <returns></returns>
    private async UniTask GameFlowTask()
    {
        Debug.Log("カウントダウン開始");

        // カウントダウン
        await countDown_.CoundDownTask();

        Debug.Log("ゲーム開始");

        // 待機ステート→落下操作ステートに切り替え
        foreach (var player in players_)
        {
            player.StartControlFall();
        }

        // 制限時間測定
        await gameLimitTimer_.MeasureLimitTimeTask();

        Debug.Log("ゲーム終了");

        // 終了
        // await

        Debug.Log("フェードアウト開始");

        // フェードアウト
        // await

        Debug.Log("リザルトに移行");

        // シーン遷移
        SceneManager.LoadScene(SceneNameConst.RankingSceneName);
    }
}
