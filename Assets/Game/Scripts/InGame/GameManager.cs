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

    [Header("制限時間タイマー"), SerializeField]
    private GameLimitTimer gameLimitTimer_;

    // TODO: プレイヤー数はプレイヤー登録シーンで決定された人数を使う
    [Header("参加プレイヤー数"), SerializeField]
    private int joinPlayerCount = 2;

    [Header("プレイヤープレハブ"), SerializeField]
    private GameObject playerPrefab_;

    [Header("プレイヤールートオブジェクト"), SerializeField]
    private Transform playerRootObj_;

    /// <summary>
    /// 参加プレイヤーたち
    /// </summary>
    private GameObject[] joinPlayers_;

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // 参加プレイヤー配列初期化
        joinPlayers_ = new GameObject[joinPlayerCount];

        // プレイヤー生成
        for(var i = 0; i < joinPlayerCount; i++)
        {
            joinPlayers_[i] = Instantiate(
                playerPrefab_,
                Vector3.zero,
                Quaternion.identity,
                // 親をプレイヤールートオブジェクトに設定
                playerRootObj_
            );
            joinPlayers_[i].name = $"Player{i + 1}";
        }

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
        for(var i = 0; i < joinPlayerCount; i++)
        {
            var player = joinPlayers_[i].GetComponent<Player>();
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
