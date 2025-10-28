using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルマネージャー
/// </summary>
public class TitleManager : MonoBehaviour
{
    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // コントローラーBボタンが押されたらゲーム終了
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            // ゲーム終了
            Application.Quit();
        }
        // Aボタンで次のシーンに進む
        else if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            // プレイヤー登録シーンへ遷移
            SceneManager.LoadScene(SceneNameConst.RegisterPlayerSceneName);
        }
    }
}