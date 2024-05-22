using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    [System.Serializable]
    private class BuildingData // 건물의 데이터를 저장하는 클래스
    {
        public string first_time_string;
        public string last_time_string;
        public bool isFirstClickSet = false;
    }

    public DateTime firstButtonClickTime;
    public DateTime lastButtonClickTime;

    public Text GoldText;
    public GameObject GoldImage;
    public GameObject Popup;


    BuildingData buildingData = new BuildingData();

    public void OnBuildingClick()
    {
        if (buildingData.isFirstClickSet == false)
        {
            firstButtonClickTime = DateTime.Now;
            buildingData.isFirstClickSet = true;
            ResourceManager.instance.Gold += 100;
            GoldText.text = "Gold : " +  ResourceManager.instance.Gold;
            Write_Json_file();
            Debug.Log("Gold : " + ResourceManager.instance.Gold);
            Debug.Log("First button clicked at: " + firstButtonClickTime);
        }
        else
        {
            lastButtonClickTime = DateTime.Now;
            Debug.Log("Last button clicked at: " + lastButtonClickTime);
            if (buildingData.isFirstClickSet == true)
            {
                CalculateTimeSpan();
            }
        }
    }
    void CalculateTimeSpan()
    {
        TimeSpan timeDifference = lastButtonClickTime - firstButtonClickTime;
        int SpentMinutes = (int)timeDifference.TotalMinutes;
        Debug.Log("Time span in minutes: " + SpentMinutes);

        if (SpentMinutes >= 5)
        {
            if (SpentMinutes >= 10){ SpentMinutes = 10; }
            ResourceManager.instance.Gold = ResourceManager.instance.Gold + SpentMinutes*LevelManager.instance.GoldCaveLevel;
            GoldText.text = "Gold : " +  ResourceManager.instance.Gold;
            Debug.Log("Gold : " + ResourceManager.instance.Gold);

            firstButtonClickTime = DateTime.Now;
            GoldImage.SetActive(false);

            Write_Json_file();
        }
        else
        {
            Popup.SetActive(true);
        }
    }


    private void Update()
    {
        // 일정 시간이 지나면 자원 획득 가능 팝업이 뜸
        if (buildingData.isFirstClickSet && GoldImage.activeSelf == false) 
        {
            TimeSpan timeDifference = DateTime.Now - firstButtonClickTime;
            int SpentMinutes = (int)timeDifference.TotalMinutes;
            if (SpentMinutes >= 5) 
            {
                GoldImage.SetActive(true);
            }
        }
    }

    void Write_Json_file() 
    {
        buildingData.first_time_string = firstButtonClickTime.ToString("yyyy-MM-dd HH:mm:ss");
        buildingData.last_time_string = lastButtonClickTime.ToString("yyyy-MM-dd HH:mm:ss");
        string output = JsonUtility.ToJson(buildingData, true);
        File.WriteAllText(Application.dataPath + "/Data/building/gold_mine.json", output);
    }

    void Read_Json_file() 
    {
        if (!File.Exists(Application.dataPath + "/Data/building/gold_mine.json")) 
        {
            Debug.Log("건물 데이터 파일이 없습니다.");
            return;
        }

        buildingData = JsonUtility.FromJson<BuildingData>(File.ReadAllText(Application.dataPath + "/Data/building/gold_mine.json"));
        firstButtonClickTime = DateTime.ParseExact(buildingData.first_time_string, "yyyy-MM-dd HH:mm:ss", null);
        lastButtonClickTime = DateTime.ParseExact(buildingData.last_time_string, "yyyy-MM-dd HH:mm:ss", null);
    }

    private void Start()
    {
        Read_Json_file();
    }
}
