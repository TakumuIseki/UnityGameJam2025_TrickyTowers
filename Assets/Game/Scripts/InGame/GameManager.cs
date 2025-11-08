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
    [Header("カウントダウン"), SerializeField]
    private CountDown countDown_;

    [Header("制限時間タイマー"), SerializeField]
    private GameLimitTimer gameLimitTimer_;

    [Header("プレイヤープレハブ"), SerializeField]
    private GameObject playerPrefab_;

    [Header("プレイヤールートオブジェクト"), SerializeField]
    private Transform playerRootObj_;

    [Header("制限時間"), SerializeField]
    private GameObject gameLimitTimerObj_;

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
        for (var i = 0; i < GameData.Instance.JoinPlayerCount; i++)
        {
            joinPlayers_[i] = PlayerInput.Instantiate(
                playerIndex: i,
                pairWithDevice: ControllerManager.Instance.TryGetInputDevice(i + 1, out InputDevice device),
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
        // カウントダウン
        await countDown_.CoundDownTask();

        // インゲームBGM再生
        SoundManager.PlayBGM("BgmGame");

        // 待機ステート→落下操作ステートに切り替え
        for (var i = 0; i < GameData.Instance.JoinPlayerCount; i++)
        {
            var player = joinPlayers_[i].GetComponent<Player>();
            player.StartControlFall();
        }

        // 制限時間タイマー表示
        gameLimitTimerObj_.SetActive(true);

        // 制限時間測定
        await gameLimitTimer_.MeasureLimitTimeTask();

        // インゲームBGM停止
        SoundManager.StopBGM();

        // シーン内のMinoタグを持つオブジェクトを全て取得して削除
        var minoObjects = GameObject.FindGameObjectsWithTag("Mino");
        foreach (var mino in minoObjects)
        {
            Destroy(mino);
        }

        // ゲーム終了SE再生
        await SoundManager.PlaySEAsync("SeEndBuzzer");

        // １秒待機
        await UniTask.Delay(System.TimeSpan.FromSeconds(1));

        // ドラムロールSE再生
        await SoundManager.PlaySEAsync("SeDrumRoll");

        // リザルトBGM再生
        SoundManager.PlayBGM("BgmRanking");

        // 拍手SE再生
        SoundManager.PlaySE("SeClap");

        // 勝利プレイヤーたちの勝利演出エフェクト再生
        for (var i = 0; i < GameData.Instance.WinPlayerNumList.Count; i++)
        {
            var winnerPlayerNumber = GameData.Instance.WinPlayerNumList[i];
            var player = joinPlayers_[winnerPlayerNumber - 1].GetComponent<Player>();
            player.PlayWinEffect();
        }

        // 1位のプレイヤーたちのStartボタンが押されるまで待機
        await UniTask.WaitUntil(() =>
        {
            foreach (var winnerPlayerNumber in GameData.Instance.WinPlayerNumList)
            {
                var playerInput = joinPlayers_[winnerPlayerNumber - 1].GetComponent<PlayerInput>();
                if (playerInput.actions["Pause"].WasPressedThisFrame())
                {
                    return true;
                }
            }
            return false;
        });

        // BGMを停止
        SoundManager.StopBGM();

        // タイトルシーンへ遷移
        SceneManager.LoadScene(SceneNameConst.TitleSceneName);
    }
}
