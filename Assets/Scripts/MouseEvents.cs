using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEvents : MonoBehaviour
{
    Vector2 mousePos;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(mousePos==mousePos2)
                GetComponent<GameController>().selectHexes(mousePos);
            else
            {
                StartCoroutine(GetComponent<GameController>().spinHexes());
            }
        }
    }
    
}
