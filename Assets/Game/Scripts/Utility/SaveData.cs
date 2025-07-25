using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;




using ResultData = RankingData; // 結果データも同じ構造体を使用


/// <summary>
/// ランキングデータ
/// </summary>
public struct RankingData
{
    public uint height; // 高さ
    public string name; // 名前
}




/// <summary>
/// セーブデータ
/// </summary>
public class SaveData
{
    private bool isRequestSave_ = false; // セーブリクエストフラグ




    /**
     * セーブデータファイルパス関連
     */
    private string saveFilePath_ = "saveData.json";
    public string SaveFilePath
    {
        private get { return saveFilePath_; }
        set { saveFilePath_ = value; }
    }




    /**
     * ランキングデータ関連
     */
    // ランキングデータ
    private List<ResultData> resultDatas_ = new List<ResultData>();
    private List<RankingData> rankingDatas = new List<RankingData>();
    /// <summary>
    /// ランキングデータ追加
    /// </summary>
    public void AddRankingData(RankingData data)
    {
        rankingDatas.Add(data);
    }
    /// <summary>
    /// ランキングデータ取得
    /// </summary>
    public List<RankingData> GetRankingDatas()
    {
        return rankingDatas;
    }
    /// <summary>
    /// 現在プレイ中のユーザープレイ結果データ
    /// NOTE: ランキングデータと同じ構造体を使用
    /// </summary>
    public void SetCurrentResultDatas(List<ResultData> datas)
    {
        resultDatas_ = datas;
    }
    public List<ResultData> GetCurrentResultDatas()
    {
        return resultDatas_; 
    }




    /// <summary>
    /// コンストラクタ
    /// </summary>
    private SaveData()
    {
        Desrialize();
    }


    /// <summary>
    /// 更新
    /// 毎フレーム呼ばれる
    /// </summary>
    public void Update()
    {
        if (isRequestSave_)
        {
            Debug.Log("セーブリクエストがありました。セーブを実行します。");
            SaveCore();
            isRequestSave_ = false; // セーブリクエストフラグをリセット
        }
    }


    /// <summary>
    /// セーブ処理本体
    /// </summary>
    private void SaveCore()
    {
        Serialize(); // シリアライズ処理
    }


    /// <summary>
    /// シリアライズ処理
    /// NOTE: セーブ時に呼ばれます
    ///       データをファイルに保存するための形式に変換します
    /// </summary>
    private void Serialize()
    {
        string path = SaveFilePath;
        // List<RankingData>をJSON形式の文字列に変換
        string json = JsonConvert.SerializeObject(rankingDatas, Formatting.Indented);
        // ファイルに書き込む
        File.WriteAllText(path, json);
        Debug.Log($"ランキングデータを保存しました!");
    }


    /// <summary>
    /// デシリアライズ処理
    /// NOTE: 読み込み時に呼ばれます
    ///       データをファイルから読み込み、内部のデータ構造に変換します
    /// </summary>
    private void Desrialize()
    {
        string path = SaveFilePath;
        // セーブファイルが存在する場合のみ読み込む
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            // JSON形式の文字列をList<RankingData>に変換
            rankingDatas = JsonConvert.DeserializeObject<List<RankingData>>(json);
            Debug.Log($"ランキングデータ読み込み完了!");
        }
        else
        {
            Debug.Log("セーブファイルが見つかりません。新しいランキングリストで開始します。");
            rankingDatas = new List<RankingData>();
        }
    }


    /// <summary>
    /// セーブリクエスト
    /// </summary>
    public void RequestSave()
    {
        Debug.Log("セーブリクエストを受け付けました。");
        isRequestSave_ = true; // セーブリクエストフラグを立てる
    }




    /**
     * シングルトン用
     */
    private static SaveData instance_ = null;

    /// <summary>
    /// インスタンスの生成をします。必ず読んでください。
    /// </summary>
    public static void Create()
    {
        if (instance_ == null)
        {
            Debug.Log("SaveDataのインスタンスを生成します。");
            instance_ = new SaveData();
        }
    }

    /// <summary>
    /// インスタンス取得
    /// </summary>
    public static SaveData Get()
    {
        return instance_;
    }

    /// <summary>
    /// インスタンスの解放をします。終了時に必ず読んでください。
    /// </summary>
    public static void Destroy()
    {
        instance_ = null;
    }
}
