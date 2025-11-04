using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// コントローラー接続状況
/// </summary>
public class RegisterPlayerSceneManager : MonoBehaviour
{
    [Header("プレイヤー入力"), SerializeField]
    private ConnectController[] connectControllers_;

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        // 誰かがスタートボタンを押したらインゲームに遷移誰かがスタートボタンを押したらインゲームに遷移
        foreach(var controller in connectControllers_)
        {
            if(controller.IsReady && ControllerManager.Instance.IsPressed(controller.PlayerNum,UnityEngine.InputSystem.LowLevel.GamepadButton.Start))
            {
                // インゲームシーンへ遷移
                SceneManager.LoadScene(SceneNameConst.GameSceneName);
            }
        }
    }
}