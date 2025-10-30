using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルマネージャー
/// </summary>
public class TitleManager : MonoBehaviour
{
    /// <summary>
    /// プレイヤー入力
    /// </summary>
    private InputActions playerInput_;

    private void Start()
    {
        SoundManager.PlayBGM("BgmTitle");
    }

    /// <summary>
    /// 有効化時
    /// </summary>
    private void OnEnable()
    {
        playerInput_ = new InputActions();

        // 入力アクション有効化(アウトゲーム用マップのみ)
        playerInput_.OutGameScene.Enable();

        // 購読

        // セレクトボタンが押されたらゲーム終了
        playerInput_.OutGameScene.GameEnd.performed += _ => Application.Quit();

        // Aボタンが押されたらプレイヤー登録シーンへ遷移
        playerInput_.OutGameScene.A.performed += _ => SceneManager.LoadScene(SceneNameConst.RegisterPlayerSceneName);
    }

    /// <summary>
    /// 無効化時
    /// </summary>
    private void OnDisable()
    {
        // 入力アクション無効化(アウトゲーム用マップのみ)
        playerInput_.OutGameScene.Disable();
    }

    private void Update()
    {
        // 入力アクションの状態を更新
        if (Gamepad.current == null)
        {
            Debug.Log("コントローラーが接続されていません");
        }
        else
        {
            Debug.Log("コントローラーが接続されています");
        }
    }
}