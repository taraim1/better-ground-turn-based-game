using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageShow_dragImg : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;
    }
}
