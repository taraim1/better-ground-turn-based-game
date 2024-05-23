using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    [System.Serializable]
    private class BuildingData // �ǹ��� �����͸� �����ϴ� Ŭ����
    {
        public string first_time_Gold_string;
        public string last_time_Gold_string;
        public bool isFirstClickSetGold = false;
        public string first_time_Gem_string;
        public string last_time_Gem_string;
        public bool isFirstClickSetGem = false;        
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
    public Text GoldText;
    public GameObject GoldImage;
    public GameObject GoldPopup;
    public Text GemText;
    public GameObject GemImage;
    public GameObject GemPopup;    
    public Text WaterText;
    public GameObject WaterImage;
    public GameObject WaterPopup;
    BuildingData buildingData = new BuildingData();

    public void OnGoldClick()
    {
        if (buildingData.isFirstClickSetGold == false)
        {
            firstGoldClickTime = DateTime.Now;
            buildingData.isFirstClickSetGold = true;
            ResourceManager.instance.Gold += 100;
            GoldText.text = "Gold : " +  ResourceManager.instance.Gold;
            Write_Json_file();
            Debug.Log("Gold : " + ResourceManager.instance.Gold);
            Debug.Log("First button clicked at: " + firstGoldClickTime);
        }
        else
        {
            lastGoldClickTime = DateTime.Now;
            Debug.Log("Last button clicked at: " + lastGoldClickTime);
            if (buildingData.isFirstClickSetGold == true)
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
            GoldText.text = "Gold : " +  ResourceManager.instance.Gold;
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
        if (buildingData.isFirstClickSetGem == false)
        {
            firstGemClickTime = DateTime.Now;
            buildingData.isFirstClickSetGem = true;
            ResourceManager.instance.Gem += 100;
            GemText.text = "Gem : " +  ResourceManager.instance.Gem;
            Write_Json_file();
            Debug.Log("Gem : " + ResourceManager.instance.Gem);
            Debug.Log("First button clicked at: " + firstGemClickTime);
        }
        else
        {
            lastGemClickTime = DateTime.Now;
            Debug.Log("Last button clicked at: " + lastGemClickTime);
            if (buildingData.isFirstClickSetGem == true)
            {
                CalculateTimeSpanGem();
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
            GemText.text = "Gem : " +  ResourceManager.instance.Gem;
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

    private void Update()
    {
        // ���� �ð��� ������ �ڿ� ȹ�� ���� �˾��� ��
        if (buildingData.isFirstClickSetGold && GoldImage.activeSelf == false) 
        {
            TimeSpan timeDifferenceGold = DateTime.Now - firstGoldClickTime;
            int SpentMinutesGold = (int)timeDifferenceGold.TotalMinutes;
            if (SpentMinutesGold >= 5) 
            {
                GoldImage.SetActive(true);
            }
        }
        
        if (buildingData.isFirstClickSetGem && GemImage.activeSelf == false) 
        {
            TimeSpan timeDifferenceGem = DateTime.Now - firstGoldClickTime;
            int SpentMinutesGem = (int)timeDifferenceGem.TotalMinutes;
            if (SpentMinutesGem >= 5) 
            {
                GemImage.SetActive(true);
            }
        }
    }

    void Write_Json_file() 
    {
        buildingData.first_time_Gold_string = firstGoldClickTime.ToString("yyyy-MM-dd HH:mm:ss");
        buildingData.last_time_Gold_string = lastGoldClickTime.ToString("yyyy-MM-dd HH:mm:ss");
        string outputGold = JsonUtility.ToJson(buildingData, true);
        File.WriteAllText(Application.dataPath + "/Data/building/gold_cave.json", outputGold);
        buildingData.first_time_Gem_string = firstGemClickTime.ToString("yyyy-MM-dd HH:mm:ss");
        buildingData.last_time_Gem_string = lastGemClickTime.ToString("yyyy-MM-dd HH:mm:ss");
        string outputGem = JsonUtility.ToJson(buildingData, true);
        File.WriteAllText(Application.dataPath + "/Data/building/Gem_cave.json", outputGem);
    }

    void Read_Json_file() 
    {
        if (!File.Exists(Application.dataPath + "/Data/building/gold_cave.json")) 
        {
            Debug.Log("�ǹ� ������ ������ �����ϴ�.");
            return;
        }

        buildingData = JsonUtility.FromJson<BuildingData>(File.ReadAllText(Application.dataPath + "/Data/building/gold_cave.json"));
        firstGoldClickTime = DateTime.ParseExact(buildingData.first_time_Gold_string, "yyyy-MM-dd HH:mm:ss", null);
        lastGoldClickTime = DateTime.ParseExact(buildingData.last_time_Gold_string, "yyyy-MM-dd HH:mm:ss", null);

        if (!File.Exists(Application.dataPath + "/Data/building/gem_cave.json")) 
        {
            Debug.Log("�ǹ� ������ ������ �����ϴ�.");
            return;
        }

        buildingData = JsonUtility.FromJson<BuildingData>(File.ReadAllText(Application.dataPath + "/Data/building/gem_cave.json"));
        firstGemClickTime = DateTime.ParseExact(buildingData.first_time_Gem_string, "yyyy-MM-dd HH:mm:ss", null);
        lastGemClickTime = DateTime.ParseExact(buildingData.last_time_Gem_string, "yyyy-MM-dd HH:mm:ss", null);
    }

    private void Start()
    {
        Write_Json_file();
        Read_Json_file();
    }
}
