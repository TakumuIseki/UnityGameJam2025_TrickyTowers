using R3;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// コントローラー管理
/// </summary>
public class ControllerManager : MonoBehaviour
{
    /// <summary>
    /// 接続されているコントローラー数プロパティ
    /// </summary>
    private ReactiveProperty<int> connectedControllerNum_ = new ReactiveProperty<int>();

    /// <summary>
    /// 接続されているコントローラーのObserver
    /// </summary>
    public Observable<int> ConnectedControllers => connectedControllerNum_.AsObservable();

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        // デバイス一覧を取得
        foreach (var device in InputSystem.devices)
        {
            // コントローラー以外はスキップ
            if (device is not Gamepad)
            {
                continue;
            }
            // デバイス名をログ出力
            Debug.Log(device.name);
        }
        // 接続されているゲームパッドの数をカウント
        var gamepadCount = Gamepad.all.Count;
        Debug.Log($"接続されているコントローラー数: {gamepadCount}");
        // プロパティに反映
        connectedControllerNum_.Value = gamepadCount;
    }
}
