using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using TMPro;

public class Building : MonoBehaviour
{
    [System.Serializable]
    private class BuildingDataGold // �ǹ��� �����͸� �����ϴ� Ŭ����
    {
        public string first_time_Gold_string;
        public string last_time_Gold_string;
        public bool isFirstClickSetGold = false;    
    }
    
    [System.Serializable]
    private class BuildingDataGem
    {
        public string first_time_Gem_string;
        public string last_time_Gem_string;
        public bool isFirstClickSetGem = false;   
    }

    [System.Serializable]
    private class BuildingDataWater
    {
        public string first_time_Water_string;
        public string last_time_Water_string;
        public bool isFirstClickSetWater = false;
    }

    public DateTime firstGoldClickTime;
    public DateTime lastGoldClickTime;
    public DateTime firstGemClickTime;
    public DateTime lastGemClickTime;
    public DateTime firstWaterClickTime;
    public DateTime lastWaterClickTime;
    public TextMeshProUGUI GoldText;
    public GameObject GoldImage;
    public GameObject GoldPopup;
    public TextMeshProUGUI GemText;
    public GameObject GemImage;
    public GameObject GemPopup;    
    public TextMeshProUGUI WaterText;
    public GameObject WaterImage;
    public GameObject WaterPopup;
    BuildingDataGold buildingDataGold = new BuildingDataGold();
    BuildingDataGem buildingDataGem = new BuildingDataGem();
    BuildingDataWater buildingDataWater = new BuildingDataWater();

    public Button GoldButton;
    public Button GemButton;
    public Button WaterButton;

    public bool isInMainScene = true;
    public void OnGoldClick()
    {
        if (buildingDataGold.isFirstClickSetGold == false)
        {
            firstGoldClickTime = DateTime.Now;
            buildingDataGold.isFirstClickSetGold = true;
            ResourceManager.instance.Gold += 1000;
            //GoldText.text = "골드 : " +  ResourceManager.instance.Gold;
            Write_Json_file();
            Debug.Log("Gold : " + ResourceManager.instance.Gold);
            Debug.Log("First button clicked at: " + firstGoldClickTime);
            GoldImage.SetActive(false);
        }
        else
        {
            lastGoldClickTime = DateTime.Now;
            Debug.Log("Last button clicked at: " + lastGoldClickTime);
            if (buildingDataGold.isFirstClickSetGold == true)
            {
                CalculateTimeSpanGold();
            }
        }
    }
    void CalculateTimeSpanGold()
    {
        TimeSpan timeDifferenceGold = lastGoldClickTime - firstGoldClickTime;
        int SpentMinutesGold = (int)timeDifferenceGold.TotalMinutes;
        Debug.Log("Time span in minutes: " + SpentMinutesGold);

        if (SpentMinutesGold >= 5)
        {
            if (SpentMinutesGold >= 10){ SpentMinutesGold = 10; }
            ResourceManager.instance.Gold = ResourceManager.instance.Gold + SpentMinutesGold*LevelManager.instance.GoldCaveLevel;
            //GoldText.text = "골드 : " +  ResourceManager.instance.Gold;
            Debug.Log("Gold : " + ResourceManager.instance.Gold);
            firstGoldClickTime = DateTime.Now;
            GoldImage.SetActive(false);

            Write_Json_file();
        }
        else
        {
            GoldPopup.SetActive(true);
        }
    }
    public void OnGemClick()
    {
        if (buildingDataGem.isFirstClickSetGem == false)
        {
            firstGemClickTime = DateTime.Now;
            buildingDataGem.isFirstClickSetGem = true;
            ResourceManager.instance.Gem += 100;
            //GemText.text = "보석 : " +  ResourceManager.instance.Gem;
            Write_Json_file();
            Debug.Log("Gem : " + ResourceManager.instance.Gem);
            Debug.Log("First button clicked at: " + firstGemClickTime);
            GemImage.SetActive(false);
        }
        else
        {
            lastGemClickTime = DateTime.Now;
            Debug.Log("Last button clicked at: " + lastGemClickTime);
            if (buildingDataGem.isFirstClickSetGem == true)
            {
                CalculateTimeSpanGem();
                GemImage.SetActive(false);
            }
        }
    }
    void CalculateTimeSpanGem()
    {
        TimeSpan timeDifferenceGem = lastGemClickTime - firstGemClickTime;
        int SpentMinutesGem = (int)timeDifferenceGem.TotalMinutes;
        Debug.Log("Time span in minutes: " + SpentMinutesGem);

        if (SpentMinutesGem >= 5)
        {
            if (SpentMinutesGem >= 10){ SpentMinutesGem = 10; }
            ResourceManager.instance.Gem = ResourceManager.instance.Gem + SpentMinutesGem*LevelManager.instance.GemCaveLevel;
            //GemText.text = "보석 : " +  ResourceManager.instance.Gem;
            Debug.Log("Gem : " + ResourceManager.instance.Gem);

            firstGemClickTime = DateTime.Now;
            GemImage.SetActive(false);

            Write_Json_file();
        }
        else
        {
            GemPopup.SetActive(true);
        }
    }
    public void OnWaterClick()
    {

        if (buildingDataWater.isFirstClickSetWater == false)
        {
            firstWaterClickTime = DateTime.Now;
            buildingDataWater.isFirstClickSetWater = true;
            ResourceManager.instance.Water += 100;
            //WaterText.text = "성수 : " +  ResourceManager.instance.Water;
            Write_Json_file();
            Debug.Log("성수 : " + ResourceManager.instance.Water);
            Debug.Log("First button clicked at: " + firstWaterClickTime);
            WaterImage.SetActive(false);
        }
        else
        {
            lastWaterClickTime = DateTime.Now;
            Debug.Log("Last button clicked at: " + lastWaterClickTime);
            if (buildingDataWater.isFirstClickSetWater == true)
            {
                CalculateTimeSpanWater();
            }
        }
    }
    void CalculateTimeSpanWater()
    {
        TimeSpan timeDifferenceWater = lastWaterClickTime - firstWaterClickTime;
        int SpentMinutesWater = (int)timeDifferenceWater.TotalMinutes;
        Debug.Log("Time span in minutes: " + SpentMinutesWater);

        if (SpentMinutesWater >= 5)
        {
            if (SpentMinutesWater >= 10){ SpentMinutesWater = 10; }
            ResourceManager.instance.Water = ResourceManager.instance.Water + SpentMinutesWater*LevelManager.instance.WStatueLevel;
            //WaterText.text = "성수 : " +  ResourceManager.instance.Water;
            Debug.Log("Water : " + ResourceManager.instance.Water);

            firstWaterClickTime = DateTime.Now;
            WaterImage.SetActive(false);

            Write_Json_file();
        }
        else
        {
            WaterPopup.SetActive(true);
        }
    }

    private void Update()
    {
        if (!isInMainScene) { return; }

        // ���� �ð��� ������ �ڿ� ȹ�� ���� �˾��� ��
        if (buildingDataGold.isFirstClickSetGold && GoldImage.activeSelf == false) 
        {
            TimeSpan timeDifferenceGold = DateTime.Now - firstGoldClickTime;
            int SpentMinutesGold = (int)timeDifferenceGold.TotalMinutes;
            if (SpentMinutesGold >= 5) 
            {
                GoldImage.SetActive(true);
            }
        }
        
        if (buildingDataGem.isFirstClickSetGem && GemImage.activeSelf == false) 
        {
            TimeSpan timeDifferenceGem = DateTime.Now - firstGoldClickTime;
            int SpentMinutesGem = (int)timeDifferenceGem.TotalMinutes;
            if (SpentMinutesGem >= 5) 
            {
                GemImage.SetActive(true);
            }
        }
        
        if (buildingDataWater.isFirstClickSetWater && WaterImage.activeSelf == false) 
        {
            TimeSpan timeDifferenceWater = DateTime.Now - firstWaterClickTime;
            int SpentMinutesWater = (int)timeDifferenceWater.TotalMinutes;
            if (SpentMinutesWater >= 5) 
            {
                WaterImage.SetActive(true);
            }
        }
        GoldText.text = "골드 : " +  ResourceManager.instance.Gold;
        GemText.text = "보석 : " +  ResourceManager.instance.Gem;
        WaterText.text = "성수 : " +  ResourceManager.instance.Water;
    }
    
    void Write_Json_file() 
    {
        buildingDataGold.first_time_Gold_string = firstGoldClickTime.ToString("yyyy-MM-dd HH:mm:ss");
        buildingDataGold.last_time_Gold_string = lastGoldClickTime.ToString("yyyy-MM-dd HH:mm:ss");
        string outputGold = JsonUtility.ToJson(buildingDataGold, true);
        File.WriteAllText(Application.dataPath + "/Data/building/gold_cave.json", outputGold);
        buildingDataGem.first_time_Gem_string = firstGemClickTime.ToString("yyyy-MM-dd HH:mm:ss");
        buildingDataGem.last_time_Gem_string = lastGemClickTime.ToString("yyyy-MM-dd HH:mm:ss");
        string outputGem = JsonUtility.ToJson(buildingDataGem, true);
        File.WriteAllText(Application.dataPath + "/Data/building/Gem_cave.json", outputGem);
        buildingDataWater.first_time_Water_string = firstWaterClickTime.ToString("yyyy-MM-dd HH:mm:ss");
        buildingDataWater.last_time_Water_string = lastWaterClickTime.ToString("yyyy-MM-dd HH:mm:ss");
        string outputWater = JsonUtility.ToJson(buildingDataWater, true);
        File.WriteAllText(Application.dataPath + "/Data/building/Water_statue.json", outputWater);
    }

    void Read_Json_file() 
    {
        if (!File.Exists(Application.dataPath + "/Data/building/gold_cave.json")) 
        {
            Debug.Log("�ǹ� ������ ������ �����ϴ�.");
            return;
        }

        buildingDataGold = JsonUtility.FromJson<BuildingDataGold>(File.ReadAllText(Application.dataPath + "/Data/building/gold_cave.json"));
        firstGoldClickTime = DateTime.ParseExact(buildingDataGold.first_time_Gold_string, "yyyy-MM-dd HH:mm:ss", null);
        lastGoldClickTime = DateTime.ParseExact(buildingDataGold.last_time_Gold_string, "yyyy-MM-dd HH:mm:ss", null);

        if (!File.Exists(Application.dataPath + "/Data/building/gem_cave.json")) 
        {
            Debug.Log("�ǹ� ������ ������ �����ϴ�.");
            return;
        }

        buildingDataGem = JsonUtility.FromJson<BuildingDataGem>(File.ReadAllText(Application.dataPath + "/Data/building/gem_cave.json"));
        firstGemClickTime = DateTime.ParseExact(buildingDataGem.first_time_Gem_string, "yyyy-MM-dd HH:mm:ss", null);
        lastGemClickTime = DateTime.ParseExact(buildingDataGem.last_time_Gem_string, "yyyy-MM-dd HH:mm:ss", null);

        if (!File.Exists(Application.dataPath + "/Data/building/Water_statue.json")) 
        {
            Debug.Log("�ǹ� ������ ������ �����ϴ�.");
            return;
        }

        buildingDataWater = JsonUtility.FromJson<BuildingDataWater>(File.ReadAllText(Application.dataPath + "/Data/building/Water_statue.json"));
        firstWaterClickTime = DateTime.ParseExact(buildingDataWater.first_time_Water_string, "yyyy-MM-dd HH:mm:ss", null);
        lastWaterClickTime = DateTime.ParseExact(buildingDataWater.last_time_Water_string, "yyyy-MM-dd HH:mm:ss", null);
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

    private void Start()
    {
        Read_Json_file();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
