using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// セーブシステム
/// </summary>
public class SaveSystem : MonoBehaviour
{
    // セーブデータを保存するファイルパス
    // 基本変えることはないはずだが、変更が必要になったらプロパティ追加する
    [SerializeField]
    private string saveFilePath = "saveData.json";

    void Awake()
    {
        SaveData.Create();
        SaveData.Get().SaveFilePath = saveFilePath;
        SaveData.Get().RequestSave();
    }

    void Start()
    {
    }

    void Update()
    {
        SaveData.Get().Update();
    }
    
    void OnDestroy()
    {
        SaveData.Destroy();
    }
}
