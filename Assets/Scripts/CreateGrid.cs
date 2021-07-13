using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    [SerializeField]
    GameObject hexPref;
    int height;
    int width;
    public void createHexes()
    {
        GameObject temp = GameObject.Find("GameController");
        height = temp.GetComponent<GameController>().height;
        width = temp.GetComponent<GameController>().width;
        
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (temp.GetComponent<GameController>().grid[i, j]==null)
                {
                    //GameObject temp1 = Instantiate(hexPref, new Vector3((j * 0.75f)-(width/2)*0.75f, (i - ((j % 2)) * 0.5f)-(height/2), 0), Quaternion.Euler(new Vector3(0, 0, 90)));
                    GameObject temp1 = Instantiate(hexPref, new Vector3(0, 100, 0), Quaternion.Euler(new Vector3(0, 0, 90)));
                    //StartCoroutine(setPosition(temp1, i, j));
                    temp1.transform.SetParent(this.transform);
                    temp1.GetComponent<Renderer>().material.SetColor("_Color", randomColor());
                    temp.GetComponent<GameController>().grid[i, j] = temp1;
                }
            }
        }
        moveHexes();
    }
    public void moveHexes()
    {
        GameObject temp = GameObject.Find("GameController");
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if(temp.GetComponent<GameController>().grid[i, j]!=null)
                    StartCoroutine(setPosition(temp.GetComponent<GameController>().grid[i,j], i, j));
            }
        }
    }
    /*IEnumerator setPosition(GameObject obj, int x,int y)
    {

        //obj.transform.position = new Vector3((y * 0.75f) - (width / 2) * 0.75f, (x - ((y % 2)) * 0.5f) - (height / 2), 0);
        Vector3 temp = new Vector3((y * 0.75f) - (width / 2) * 0.75f, (x - ((y % 2)) * 0.5f) - (height / 2), 0);
        while (obj != null && temp != obj.transform.position)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, temp, 0.15f);
            yield return null;
        }
        yield return null;
    }*/

    IEnumerator setPosition(GameObject obj, int x, int y)
    {
        float time = 0;
        float duration = 0.5f;
        //obj.transform.position = new Vector3((y * 0.75f) - (width / 2) * 0.75f, (x - ((y % 2)) * 0.5f) - (height / 2), 0);
        Vector3 temp = new Vector3((y * 0.75f) - (width / 2) * 0.75f, (x - ((y % 2)) * 0.5f) - (height / 2), 0);
        while (time<duration)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, temp, time/duration);
            time += Time.deltaTime;
            yield return null;
        }
    }
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