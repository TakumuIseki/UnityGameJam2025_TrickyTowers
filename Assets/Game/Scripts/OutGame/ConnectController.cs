using R3;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

/// <summary>
/// コントローラー接続処理
/// </summary>
public class ConnectController : MonoBehaviour
{
    [Header("プレイヤー番号"), SerializeField]
    private int playerNum_ = 0;

    [Header("コントローラー接続画像"), SerializeField]
    private Sprite ConectControllerSprite;

    [Header("プレイヤー参加画像"), SerializeField]
    private Sprite RegisterPlayerSprite;

    [Header("プレイヤー登録画像"), SerializeField]
    private Image RegisterPlayerImage;

    /// <summary>
    /// コントローラー管理
    /// </summary>
    private ControllerManager controllerManager_;

    /// <summary>
    /// オブジェクトがアクティブになったときに1度だけ呼び出される
    /// </summary>
    private void Start()
    {
        controllerManager_ = FindObjectOfType<ControllerManager>();

        // コントローラーの接続状況を監視
        controllerManager_.ConnectedControllers.Subscribe(count =>
            {
                if (count >= playerNum_)
                {
                    RegisterPlayerImage.sprite = RegisterPlayerSprite;
                    GoReadyTask().Forget();
                }
                else
                {
                    RegisterPlayerImage.sprite = ConectControllerSprite;
                    ChangeColor(Color.white);
                }
            }
        );
    }

    /// <summary>
    /// プレイヤー準備完了までのタスク
    /// </summary>
    private async UniTask GoReadyTask()
    {
        // プレイヤーのAボタンが押されるまで待機
        await UniTask.WaitUntil(() => 
            controllerManager_.GetPlayerInput(playerNum_).OutGameScene.A.WasPressedThisFrame()
        );

        // プレイヤー準備完了状態
        ChangeColor(Color.yellow);

        BackReadyTask().Forget();
    }

    /// <summary>
    /// プレイヤー準備中までのタスク
    /// </summary>
    private async UniTask BackReadyTask()
    {
        // Bボタンが押されるまで待機
        await UniTask.WaitUntil(() => 
            controllerManager_.GetPlayerInput(playerNum_).OutGameScene.B.WasPressedThisFrame()
        );

        // プレイヤー準備中状態
        ChangeColor(Color.white);

        GoReadyTask().Forget();
    }

    /// <summary>
    /// 色変更
    /// </summary>
    private void ChangeColor(Color color)
    {
        RegisterPlayerImage.color = color;
    }
}
