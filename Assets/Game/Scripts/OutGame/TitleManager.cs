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
        if (Input.anyKeyDown)
        {
            // プレイヤー登録シーンへ遷移
            SceneManager.LoadScene(SceneNameConst.RegisterPlayerSceneName);
        }
    }
}
