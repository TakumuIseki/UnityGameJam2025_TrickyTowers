///
/// コントローラー管理
///
using R3;
using System.Linq;
using UnityEngine;

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
    /// オブジェクトがアクティブになったときに1度だけ呼び出される
    /// </summary>
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        // コントローラーの接続状態を更新

        var names = Input.GetJoystickNames();
        var newJoysticks = names.Where(name => !string.IsNullOrEmpty(name)).ToList();

        // 接続コントローラー数が変わった場合のみ更新通知
        if (connectedControllerNum_.Value != newJoysticks.Count)
        {
            connectedControllerNum_.Value = newJoysticks.Count;
        }
    }
}
