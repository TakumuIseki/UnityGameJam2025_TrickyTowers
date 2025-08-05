/// <summary>
/// カウントダウンを管理するスクリプト。
/// </summary>
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countText_;     // テキスト表示する。
    private float countDown_ = 4f;          // 3,2,1,GO!の合算遷移時間。
    private int count_ = 0;                 // float型のcountDown_を代入し、整数で表示する。

    void Start()
    {
        countText_ = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        ShowText();
    }

    /// <summary>
    /// カウントダウンのテキスト遷移設定。
    /// </summary>
    private void ShowText()
    {
        countDown_ -= Time.deltaTime;

        // 3,2,1を表示する。
        if (countDown_ >= 1)
        {
            count_ = (int)countDown_;
            countText_.text = count_.ToString();
        }

        // カウントダウンが残り1秒になったら、「GO!」と表示する。
        if (countDown_ < 1)
        {
            countText_.text = "GO!";
        }

        if (countDown_ < 0)
        {
            // カウントダウンが終わったら自身を消滅させる。
            Destroy(gameObject);

            ActiveGame();
        }
    }

    /// <summary>
    /// ゲームを開始するメソッドを呼び出す。
    /// </summary>
    private void ActiveGame()
    {
        //カウントダウンが終わったらゲームを開始する。
    }
}
