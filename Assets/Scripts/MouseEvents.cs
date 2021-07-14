using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEvents : MonoBehaviour
{
    Vector2 mousePosLeft; /*Mouse sol tık pozisyonu, mobil cihazlar için normal tıklama.*/
    Vector2 mousePosRight;  /*Mouse sağ tık pozisyonu, mobil cihazlar için iki parmakla tıklama.*/

    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && GetComponent<GameController>().locked == false) /*Mouse sol tık basıldığında ve yapılan bir işlem yoksa.*/
        {
            mousePosLeft = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0) && GetComponent<GameController>().locked == false) /*Mouse sol tık bırakıldığında ve yapılan bir işlem yoksa.*/
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(mousePosLeft == mousePos) /*Tıklama ve bırakma pozisyonun aynı ise hexagon seçimi yapar.*/
                GetComponent<GameController>().selectHexes(mousePosLeft);
            else if(mousePosLeft.y>mousePos.y) /*Pozisyona göre seçili hexagonları çevirir.*/
            {
                StartCoroutine(GetComponent<GameController>().spinHexes(true));
            }
            else if (mousePosLeft.y < mousePos.y) /*Pozisyona göre seçili hexagonları çevirir.*/
            {
                StartCoroutine(GetComponent<GameController>().spinHexes(false));
            }
        }
        if (Input.GetMouseButtonDown(1) && GetComponent<GameController>().locked == false) /*Mouse sol tık basıldığında ve yapılan işlem yoksa. Hexagon seçimini sıfırlar.*/
        {
            for (int i = 0; i < 3; i++)
            {
                GetComponent<GameController>().selected[i] = -1;
            }
            GetComponent<GameController>().outlineObject();
            mousePosRight = Input.mousePosition;
        }
        if (!Input.GetMouseButton(1)) return;
        if (GetComponent<GameController>().locked == false) /*Yapılan işlem yoksa kamerayı kaydır.*/
        {
            GetComponent<CameraScript>().moveCam(mousePosRight);
        }
    }

}
