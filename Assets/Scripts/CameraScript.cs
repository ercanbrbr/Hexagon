using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    Camera _mainCam;
    float minX;
    float minY;
    float maxX;
    float maxY;
    public float panSpeed = 0.5f; /*Kamera hareket hızı.*/
    


    /*Başlamadan önce grid boyutuna göre kamera boyutunu ayarlıyor.*/
    private void Awake()
    {
        if (GetComponent<GameController>().width>=10)
            _mainCam.orthographicSize = 10;
        else if(GetComponent<GameController>().width <= 5)
            _mainCam.orthographicSize = 5;
        else
            _mainCam.orthographicSize = GetComponent<GameController>().width-1;
    }
    private void Start()
    {
        StartCoroutine(getValues());
    }

    /*Grid boyutuna göre kameranın sınırlarını belirliyor.*/
    IEnumerator getValues()
    {
        GameController _gameController = GetComponent<GameController>();
        yield return new WaitForSeconds(0.5f);
        minX = _gameController.grid[0, 0].transform.position.x + 4;
        maxX = _gameController.grid[0, GetComponent<GameController>().width - 1].transform.position.x - 4;
        minY = _gameController.grid[0, 0].transform.position.y + 8;
        maxY = _gameController.grid[GetComponent<GameController>().height - 1, 0].transform.position.y - 5;
        yield return null;
    }

    /*Kamerayı sınırlar içerisinde hareket ettiriyor.*/
    public void moveCam(Vector3 firstPos)
    {
        Vector3 pos = _mainCam.ScreenToViewportPoint(Input.mousePosition - firstPos);
        if (pos.x < 0 && _mainCam.transform.position.x <= minX)
        {
            pos.x = 0;
        }
        if (pos.x > 0 && _mainCam.transform.position.x >= maxX)
        {
            pos.x = 0;
        }
        if (pos.y < 0 && _mainCam.transform.position.y <= minY)
        {
            pos.y = 0;
        }
        if (pos.y > 0 && _mainCam.transform.position.y >= maxY)
        {
            pos.y = 0;
        }

        Vector3 move = new Vector3(pos.x * panSpeed, pos.y * panSpeed, 0);
        _mainCam.transform.Translate(move, Space.World);
    }
}
