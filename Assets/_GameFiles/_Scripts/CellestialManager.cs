using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellestialManager : MonoBehaviour
{
    public static List<Gravity> masses;
    public static bool movingObj;

    void Update()
    {
        if (movingObj)
        {
            // Display placeable regions

        }
    }

    public static Vector3 GetMousePos() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.transform.position.y;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        return mousePos;
    }

}