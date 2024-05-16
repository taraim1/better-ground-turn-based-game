using UnityEngine;
using UnityEngine.UI;
using System;

public class GoldButton : MonoBehaviour
{
    public Text goldText;
    //private int goldAmount = 0;
    private DateTime lastTimeGoldEarned;

    private bool isFirstClick = true; // 맨 처음 클릭 여부를 저장하기 위한 변수

    private void Start()
    {
        // 이전에 골드를 획득한 시간을 로드
        string lastTimeGoldEarnedString = PlayerPrefs.GetString("LastTimeGoldEarned");
        lastTimeGoldEarned = DateTime.Parse(lastTimeGoldEarnedString);

    

        // 처음 실행 시 골드 텍스트 업데이트
        UpdateGoldText();
    }

    public void OnClick()
    {
        if (isFirstClick)
        {
            // 처음 클릭 시에만 골드를 추가
            ResourceManager.instance.Gold += 100; // 추가 골드값
            isFirstClick = false; // isFirstClick을 false로 설정하여 다음에는 이 로직이 실행되지 않도록 함
        }
        else
        {
            // 현재 시간을 기준으로 마지막으로 골드를 획득한 시간부터 경과한 시간을 계산
            TimeSpan timeSinceLastGoldEarned = DateTime.Now - lastTimeGoldEarned;
            int minutesSinceLastGoldEarned = (int)timeSinceLastGoldEarned.TotalMinutes;

            // 1분 이상 경과했을 때만 골드를 획득
            if (minutesSinceLastGoldEarned >= 1)
            {
                // 골드를 획득하고 시간을 갱신
                int goldEarned = minutesSinceLastGoldEarned * 100; // 1분마다 100골드씩 획득
                ResourceManager.instance.Gold += goldEarned;
                lastTimeGoldEarned = DateTime.Now;
                PlayerPrefs.SetString("LastTimeGoldEarned", lastTimeGoldEarned.ToString());
                UpdateGoldText();
            }
            else
            {
                // 2분 미만이면 골드를 획득할 수 없음
                Debug.Log("아직 골드를 얻을 수 없습니다. 좀 더 기다려주세요!");
            }
        }
    }

    private void UpdateGoldText()
{
    if (goldText != null)
    {
        goldText.text = "Gold: " + ResourceManager.instance.Gold.ToString();
    }
    else
    {
        Debug.LogError("GoldText is not assigned in the Inspector!");
    }
}

}
