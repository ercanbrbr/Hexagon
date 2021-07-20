using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEvents : MonoBehaviour
{
    Vector2 mousePosLeft; /*Mouse sol tık pozisyonu, mobil cihazlar için normal tıklama.*/
    Vector2 mousePosRight;  /*Mouse sağ tık pozisyonu, mobil cihazlar için iki parmakla tıklama.*/
    GameController _gameController;

    private void Awake()
    {
        _gameController = GetComponent<GameController>(); ;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _gameController.locked == false) /*Mouse sol tık basıldığında ve yapılan bir işlem yoksa.*/
        {
            mousePosLeft = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0) && _gameController.locked == false) /*Mouse sol tık bırakıldığında ve yapılan bir işlem yoksa.*/
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(mousePosLeft == mousePos) /*Tıklama ve bırakma pozisyonun aynı ise hexagon seçimi yapar.*/
                _gameController.selectHexes(mousePosLeft);
            else if(mousePosLeft.y>mousePos.y) /*Pozisyona göre seçili hexagonları çevirir.*/
            {
                StartCoroutine(_gameController.spinHexes(true));
            }
            else if (mousePosLeft.y < mousePos.y) /*Pozisyona göre seçili hexagonları çevirir.*/
            {
                StartCoroutine(_gameController.spinHexes(false));
            }
        }
        if (Input.GetMouseButtonDown(1) && _gameController.locked == false) /*Mouse sol tık basıldığında ve yapılan işlem yoksa. Hexagon seçimini sıfırlar.*/
        {
            for (int i = 0; i < 3; i++)
            {
                _gameController.selected[i] = -1;
            }
            _gameController.outlineObject();
            mousePosRight = Input.mousePosition;
        }
        if (!Input.GetMouseButton(1)) return;
        if (_gameController.locked == false) /*Yapılan işlem yoksa kamerayı kaydır.*/
        {
            GetComponent<CameraScript>().moveCam(mousePosRight);
        }
    }

}
