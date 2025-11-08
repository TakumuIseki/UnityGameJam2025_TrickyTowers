using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEditor.Timeline.TimelinePlaybackControls;

/// <summary>
/// ゲームマネージャー
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("カウントダウン"),SerializeField]
    private CountDown countDown_;

    [Header("制限時間タイマー"), SerializeField]
    private GameLimitTimer gameLimitTimer_;

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
        joinPlayers_ = new GameObject[GameData.Instance.JoinPlayerCount];

        // プレイヤー生成
        for(var i = 0; i < GameData.Instance.JoinPlayerCount; i++)
        {
            joinPlayers_[i] = PlayerInput.Instantiate(
                playerIndex: i,
                pairWithDevice: ControllerManager.Instance.TryGetInputDevice(i,out InputDevice device),
                prefab: playerPrefab_
                ).gameObject;
            joinPlayers_[i].name = $"Player{i + 1}";
            joinPlayers_[i].transform.SetParent(playerRootObj_);
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

        // ゲーム開始SE再生
        SoundManager.PlaySE("SeEndBuzzer");

        // インゲームBGM再生
        SoundManager.PlayBGM("BgmGame");

        // 待機ステート→落下操作ステートに切り替え
        for(var i = 0; i < GameData.Instance.JoinPlayerCount; i++)
        {
            var player = joinPlayers_[i].GetComponent<Player>();
            player.StartControlFall();
        }

        // 制限時間測定
        await gameLimitTimer_.MeasureLimitTimeTask();

        Debug.Log("ゲーム終了");

        // インゲームBGM停止
        SoundManager.StopBGM();

        // ゲーム終了SE再生
        SoundManager.PlaySE("SeEndBuzzer");

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
