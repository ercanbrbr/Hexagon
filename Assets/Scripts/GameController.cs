using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    int highScore;
    [SerializeField]
    public int bombCounter = 200;
    public int[] selected;
    /*Birbiri ile çakışabilecek işlemleri engellemek için kilit. İşlem yapılırken true atanır. True iken diğer işlemler, yapılan işlemin bitmesini bekler.*/
    public bool locked = false;

    public void Start()
    {
        print(GetComponent<Coordinates>().even.GetLength(0));
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        selected = new int[3] { -1, -1, -1 };
        grid = new GameObject[height, width];
        GameObject.Find("Tilemap").GetComponent<CreateGrid>().createHexes();
    }
    /*Parçalar yok edildikten sonra oluşan boşlukları, yukarıdaki parçalar ile doldurur.*/
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
    /*Eğer desenlerin yok edilmesi isteniyorsa, yok ediyor. */
    public bool checkPattern(bool destroy)
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
        if (temp.Count == 0)
            return false;
        if (destroy == true)
        {
            if (FindObjectOfType<MainSoundManager>() != null)
                FindObjectOfType<MainSoundManager>().Play("pop");
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
                bombCounter -= 5;
            }
        }
        return true;
    }
    /*Oluşan desen varsa, desen oluşturan parçaları bir listede topluyor. Ve listeyi geri döndürüyor.*/
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
                for (int x = 0; x < GetComponent<Coordinates>().odd.GetLength(0); x++)
                {
                    int[] tempCoordinate1 = { GetComponent<Coordinates>().odd[x, 0], GetComponent<Coordinates>().odd[x, 1] };
                    int[] tempCoordinate2 = { GetComponent<Coordinates>().odd[(x + 1) % GetComponent<Coordinates>().even.GetLength(0), 0], GetComponent<Coordinates>().odd[(x + 1) % GetComponent<Coordinates>().even.GetLength(0), 1] };
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

    /*Mouseun tıklandığı andaki pozisyonu alır. En yakın hexagonları seçili hale getirir.*/
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
        Array.Sort(selected);
        /*Seçerken bazen aynı satırdan seçiyor. Seçimi iptal ediyoruz.*/
        if(selected[0]+1==selected[1] && selected[1] + 1 == selected[2])
        {
            return;
        }
        outlineObject();
    }
    /*Seçili objeleri belirtmek için etrafında bir dış çizgi oluşturur.*/
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
    /*Seçili olan hex grubunu, gelen bilgiye göre saat yönüne veya tersine döndürüyor. Bu işlem sırasında başka işlemlerin yapılmasını engellemek için locked değişkenine true atıyoruz. 
    Eğer seçili bir grup yoksa işlem gerçekleştirilmez.*/
    public IEnumerator spinHexes(bool clockwise)
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
            if (selected[0]/width==selected[1]/width && clockwise==true || selected[0] / width != selected[1] / width && clockwise == false) //Saat yönünün tersi.
            {
                temp = grid[selected[0] / width, selected[0] % width];
                grid[selected[0] / width, selected[0] % width] = grid[selected[1] / width, selected[1] % width];
                grid[selected[1] / width, selected[1] % width] = grid[selected[2] / width, selected[2] % width];
                grid[selected[2] / width, selected[2] % width] = temp;
            }
            else if(selected[0] / width != selected[1] / width && clockwise == true || selected[0] / width == selected[1] / width && clockwise == false) //Saat yönü.
            {
                temp = grid[selected[0] / width, selected[0] % width];
                grid[selected[0] / width, selected[0] % width] = grid[selected[2] / width, selected[2] % width];
                grid[selected[2] / width, selected[2] % width] = grid[selected[1] / width, selected[1] % width];
                grid[selected[1] / width, selected[1] % width] = temp;
            }
            
            GameObject.Find("Tilemap").GetComponent<CreateGrid>().moveHexes();
            yield return new WaitForSeconds(0.3f);
            while (checkPattern(true) == true)
            {
                correctGrid();
                yield return new WaitForSeconds(0.25f);
                GameObject.Find("Tilemap").GetComponent<CreateGrid>().moveHexes(); 
                yield return new WaitForSeconds(0.25f);
                GameObject.Find("Tilemap").GetComponent<CreateGrid>().createHexes();
                scoreUpdate();
                yield return new WaitForSeconds(0.25f);
                selected = new int[3] { -1, -1, -1 };
                if (checkPattern(false) == false)
                {
                    locked = false;
                    outlineObject();
                    foreach (var item in grid)
                    {
                        try
                        {
                            item.GetComponent<Bomb>().countDown();
                        }
                        catch{ }
                    }
                    checkPossibleCombo();
                    yield break;
                }
            }
        }
        locked = false;
        checkPossibleCombo();
        yield return null;
    }
    /*Yanyana aynı renkten var mı diye kontrol ediyoruz. Eğer yoksa yapılacak hamle kalmadığı için oyunu sonlandırıyoruz.*/
    void checkPossibleCombo()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (j%2==0)
                {
                    for (int x = 0; x < GetComponent<Coordinates>().even.Length; x++)
                    {
                        int[] tempCoordinate = { GetComponent<Coordinates>().even[x, 0], GetComponent<Coordinates>().odd[x, 1] };
                        try
                        {
                            if (grid[i, j].GetComponent<Renderer>().material.color == grid[i + tempCoordinate[0], j + tempCoordinate[1]].GetComponent<Renderer>().material.color)
                            {
                                return;
                            }
                        }
                        catch { }
                    }
                    
                }
                else
                {
                    for (int x = 0; x < GetComponent<Coordinates>().odd.Length; x++)
                    {
                        int[] tempCoordinate = { GetComponent<Coordinates>().odd[x, 0], GetComponent<Coordinates>().odd[x, 1] };
                        try
                        {
                            if (grid[i, j].GetComponent<Renderer>().material.color == grid[i + tempCoordinate[0], j + tempCoordinate[1]].GetComponent<Renderer>().material.color)
                            {
                                return;
                            }
                        }
                        catch { }
                    }
                }
            }
        }
        gameOver();
    }
    public void gameOver()
    {
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", Convert.ToInt32(score));
        }
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        Instantiate(Resources.Load("GameOver"));
        GameObject.Find("HighScore").GetComponent<Text>().text = "HighScore : "+ highScore.ToString();
        GameObject.Find("LastScore").GetComponent<Text>().text = score.ToString();
        enabled = false;
        GetComponent<MouseEvents>().enabled = false;
        GetComponent<CreateGrid>().enabled = false;
        GetComponent<GameController>().enabled = false;
    }
    public void scoreUpdate()
    {
        GameObject scoreBoard;
        scoreBoard = GameObject.Find("Score");
        scoreBoard.GetComponent<Text>().text = score.ToString();
    }
}
