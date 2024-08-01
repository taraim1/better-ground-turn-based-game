using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_base : MonoBehaviour
{
    [SerializeField] private GameObject SkillSlotCanvas;
    [SerializeField] private GameObject CharacterCanvas;
    [SerializeField] private GameObject MasterLayoutGroup;
    [SerializeField] private GameObject effectLayoutGroup;

    public enum location 
    { 
        Base,
        Skill_slot,
        Middle_canvas,
        Bottom_layoutGroup,
        effect_layoutGroup
    }

    // ĳ���� ������Ʈ�� �ٸ� UI ���̱�
    public GameObject Attach(location location, GameObject obj) 
    {
        GameObject parent;

        switch (location) 
        {
            case location.Skill_slot:
                parent = SkillSlotCanvas;
                break;
            case location.Middle_canvas:
                parent = CharacterCanvas;
                break;
            case location.Bottom_layoutGroup:
                parent = MasterLayoutGroup;
                break;
            case location.effect_layoutGroup:
                parent = effectLayoutGroup;
                break;
            default:
                parent = gameObject;
                break;
        }

        return Instantiate(obj, parent.transform);
    }

}
