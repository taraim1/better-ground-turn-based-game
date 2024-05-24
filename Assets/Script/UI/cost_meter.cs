using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class cost_meter : MonoBehaviour
{
    [SerializeField]
    private Image fill;
    [SerializeField]
    private TMP_Text tmp;

    private int max_cost;
    private int current_cost;
    public int Current_cost { 
        get { return current_cost; }
        set 
        {
            current_cost += value;
            if (current_cost > max_cost) { current_cost = max_cost; }
            if (current_cost < 0) { current_cost = 0; }
            fill.fillAmount = ((float)current_cost / (float)max_cost);
            tmp.text = string.Format("cost\n{0}", current_cost);
        }
            
    }
    public void Setup(int max_cost, int current_cost) 
    { 
        this.max_cost = max_cost;
        this.Current_cost = current_cost;
    }



    
}
