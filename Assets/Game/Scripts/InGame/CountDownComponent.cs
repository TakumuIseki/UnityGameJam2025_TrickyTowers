/// <summary>
/// カウントダウンを管理するスクリプト。
/// </summary>
using TMPro;
using UnityEngine;

public class CountDownComponent : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countDownText_;     // テキスト表示する。
    private float countDownTimer_ = 4f;         // 3,2,1,GO!の合算遷移時間。
    private int currentCountDownValue_ = 0;     // float型のcountDown_を代入し、整数で表示する。

    void Start()
    {
        countDownText_ = GetComponent<TextMeshProUGUI>();
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
        countDownTimer_ -= Time.deltaTime;

        // 3,2,1を表示する。
        if (countDownTimer_ >= 1)
        {
            currentCountDownValue_ = (int)countDownTimer_;
            countDownText_.text = currentCountDownValue_.ToString();
        }

        // カウントダウンが残り1秒になったら、「GO!」と表示する。
        if (countDownTimer_ < 1)
        {
            countDownText_.text = "GO!";
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
