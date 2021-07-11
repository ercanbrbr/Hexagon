using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    Camera mainCam;
    Vector3 firstPos;
    public float panSpeed = 0.5f;
    private void Awake()
    {
        if(GetComponent<GameController>().width>=10)
            mainCam.GetComponent<Camera>().orthographicSize = 10;
        else if(GetComponent<GameController>().width <= 5)
            mainCam.GetComponent<Camera>().orthographicSize = 5;
        else
            mainCam.GetComponent<Camera>().orthographicSize = GetComponent<GameController>().width-1;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            firstPos = Input.mousePosition;
            print(firstPos);
        }
        if (!Input.GetMouseButton(1)) return;

        Vector3 pos = mainCam.ScreenToViewportPoint(Input.mousePosition - firstPos);
        print(pos);
        Vector3 move = new Vector3(pos.x * panSpeed, pos.y * panSpeed,0);

        mainCam.transform.Translate(move, Space.World);
    }
}
