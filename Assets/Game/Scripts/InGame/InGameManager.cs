/// <summary>
/// インゲームの進行を管理するスクリプト。
/// </summary>
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [Header("PlayerUnitのプレハブ"), SerializeField]
    private GameObject playerUnitPrefab_;                               // 複製するプレイヤーユニットのプレハブ。

    // NOTO:takebayashi コントローラーの入力に応じて変化するよう修正予定。
    [Header("プレイヤーの人数"), SerializeField]
    private int playerTotalCount_ = 2;

    private float widthPerPlayer_ = 0f;                                 // プレイヤー1人当たりのウィンドウ幅サイズ。
    private GameObject[] playerUnits_;                                  // プレイヤーユニットを格納。

    void Start()
    {
        // プレイヤー1人当たりのウィンドウ幅を設定。
        widthPerPlayer_ = Screen.width / playerTotalCount_;

        SpawnPlayerUnits(playerTotalCount_);
    }

    /// <summary>
    /// 人数分プレイヤーユニットを複製。
    /// </summary>
    /// <param name="count">プレイヤーの人数。</param>
    private void SpawnPlayerUnits(int count)
    {
        // playerUnit_の空オブジェクトを生成。
        playerUnits_ = new GameObject[count];

        // プレイヤーの人数分処理。
        for (int num = 0; num < count; num++)
        {
            // プレイヤーユニットを生成。
            GameObject unit = Instantiate(playerUnitPrefab_);

            // 名前に番号を付与。
            unit.name = "PlayerUnit" + (num + 1);

            // 位置を設定。
            unit.transform.position = new Vector2(GetFirstPlayerWindowCenterX() + num * widthPerPlayer_, GetWindowCenterY());

            // PlayerUnitとその子にPlayerTypeをセットする。
            var id = unit.GetComponentsInChildren<PlayerTypeComponent>();
            foreach (var comp in id)
            {
                comp.playerType = num + 1;
            }

            // playerUnits_に格納する。
            playerUnits_[num] = unit;
        }
    }

    /// <summary>
    /// 1人目のプレイヤーのウィンドウの中央位置（X座標）を取得。
    /// </summary>
    /// <returns></returns>
    private float GetFirstPlayerWindowCenterX()
    {
        float firstPlayerWindowCenterX = Screen.width / playerTotalCount_ - widthPerPlayer_ / 2;
        return firstPlayerWindowCenterX;
    }

    private float GetWindowCenterY()
    {
        float windowCenterY = Screen.height / 2;
        return windowCenterY;
    }
}
