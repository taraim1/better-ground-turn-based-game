using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class NewBuilding : MonoBehaviour
{
    public string url = "";
    private int isFirstClick = 0; 
    public void OnButtonClick()
    {
        StartCoroutine(WebChk());
    }
    IEnumerator WebChk()
    {
        UnityWebRequest request = new UnityWebRequest();
        using (request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string date = request.GetResponseHeader("date");

                DateTime dateTime = DateTime.Parse(date);
                TimeSpan timestamp = dateTime - new DateTime(1970, 1, 1, 0, 0, 0);

                int currentTimestamp = (int)timestamp.TotalSeconds;

                if (isFirstClick == 0)
                {
                    PlayerPrefs.SetInt("net", currentTimestamp);
                    Debug.Log("Time saved: " + currentTimestamp + " sec");
                    isFirstClick = 1;
                }
                else
                {
                    int savedTimestamp = PlayerPrefs.GetInt("net", currentTimestamp);
                    int elapsedTime = currentTimestamp - savedTimestamp;
                    Debug.Log("Elapsed time: " + elapsedTime + " sec");
                }
            }
        }
    }
}

