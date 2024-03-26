using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singletone<T> : MonoBehaviour where T : MonoBehaviour //½Ì±ÛÅæ ¸¸µå´Â Å¬·¡½º
{
    public static T instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = (T)FindObjectOfType(typeof(T));

            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                instance = obj.GetComponent<T>();
            }
        }
        DontDestroyOnLoad(this.transform.root.gameObject);        
    }
}
