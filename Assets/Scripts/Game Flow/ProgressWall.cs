using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressWall : MonoBehaviour
{
    public bool condition;

    public GameObject[] beforeWalls;
    public GameObject[] afterWalls;

    void ToggleWalls()
    {
        condition = !condition;
        foreach(GameObject wall in beforeWalls)
        {
            wall.SetActive(condition);
        }
        foreach(GameObject wall in afterWalls)
        {
            wall.SetActive(!condition);
        }
    }
}
