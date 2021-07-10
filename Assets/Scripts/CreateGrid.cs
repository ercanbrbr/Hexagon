using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    [SerializeField]
    GameObject hexPref;
    public void createGrid()
    {
        GameObject temp = GameObject.Find("GameController");
        int height= temp.GetComponent<GameController>().height;
        int width= temp.GetComponent<GameController>().width;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject temp1 = Instantiate(hexPref, new Vector3((j * 0.75f)-(width/2)*0.75f, (i - ((j % 2)) * 0.5f)-(height/2), 0), Quaternion.Euler(new Vector3(0, 0, 90)));
                temp1.transform.SetParent(this.transform);
                temp1.GetComponent<Renderer>().material.SetColor("_Color", randomColor());
                temp.GetComponent<GameController>().grid[i, j] = temp1;
            }
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
