using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    [SerializeField]
    GameObject hexPref;
    [SerializeField]
    GameObject hexPrefBomb;
    int height;
    int width;
    bool firstCreation = true;

    /*Grid boyutuna göre hexleri yaratıyor.*/
    public void createHexes()
    {
        GameObject gameController = GameObject.Find("GameController");
        height = gameController.GetComponent<GameController>().height;
        width = gameController.GetComponent<GameController>().width;
        
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (gameController.GetComponent<GameController>().grid[i, j]==null)
                {
                    GameObject hex;
                    if (gameController.GetComponent<GameController>().bombCounter <= 0)
                        hex = Instantiate(hexPrefBomb, new Vector3(0, 100, 0), Quaternion.Euler(new Vector3(0, 0, 90)));
                    else
                        hex = Instantiate(hexPref, new Vector3(0, 100, 0), Quaternion.Euler(new Vector3(0, 0, 90)));
                    hex.transform.SetParent(this.transform);
                    gameController.GetComponent<GameController>().grid[i, j] = hex;
                    do
                    {
                        if (gameController.GetComponent<GameController>().bombCounter <= 0)
                        {
                            hex.AddComponent<Bomb>();
                            gameController.GetComponent<GameController>().bombCounter += 1000;
                        }
                        hex.GetComponent<Renderer>().material.SetColor("_Color", randomColor());
                    } while (gameController.GetComponent<GameController>().checkPattern(false) && firstCreation==true);
                }
            }
        }
        firstCreation = false;
        moveHexes();
    }
    /*Arraydeki hexleri, indexi ile beraber setpositiona yollar.*/
    public void moveHexes()
    {
        GameObject gameController = GameObject.Find("GameController");
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if(gameController.GetComponent<GameController>().grid[i, j]!=null)
                    StartCoroutine(setPosition(gameController.GetComponent<GameController>().grid[i,j], i, j));
            }
        }
    }
    /*Gelen objenin pozisyonunu, gelen x ve y değerlerine göre belirleyip. O pozisyona götürüyor.*/
    IEnumerator setPosition(GameObject obj, int x, int y)
    {
        float time = 0;
        float duration = 0.3f;
        Vector3 position = new Vector3((y * 0.75f) - (width / 2) * 0.75f, (x - ((y % 2)) * 0.5f) - (height / 2), 0);
        while (time<duration)
        {
            try
            {
                obj.transform.position = Vector3.Lerp(obj.transform.position, position, time / duration);
                time += Time.deltaTime;
            }
            catch { }
            yield return null;
        }
    }
    /*Seçili renkler arasından rastgele renk döndürür.*/
    Color randomColor()
    {
        GameObject temp = GameObject.Find("GameController");
        string color = temp.GetComponent<GameController>().hexColors[Random.Range(0, temp.GetComponent<GameController>().hexColors.Length)].ToString();
        if (color == "Red")
        {
            return Color.red;
        }
        else if (color == "Blue")
        {
            return Color.blue;
        }
        else if (color == "Green")
        {
            return Color.green;
        }
        else if (color == "Black")
        {
            return Color.black;
        }
        else if (color == "White")
        {
            return Color.white;
        }
        else if (color == "Yellow")
        {
            return Color.yellow;
        }
        else if (color == "Orange")
        {
            return new Color32(255, 114, 0, 255);
        }
        else
            return Color.gray;
    }
}