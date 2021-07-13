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
    [SerializeField]
    int score = 0;
    public int[] selected;
    public bool locked = false;

    public void Start()
    {
        selected = new int[3] { 0, 0, 0 };
        grid = new GameObject[height, width];
        GameObject.Find("Tilemap").GetComponent<CreateGrid>().createHexes();

        //StartCoroutine(test());
    }
    void correctGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (grid[j,i]==null)
                {
                    for (int k = j+1; k < height; k++)
                    {
                        if (grid[k,i]!=null)
                        {
                            grid[j, i] = grid[k, i];
                            grid[k, i] = null;
                            break;
                        }
                    }
                }
            }
        }
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
            score += 5;
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
                            break;
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
                            break;
                        }
                    }
                    catch { }
                }
            }
        }
        catch { }
        return temp;
    }
    public void selectHexes(Vector2 mousePos)
    {
        float[] closest = { 100f,100f,100f};
        Vector2 avg;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                for (int q = 0; q < 2; q++)
                {
                    if (Vector2.Distance(mousePos, grid[i, j].transform.position) < closest[q])
                    {
                        closest[q]= Vector2.Distance(mousePos, grid[i, j].transform.position);
                        selected[q] = (width * i) + j;
                        break;
                    }
                }
            }
        }
        avg = (grid[selected[0]/width, selected[0] % width].transform.position + grid[selected[1] / width, selected[1] % width].transform.position)/2;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (Vector2.Distance(avg, grid[i, j].transform.position) < closest[2])
                {
                    if (grid[i,j]== grid[selected[0] / width, selected[0] % width] || grid[i, j] == grid[selected[1] / width, selected[1] % width])
                    {
                        continue;
                    }
                    closest[2] = Vector2.Distance(avg, grid[i, j].transform.position);
                    selected[2] = (width * i) + j;
                }
            }
        }
        outlineObject();
    }
    public void outlineObject()
    {
        foreach (var item in grid)
        {
            item.GetComponent<cakeslice.Outline>().eraseRenderer = true;
        }
        foreach (var index in selected)
        {
            if (index < 0)
                break;
            grid[index/width, index%width].GetComponent<cakeslice.Outline>().eraseRenderer = false;
        }
    }
    public IEnumerator spinHexes()
    {
        foreach (var item in selected)
        {
            if (item < 0)
                yield break;
        }
        GameObject temp;
        locked = true;
        for (int i = 0; i < 3; i++)
        {
            temp = grid[selected[0] / width, selected[0] % width];
            grid[selected[0] / width, selected[0] % width] = grid[selected[1] / width, selected[1] % width];
            grid[selected[1] / width, selected[1] % width] = grid[selected[2] / width, selected[2] % width];
            grid[selected[2] / width, selected[2] % width] = temp;
            
            GameObject.Find("Tilemap").GetComponent<CreateGrid>().moveHexes();
            yield return new WaitForSeconds(0.4f);
        }
        locked = false;
        yield return null;
    }
    IEnumerator test()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.5f);
            checkPattern();
            correctGrid();
            yield return new WaitForSeconds(0.25f);
            GameObject.Find("Tilemap").GetComponent<CreateGrid>().moveHexes();
            yield return new WaitForSeconds(0.25f);
            GameObject.Find("Tilemap").GetComponent<CreateGrid>().createHexes();
        }
    }
}
