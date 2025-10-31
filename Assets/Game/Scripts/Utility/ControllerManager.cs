using R3;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// コントローラー管理
/// </summary>
public class ControllerManager : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのInputActions配列
    /// </summary>
    private InputActions[] inputActions_ = new InputActions[4];

    /// <summary>
    /// 接続されているコントローラー数プロパティ
    /// </summary>
    private ReactiveProperty<int> connectedControllerNum_ = new ReactiveProperty<int>();

    /// <summary>
    /// 接続されているコントローラーのObserver
    /// </summary>
    public Observable<int> ConnectedControllers => connectedControllerNum_.AsObservable();

    private void Awake()
    {
        // InputActionsの初期化
        for (int i = 0; i < inputActions_.Length; i++)
        {
            inputActions_[i] = new InputActions();
            inputActions_[i].Enable();
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        // デバイス一覧を取得
        // 接続されているゲームパッドの数をカウント
        var gamepadCount = Gamepad.all.Count;
        Debug.Log($"接続されているコントローラー数: {gamepadCount}");
        // プロパティに反映
        connectedControllerNum_.Value = gamepadCount;
    }

    /// <summary>
    /// プレイヤーごとのInputActionsを取得
    /// </summary>
    /// <param name="playerNum">プレイヤー番号</param>
    /// <returns></returns>
    public InputActions GetPlayerInput(int playerNum)
    {
        return inputActions_[playerNum-1];
    }
}