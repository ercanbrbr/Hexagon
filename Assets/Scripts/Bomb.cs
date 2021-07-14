using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bomb : MonoBehaviour
{
    int bombCounter = 1; //Bir eksiği ile başlıyor.


    private void Start()
    {
        text();
    }
    public void countDown()
    {
        bombCounter--;
        text();
        if (bombCounter <= 0)
        {
            GameObject.Find("GameController").GetComponent<GameController>().gameOver();
        }
    }
    void text()
    {
        transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = bombCounter.ToString();
    }
}
