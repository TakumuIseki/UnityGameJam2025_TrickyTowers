/// <summary>
/// カウントダウンを管理するスクリプト。
/// </summary>
using TMPro;
using UnityEngine;

public class CountDownComponent : MonoBehaviour
{
    [Header("カウントダウンテキスト"),SerializeField]
    private TextMeshProUGUI countDownText_;

    private float countDownTimer_ = 4f;         // 3,2,1,GO!の合算遷移時間。

    private int currentCountDownValue_ = 0;     // float型のcountDown_を代入し、整数で表示する。

    /// <summary>
    /// Update
    /// </summary>
    void Update()
    {
        CoundDown();
    }

    /// <summary>
    /// カウントダウン
    /// </summary>
    private void CoundDown()
    {
        countDownTimer_ -= Time.deltaTime;

        if (countDownTimer_ >= 1)
        {
            currentCountDownValue_ = (int)countDownTimer_;
            countDownText_.text = currentCountDownValue_.ToString();
        }
        if (countDownTimer_ < 1)
        {
            countDownText_.text = "スタート！";
        }
        if (countDownTimer_ < 0)
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
