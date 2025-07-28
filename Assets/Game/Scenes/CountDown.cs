using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    public TextMeshProUGUI countText;
    float countDown = 4f;
    int count = 4;

    // Start is called before the first frame update
    void Start()
    {
        countText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        ShowText();
    }

    void ShowText() 
    {
        if (countDown >= 1)
        {
            countDown -= Time.deltaTime;
            count = (int)countDown;
            countText.text = count.ToString();
        }
        if (countDown <= 1)
        {
            countText.text = "GO!";

        }
    }

    void StartGame() {
        //カウントダウンが終わったらゲームを開始する。
    }
}
