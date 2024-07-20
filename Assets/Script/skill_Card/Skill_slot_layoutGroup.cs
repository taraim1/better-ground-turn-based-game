using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_slot_layoutGroup : MonoBehaviour
{
    [SerializeField] private Character _character;
    void Awake()
    {
        _character.data.skill_layoutGroup = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
