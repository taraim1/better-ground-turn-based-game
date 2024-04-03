using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singletone<T> : MonoBehaviour where T : MonoBehaviour //½Ì±ÛÅæ ¸¸µå´Â Å¬·¡½º
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    private void Awake()
    {
            DontDestroyOnLoad(this.transform.root.gameObject);        
    }
}
