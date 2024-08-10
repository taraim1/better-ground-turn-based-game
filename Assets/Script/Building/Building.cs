using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using TMPro;
using UnityEngine.Networking;
using Unity.VisualScripting;

public class Building : MonoBehaviour
{
    private int isFirstClickSetGold = 1;  
    public int isFirstClickSetGem = 1;   
    public int isFirstClickSetWater = 1;
    public TextMeshProUGUI GoldText;
    public GameObject GoldImage;
    public GameObject GoldPopup;
    public TextMeshProUGUI GemText;
    public GameObject GemImage;
    public GameObject GemPopup;    
    public TextMeshProUGUI WaterText;
    public GameObject WaterImage;
    public GameObject WaterPopup;
    public Button GoldButton;
    public Button GemButton;
    public Button WaterButton;

    public bool isInMainScene = true;
    public string url = "";
    private int GoldTime = 1;
    private int GemTime = 1;
    private int WaterTime = 1;
    private void Start()
    {
        //Read_Json_file();
        SceneManager.sceneLoaded += OnSceneLoaded;
        isFirstClickSetGold = PlayerPrefs.GetInt("isFirstClickSetGold", 0);
        isFirstClickSetGem = PlayerPrefs.GetInt("isFirstClickSetGem", 0);
        isFirstClickSetWater = PlayerPrefs.GetInt("isFirstClickSetWater", 0);
    }
    public void OnGoldClick()
    {
        StartCoroutine(GoldWebChk());
    }
    IEnumerator GoldWebChk()
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
                string dateGold = request.GetResponseHeader("date");

                DateTime dateTimeGold = DateTime.Parse(dateGold);
                TimeSpan timestampGold = dateTimeGold - new DateTime(1970, 1, 1, 0, 0, 0);

                int currentTimestampGold = (int)timestampGold.TotalMinutes;

                if (isFirstClickSetGold == 1)
                {
                    PlayerPrefs.SetInt("netGold", currentTimestampGold);
                    ResourceManager.instance.Gold += 1000;
                    Debug.Log("Gold : " + ResourceManager.instance.Gold);
                    GoldImage.SetActive(false);
                    isFirstClickSetGold = 0;
                    PlayerPrefs.SetInt("isFirstClickSetGold", isFirstClickSetGold);
                }
                else
                {
                    int savedTimestampGold = PlayerPrefs.GetInt("netGold", currentTimestampGold);
                    int elapsedTimeGold = currentTimestampGold - savedTimestampGold;
                    Debug.Log("Elapsed time Gold : " + elapsedTimeGold + " min");
                    if (elapsedTimeGold >= GoldTime)
                    {
                        if (elapsedTimeGold >= 100){ elapsedTimeGold = 100; }
                        ResourceManager.instance.Gold = ResourceManager.instance.Gold + elapsedTimeGold*5*LevelManager.instance.GoldCaveLevel;
                        Debug.Log("Gold : " + ResourceManager.instance.Gold);
                        GoldImage.SetActive(false);
                        PlayerPrefs.SetInt("netGold", currentTimestampGold);
                    }
                    else
                    {
                        GoldPopup.SetActive(true);
                    }
                    
                }
            }
        }
    }
    public void OnGemClick()
    {
        StartCoroutine(GemWebChk());
    }
        IEnumerator GemWebChk()
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
                string dateGem = request.GetResponseHeader("date");

                DateTime dateTimeGem = DateTime.Parse(dateGem);
                TimeSpan timestampGem = dateTimeGem - new DateTime(1970, 1, 1, 0, 0, 0);

                int currentTimestampGem = (int)timestampGem.TotalMinutes;

                if (isFirstClickSetGem == 1)
                {
                    PlayerPrefs.SetInt("netGem", currentTimestampGem);
                    ResourceManager.instance.Gem += 100;
                    Debug.Log("Gem : " + ResourceManager.instance.Gem);
                    GemImage.SetActive(false);
                    isFirstClickSetGem = 0;
                    PlayerPrefs.SetInt("isFirstClickSetGem", isFirstClickSetGem);
                }
                else
                {
                    int savedTimestampGem = PlayerPrefs.GetInt("netGem", currentTimestampGem);
                    int elapsedTimeGem = currentTimestampGem - savedTimestampGem;
                    Debug.Log("Elapsed time Gem : " + elapsedTimeGem + " min");
                    if (elapsedTimeGem >= GemTime)
                    {
                        if (elapsedTimeGem >= 100){ elapsedTimeGem = 100; }
                        ResourceManager.instance.Gem = ResourceManager.instance.Gem + elapsedTimeGem*LevelManager.instance.GemCaveLevel;
                        Debug.Log("Gem : " + ResourceManager.instance.Gem);
                        GemImage.SetActive(false);
                        PlayerPrefs.SetInt("netGem", currentTimestampGem);
                    }
                    else
                    {
                        GemPopup.SetActive(true);
                    }
                    
                }
            }
        }
    }
    public void OnWaterClick()
    {
        StartCoroutine(WaterWebChk());
    }
    
    IEnumerator WaterWebChk()
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
                string dateWater = request.GetResponseHeader("date");

                DateTime dateTimeWater = DateTime.Parse(dateWater);
                TimeSpan timestampWater = dateTimeWater - new DateTime(1970, 1, 1, 0, 0, 0);

                int currentTimestampWater = (int)timestampWater.TotalMinutes;

                if (isFirstClickSetWater == 1)
                {
                    PlayerPrefs.SetInt("netWater", currentTimestampWater);
                    ResourceManager.instance.Water += 100;
                    Debug.Log("Water : " + ResourceManager.instance.Water);
                    WaterImage.SetActive(false);
                    isFirstClickSetWater = 0;
                    PlayerPrefs.SetInt("isFirstClickSetWater", isFirstClickSetWater);
                }
                else
                {
                    int savedTimestampWater = PlayerPrefs.GetInt("netWater", currentTimestampWater);
                    int elapsedTimeWater = currentTimestampWater - savedTimestampWater;
                    Debug.Log("Elapsed time Water : " + elapsedTimeWater + " min");
                    if (elapsedTimeWater >= WaterTime)
                    {
                        if (elapsedTimeWater >= 100){ elapsedTimeWater = 100; }
                        ResourceManager.instance.Water = ResourceManager.instance.Water + elapsedTimeWater*LevelManager.instance.WStatueLevel;
                        Debug.Log("Water : " + ResourceManager.instance.Water);
                        WaterImage.SetActive(false);
                        PlayerPrefs.SetInt("netWater", currentTimestampWater);
                    }
                    else
                    {
                        WaterPopup.SetActive(true);
                    }
                    
                }
            }
        }
    }
    void Update()
    {
        GoldText.text = "골드 : " +  ResourceManager.instance.Gold;
        GemText.text = "보석 : " +  ResourceManager.instance.Gem;
        WaterText.text = "성수 : " +  ResourceManager.instance.Water;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main")
        {
            isInMainScene = true;

            Transform canvasTrans = GameObject.Find("Canvas").transform;

            GoldText = canvasTrans.Find("GoldBuilding").gameObject.transform.Find("GoldText").gameObject.GetComponent<TextMeshProUGUI>();
            GoldImage = canvasTrans.Find("GoldBuilding").gameObject.transform.Find("GoldImage").gameObject;
            GoldPopup = canvasTrans.Find("GoldPopup").gameObject;
            GoldButton = canvasTrans.Find("GoldBuilding").gameObject.GetComponent<Button>();
            GoldButton.onClick.RemoveAllListeners();
            GoldButton.onClick.AddListener(OnGoldClick);
            GemText = canvasTrans.Find("GemBuilding").gameObject.transform.Find("GemText").gameObject.GetComponent<TextMeshProUGUI>();
            GemImage = canvasTrans.Find("GemBuilding").gameObject.transform.Find("GemImage").gameObject;
            GemPopup = canvasTrans.Find("GemPopup").gameObject;
            GemButton = canvasTrans.Find("GemBuilding").gameObject.GetComponent<Button>();
            GemButton.onClick.RemoveAllListeners();
            GemButton.onClick.AddListener(OnGemClick);
            WaterText = canvasTrans.Find("WaterBuilding").gameObject.transform.Find("WaterText").gameObject.GetComponent<TextMeshProUGUI>();
            WaterImage = canvasTrans.Find("WaterBuilding").gameObject.transform.Find("WaterImage").gameObject;
            WaterPopup = canvasTrans.Find("WaterPopup").gameObject;
            WaterButton = canvasTrans.Find("WaterBuilding").gameObject.GetComponent<Button>();
            WaterButton.onClick.RemoveAllListeners();
            WaterButton.onClick.AddListener(OnWaterClick);


        }
        else
        {
            isInMainScene = false;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}