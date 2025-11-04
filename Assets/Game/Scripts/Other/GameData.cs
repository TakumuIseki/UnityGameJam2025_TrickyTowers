using UnityEngine;

/// <summary>
/// ゲームデータ
/// </summary>
public class GameData : MonoBehaviour
{
    /// <summary>
    /// インスタンス
    /// </summary>
    public static GameData Instance { get; private set; }

    /// <summary>
    /// 参加プレイヤー数
    /// </summary>
    public int JoinPlayerCount { get; set; } = 4;

    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        // シングルトン設定
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
