using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class TimeSaveTest : MonoBehaviour 
{
    public string url = "";

    void Start ()
    {
        StartCoroutine(WebChk());
    }

    IEnumerator WebChk()
    {
        UnityWebRequest request = new UnityWebRequest();
        using(request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string date = request.GetResponseHeader("date");
                
                DateTime dateTime = DateTime.Parse(date);//.ToUniversalTime();
                TimeSpan timestamp = dateTime - new DateTime(1970, 1, 1, 0, 0, 0);
                
                int stopwatch = (int)timestamp.TotalSeconds - PlayerPrefs.GetInt("net", (int)timestamp.TotalSeconds);

                Debug.Log(stopwatch + "sec");
                PlayerPrefs.SetInt("net", (int)timestamp.TotalSeconds);
            }
        }
    }
}
