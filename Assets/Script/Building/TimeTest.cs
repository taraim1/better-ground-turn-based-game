using System;
using UnityEngine;

public class TimeSpanCalculator : MonoBehaviour
{
    private DateTime firstButtonClickTime;
    private DateTime lastButtonClickTime;

    private bool isFirstClickSet = false;
    private bool isLastClickSet = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            firstButtonClickTime = DateTime.Now;
            isFirstClickSet = true;
            Debug.Log("First button clicked at: " + firstButtonClickTime);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            lastButtonClickTime = DateTime.Now;
            isLastClickSet = true;
            Debug.Log("Last button clicked at: " + lastButtonClickTime);

            if (isFirstClickSet && isLastClickSet)
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
    }
}
