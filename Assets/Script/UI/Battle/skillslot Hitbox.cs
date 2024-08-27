using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillslotHitbox : MonoBehaviour, Iclickable
{
    public enemy_skillCard_slot _slot;

    public void OnClick() => _slot.OnClick(); 
}
