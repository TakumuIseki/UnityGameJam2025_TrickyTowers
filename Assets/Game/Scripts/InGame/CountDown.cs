/// <summary>
/// カウントダウンを管理するスクリプト。
/// </summary>
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countText_;
    private float countDown_ = 4f;
    private int count_ = 4;

    void Start()
    {
        countText_ = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        ShowText();
    }

    private void ShowText()
    {
        countDown_ -= Time.deltaTime;

        if (countDown_ >= 1)
        {
            count_ = (int)countDown_;
            countText_.text = count_.ToString();
        }

        if (countDown_ < 1)
        {
            countText_.text = "GO!";
        }

        if (countDown_ < 0)
        {
            //countText_.gameObject.SetActive(false); // カウントダウンが終わったらテキストを非表示にする。
            Destroy(gameObject);
            ActiveGame(); // ゲームを開始するメソッドを呼び出す。
        }
    }

    private void ActiveGame()
    {
        //カウントダウンが終わったらゲームを開始する。
    }
}
