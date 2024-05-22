using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    private DateTime firstButtonClickTime;
    private DateTime lastButtonClickTime;
    private bool isFirstClickSet = false;
    public Text GoldText;
    public GameObject GoldImage;
    public GameObject Popup;
    public void OnBuildingClick()
    {
        if (isFirstClickSet == false)
        {
            firstButtonClickTime = DateTime.Now;
            isFirstClickSet = true;
            ResourceManager.instance.Gold += 100;
            GoldText.text = "Gold : " +  ResourceManager.instance.Gold;
            StartCoroutine(Imageup());
            Debug.Log("Gold : " + ResourceManager.instance.Gold);
            Debug.Log("First button clicked at: " + firstButtonClickTime);
        }
        else
        {
            lastButtonClickTime = DateTime.Now;
            Debug.Log("Last button clicked at: " + lastButtonClickTime);
            if (isFirstClickSet == true)
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
            if (SpentMinutes >= 10)
            {
                SpentMinutes = 10;
                ResourceManager.instance.Gold = ResourceManager.instance.Gold + SpentMinutes*LevelManager.instance.GoldCaveLevel;
                GoldText.text = "Gold : " +  ResourceManager.instance.Gold;
                StartCoroutine(Imageup());
                Debug.Log("Gold : " + ResourceManager.instance.Gold);
            }
            else
            {
                ResourceManager.instance.Gold = ResourceManager.instance.Gold + SpentMinutes*LevelManager.instance.GoldCaveLevel;
                GoldText.text = "Gold : " +  ResourceManager.instance.Gold;
                StartCoroutine(Imageup());
                Debug.Log("Gold : " + ResourceManager.instance.Gold);
            }
            firstButtonClickTime = DateTime.Now;
            GoldImage.SetActive(false);
        }
        else
        {
            Popup.SetActive(true);
        }
    }
    IEnumerator Imageup()
    {
	    yield return new WaitForSeconds(300f);
        GoldImage.SetActive(true);
    }   
    
}
