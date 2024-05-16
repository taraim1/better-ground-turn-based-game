using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int Scenme = 0;
    [SerializeField]
    private GameObject Manager;
    public void Change()
    {
        Manager.GetComponent<Scene_Change>().Scene_Active(Scenme);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
