using System.Collections.Generic;
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
    /// 勝利したプレイヤー番号リスト
    /// </summary>
    public List<int> WinPlayerNumList { get; private set; } = new List<int>();

    /// <summary>
    /// 制限時間（秒）
    /// </summary>
    public int LimitTime { get; set; }

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

    /// <summary>
    /// 勝利したプレイヤー番号を追加
    /// </summary>
    public void AddWinPlayerNum(int playerNum)
    {
        if(!WinPlayerNumList.Contains(playerNum))
        {
            WinPlayerNumList.Add(playerNum);
        }
    }

    /// <summary>
    /// 勝利したプレイヤー番号リストから指定の番号を削除
    /// </summary>
    public void RemoveWinPlayerNum(int playerNum)
    {
        if(WinPlayerNumList.Contains(playerNum))
        {
            WinPlayerNumList.Remove(playerNum);
        }
    }
}
