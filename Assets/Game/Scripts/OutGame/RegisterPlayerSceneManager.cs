using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// コントローラー接続状況
/// </summary>
public class RegisterPlayerSceneManager : MonoBehaviour
{
    [Header("プレイヤー入力"), SerializeField]
    private ConnectController[] connectControllers_;

    private bool ehe;

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        if(ehe)
        {
            return;
        }

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

                ChangeScene();
            }
        }
    }

    /// <summary>
    /// シーン遷移
    /// </summary>
    private async void ChangeScene()
    {
        ehe = true;

        // 決定SE再生
        await SoundManager.PlaySEAsync("SeDecision");
        // インゲームシーンへ遷移
        SceneManager.LoadScene(SceneNameConst.GameSceneName);
    }
}