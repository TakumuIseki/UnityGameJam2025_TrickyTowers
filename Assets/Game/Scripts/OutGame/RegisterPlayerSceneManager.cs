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
                // BGM停止
                SoundManager.StopBGM();

                // プレイヤー数を登録
                GameData.Instance.JoinPlayerCount = 0;
                foreach (var connectController in connectControllers_)
                {
                    // 準備完了しているプレイヤー数をカウント
                    if (connectController.IsReady)
                    {
                        GameData.Instance.JoinPlayerCount++;
                    }
                }
                
                // 決定SE再生
                SoundManager.PlaySE("SeDecision");

                // インゲームシーンへ遷移
                SceneManager.LoadScene(SceneNameConst.GameSceneName);
            }
        }
    }
}