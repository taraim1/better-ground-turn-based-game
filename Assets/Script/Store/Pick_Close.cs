using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Pick_Close : MonoBehaviour
{
    GameObject Scene_Changer;
    
    public UnityEvent Pick_Screen_Closed;
    public void Change()
    {
        Scene_Changer.GetComponent<Scene_Change>().Scene_Active(0);
    }
    // Start is called before the first frame update
    void Start()
    {
        Scene_Changer = GameObject.Find("Scene");
    }
    public void Close()
    {
        Pick_Screen_Closed.Invoke();
    }


}
