using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    private TextMeshProUGUI countText_;
    private float countDown_ = 4f;
    private int count_ = 4;

    // Start is called before the first frame update
    void Start()
    {
        countText_ = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowText();
        StartGame();
    }

    void ShowText() 
    {
        if (countDown_ >= 1)
        {
            countDown_ -= Time.deltaTime;
            count_ = (int)countDown_;
            countText_.text = count_.ToString();
        }
        if (countDown_ <= 1)
        {
            countText_.text = "GO!";

        }
    }

    void StartGame() {
        //カウントダウンが終わったらゲームを開始する。
    }
}
