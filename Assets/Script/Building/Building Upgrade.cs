using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;


public class BuildingUpgrade : MonoBehaviour
{
    public TextMeshProUGUI NeedGold;
    public TextMeshProUGUI NeedGem;
    public TextMeshProUGUI NeedWater;
    public GameObject GoldWarningText;
    public GameObject GemWarningText;
    public GameObject WaterWarningText;

    public bool isInMainScene = true;
    public void OnGoldClick()
    {
        if(ResourceManager.instance.Gold >= LevelManager.instance.GoldCaveLevel * 100)
        {
            ResourceManager.instance.Gold -= 100 * LevelManager.instance.GoldCaveLevel;
            LevelManager.instance.GoldCaveLevel = LevelManager.instance.GoldCaveLevel += 1;
            Debug.Log("남은 골드는 " + ResourceManager.instance.Gold);
            Debug.Log("현재 이 건물의 레벨은" + LevelManager.instance.GoldCaveLevel);
        }
        else
        {
            GoldWarningText.SetActive(true);
            StartCoroutine(HideGoldTextAfterDelay(3f));
        }
    
    IEnumerator HideGoldTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GoldWarningText.SetActive(false);
    }
    }
    public void OnGemClick()
    {
        if(ResourceManager.instance.Gold >= LevelManager.instance.GemCaveLevel * 100)
        {
            ResourceManager.instance.Gold -= 100 * LevelManager.instance.GemCaveLevel;
            LevelManager.instance.GemCaveLevel = LevelManager.instance.GemCaveLevel += 1;
            Debug.Log("남은 골드는 " + ResourceManager.instance.Gold);
            Debug.Log("현재 이 건물의 레벨은" + LevelManager.instance.GemCaveLevel);
        }
        else
        {
            GemWarningText.SetActive(true);
            StartCoroutine(HideGemTextAfterDelay(3f));
        }

    IEnumerator HideGemTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GemWarningText.SetActive(false);
    }
    }
    public void OnWaterClick()
    
    {
        if(ResourceManager.instance.Gold >= LevelManager.instance.WStatueLevel * 100)
        {
            ResourceManager.instance.Gold -= 100 * LevelManager.instance.WStatueLevel;
            LevelManager.instance.WStatueLevel = LevelManager.instance.WStatueLevel += 1;
            Debug.Log("남은 골드는 " + ResourceManager.instance.Gold);
            Debug.Log("현재 이 건물의 레벨은" + LevelManager.instance.WStatueLevel);
        }
        else
        {
            WaterWarningText.SetActive(true);
            StartCoroutine(HideWaterTextAfterDelay(3f));
        }
    
    IEnumerator HideWaterTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        WaterWarningText.SetActive(false);
    }
    }
        
    
    void Update()
    {
        if (!isInMainScene) { return; }

        NeedGold.text = "Gold :" + LevelManager.instance.GoldCaveLevel * 100;
        NeedGem.text = "Gold :" + LevelManager.instance.GemCaveLevel * 100;
        NeedWater.text = "Gold :" + LevelManager.instance.WStatueLevel * 100;
    }
    void Read_Json_file()
    {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main")
        {
            isInMainScene = true;
            Transform canvasTrans = GameObject.Find("Canvas").transform;

            NeedGold = canvasTrans.Find("GoldPopup").gameObject.transform.Find("Upgrade").gameObject.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
            NeedGem = canvasTrans.Find("GemPopup").gameObject.transform.Find("Upgrade").gameObject.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
            NeedWater = canvasTrans.Find("WaterPopup").gameObject.transform.Find("Upgrade").gameObject.transform.Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
            GoldWarningText = canvasTrans.Find("GoldPopup").gameObject.transform.Find("Warning").gameObject;
            GemWarningText = canvasTrans.Find("GemPopup").gameObject.transform.Find("Warning").gameObject;
            WaterWarningText = canvasTrans.Find("WaterPopup").gameObject.transform.Find("Warning").gameObject;
        }
        else
        {
            isInMainScene = false;
        }
    }

    void Start()
    {
        Read_Json_file();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
