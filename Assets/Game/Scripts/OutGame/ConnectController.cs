using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem.LowLevel;

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
    /// 準備完了状態か
    /// </summary>
    public bool IsReady { get; private set; }

    /// <summary>
    /// オブジェクトがアクティブになったときに1度だけ呼び出される
    /// </summary>
    private void Start()
    {
        // コントローラー接続状態
        ControllerManager.Instance.OnGamepadConnectionChanged += HandlePadChange;
        ControllerManager.Instance.OnGamepadConnectionChanged += HandlePadChange;
        // コントローラー未接続状態
        RegisterPlayerImage.sprite = ConectControllerSprite;
        ChangeColor(Color.white);
    }

    /// <summary>
    /// プレイヤー準備完了までのタスク
    /// </summary>
    private async UniTask GoReadyTask()
    {
        // プレイヤーのAボタンが押されるまで待機
        await ControllerManager.Instance.WaitForButtonDownAsync(playerNum_,GamepadButton.A);

        // プレイヤー準備完了状態
        ChangeColor(Color.yellow);
        IsReady = true;
        BackReadyTask().Forget();
    }

    /// <summary>
    /// プレイヤー準備中までのタスク
    /// </summary>
    private async UniTask BackReadyTask()
    {
        // Bボタンが押されるまで待機
        await ControllerManager.Instance.WaitForButtonDownAsync(playerNum_,GamepadButton.B);

        // プレイヤー準備中状態
        ChangeColor(Color.white);
        IsReady = false;
        GoReadyTask().Forget();
    }

    /// <summary>
    /// 色変更
    /// </summary>
    private void ChangeColor(Color color)
    {
        RegisterPlayerImage.color = color;
    }

    /// <summary>
    /// コントローラー接続状態変更ハンドラー
    /// </summary>
    private void HandlePadChange(int playerNum,bool connected)
    {
        RegisterPlayerImage.sprite = RegisterPlayerSprite;
        GoReadyTask().Forget();
    }
}
