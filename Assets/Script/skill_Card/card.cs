using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEditor.ShaderGraph.Internal;

public class card : MonoBehaviour
{
    [SerializeField] SpriteRenderer illust;
    [SerializeField] TMP_Text nameTMP;
    [SerializeField] TMP_Text costTMP;
    [SerializeField] TMP_Text typeTMP;
    [SerializeField] TMP_Text behavior_typeTMP;
    [SerializeField] TMP_Text value_rangeTMP;

    // ī���� ���� ��ġ, ȸ��, �������� ����
    public PRS originPRS;

    public Cards Card;

    public void Setup(Cards card) 
    {
        Card = card;

        illust.sprite = Card.sprite;
        nameTMP.text = Card.name;
        costTMP.text = Card.cost.ToString();
        typeTMP.text = Card.type;
        behavior_typeTMP.text = Card.behavior_type;
        value_rangeTMP.text = string.Format("{0} - {1}", Card.minPowerOfLevel[0], Card.maxPowerOfLevel[0]);

    }

    // �־��� PRS�� Dotween ����� �̵� or �׳� �̵�
    public void MoveTransform(PRS prs, bool use_Dotween, float DotweenTime) 
    {
        if (use_Dotween) 
        {
            transform.DOMove(prs.pos, DotweenTime);
            transform.DORotateQuaternion(prs.rot, DotweenTime);
            transform.DOScale(prs.scale, DotweenTime);
            return;
        }

        transform.position = prs.pos;
        transform.rotation = prs.rot;
        transform.localScale = prs.scale;
    }
}
