/// <summary>
/// プレイヤーユニット内のオブジェクトの配置を管理するスクリプト。
/// </summary>
using UnityEngine;

public class PlayerUnitLayoutManager : MonoBehaviour
{
    [System.Serializable]
    private struct ChildPositionSetting
    {
        public string childName;        // 子オブジェクト名
        public Vector2 localPosition;   // 設定するローカル座標
    }

    [Header("子オブジェクトの名前と位置")]
    [SerializeField] private ChildPositionSetting[] positionSettings;

    private void Start()
    {
        SetupChildPositions();
    }

    /// <summary>
    /// 子のオブジェクトのポジションを設定する処理。
    /// </summary>
    private void SetupChildPositions()
    {
        // positionSettingsのelementsを順に処理。
        foreach (var setting in positionSettings)
        {
            // オブジェクト名で探す。
            var childObject = transform.Find(setting.childName);

            // 見つからない場合は警告する。
            if (childObject == null)
            {
                Debug.LogError($"{setting.childName} が見つかりません。名前が変更されている可能性があります。");
                continue;
            }

            // 予め設定したポジションをオブジェクトに代入する。
            childObject.localPosition = setting.localPosition;
        }
    }
}
