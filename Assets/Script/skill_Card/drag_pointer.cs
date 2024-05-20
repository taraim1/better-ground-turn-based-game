using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drag_pointer : MonoBehaviour
{
    public Cards cards;


    void Update()
    {
        if (Input.GetMouseButtonUp(0)) 
        { 
            Destroy(gameObject);
        }

        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = new Vector3(mousepos.x, mousepos.y, transform.position.z);
    }
}
