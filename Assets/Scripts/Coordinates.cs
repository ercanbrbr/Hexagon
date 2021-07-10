using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates : MonoBehaviour
{
    public int[,] odd = new int[,] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, -1 }, { -1, 0 }, { -1, 1 } };
    public int[,] even = new int[,] { { 0, 1 }, { 1, 1 }, { 1, 0 }, { 1, -1 }, { 0, -1 }, { -1, 0 } };
}
