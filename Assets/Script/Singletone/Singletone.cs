using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singletone<T> : MonoBehaviour where T : MonoBehaviour //싱글톤 만드는 클래스
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
        else 
        {
            //인스턴스 존재시 게임오브젝트 파괴
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.transform.root.gameObject);        
    }
}
