using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singletone<T> : MonoBehaviour where T : MonoBehaviour //�̱��� ����� Ŭ����
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
