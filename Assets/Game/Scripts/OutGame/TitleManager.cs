///
/// タイトルマネージャー
///
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        // エスケープキーが押されたらゲーム終了
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ゲーム終了
            Application.Quit();
        }
        // Aボタンで次のシーンに進む
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // プレイヤー登録シーンへ遷移
            SceneManager.LoadScene(SceneNameConst.RegisterPlayerSceneName);
        }
    }
}
