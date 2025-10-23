/// <summary>
/// インゲームの進行を管理するスクリプト。
/// </summary>
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private float widthPerPlayer_ = 0f;                                 // プレイヤー1人当たりのウィンドウ幅サイズ。
    private GameObject[] playerUnits_;                                  // プレイヤーユニットを格納。

    /// <summary>
    /// 他のプレイヤーユニットを取得して各スキルマネージャーに渡す。
    /// </summary>
    private void SetOtherPlayerUnitsToEachSkillManager()
    {
        // 全てのプレイヤーユニットで順に処理。
        foreach (var unit in playerUnits_)
        {
            // スキルマネージャーを取得。
            var skillManager = unit.GetComponent<SkillManagerComponent>();

            // スキルマネージャーが見つからない場合は警告。
            if (skillManager == null)
            {
                Debug.LogError($"SkillManagerComponentが見つかりません。");
                continue;
            }

            // 他のプレイヤーユニットを格納する変数を用意。
            var others = new System.Collections.Generic.List<GameObject>();

            // 全てのプレイヤーユニットで順に処理。
            foreach (var otherUnit in playerUnits_)
            {
                // 自分以外のプレイヤーユニットを変数に格納する。
                if (otherUnit != unit)
                {
                    others.Add(otherUnit);
                }
            }

            // 取得した他のプレイヤーユニットをスキルマネージャーに渡す。
            skillManager.SetOtherPlayerUnits(others.ToArray());
        }
    }
}
