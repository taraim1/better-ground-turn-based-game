using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

   
    void Start()
    {

        PartyManager.instance.read_Json_file();
    }

}


