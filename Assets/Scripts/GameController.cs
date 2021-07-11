using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum Colors {Red,Blue,Orange,Green,Yellow,White,Black,Gray }; 
    [SerializeField]
    public int height = 8;
    [SerializeField]
    public int width = 9;
    [SerializeField]
    public Colors[] hexColors;
    public GameObject[,] grid;
    int score = 0;

    public void Start()
    {
        grid = new GameObject[height, width];
        GameObject.Find("Tilemap").GetComponent<CreateGrid>().createGrid();
        //StartCoroutine(test());
    }

    void checkPattern()
    {
        List<GameObject> temp = new List<GameObject>();
        List<GameObject> temp1 = new List<GameObject>();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                temp1 = pattern(i, j);
                foreach (var item in temp1)
                {
                    temp.Add(item);
                }
            }
        }
        foreach (var item in temp)
        {
            for (int i = 0; i < grid.Length; i++)
            {
                if (grid[i / width, i % width] == item)
                {
                    grid[i / width, i % width] = null;
                    break;
                }
            }
            Destroy(item);
        }
    }

    List<GameObject> pattern(int i, int j)
    {
        List<GameObject> temp = new List<GameObject>();
        try
        {
            if (j%2==0)
            {
                for (int x = 0; x < GetComponent<Coordinates>().even.GetLength(0); x++)
                {
                    int[] tempCoordinate1 = { GetComponent<Coordinates>().even[x, 0], GetComponent<Coordinates>().even[x, 1] };
                    int[] tempCoordinate2 = { GetComponent<Coordinates>().even[(x + 1)% GetComponent<Coordinates>().even.GetLength(0), 0], GetComponent<Coordinates>().even[(x + 1)% GetComponent<Coordinates>().even.GetLength(0), 1] };
                    try
                    {
                        if (grid[i, j].GetComponent<Renderer>().material.color == grid[i + tempCoordinate1[0], j + tempCoordinate1[1]].GetComponent<Renderer>().material.color && grid[i, j].GetComponent<Renderer>().material.color == grid[i + tempCoordinate2[0], j + tempCoordinate2[1]].GetComponent<Renderer>().material.color)
                        {
                            temp.Add(grid[i, j]);
                        }
                    }
                    catch { }
                }
            }
            else
            {
                for (int x = 0; x < GetComponent<Coordinates>().odd.Length; x++)
                {
                    int[] tempCoordinate1 = { GetComponent<Coordinates>().odd[x, 0], GetComponent<Coordinates>().odd[x, 1] };
                    int[] tempCoordinate2 = { GetComponent<Coordinates>().odd[(x + 1) % 6, 0], GetComponent<Coordinates>().odd[(x + 1) % 6, 1] };
                    try
                    {
                        if (grid[i, j].GetComponent<Renderer>().material.color == grid[i + tempCoordinate1[0], j + tempCoordinate1[1]].GetComponent<Renderer>().material.color && grid[i, j].GetComponent<Renderer>().material.color == grid[i + tempCoordinate2[0], j + tempCoordinate2[1]].GetComponent<Renderer>().material.color)
                        {
                            temp.Add(grid[i, j]);
                        }
                    }
                    catch { }
                }
            }
        }
        catch { }
        return temp;
    }
    IEnumerator test()
    {
        yield return new WaitForSeconds(1);
        checkPattern();
    }
}
