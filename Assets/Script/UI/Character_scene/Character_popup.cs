using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Character_popup : MonoBehaviour
{
    private character_code code;
    private Character character;
    [SerializeField] private Vector3 character_spawnpoint;
    [SerializeField] private Vector3 character_scale;
    [SerializeField] TMP_Text level_and_name_tmp;
    [SerializeField] TMP_Text description_tmp;
    [SerializeField] TMP_Text status_tmp;
    [SerializeField] private GameObject cell_prefab;
    [SerializeField] private GameObject move_range_display;
    [SerializeField] private GameObject deck_cell_prefab;
    [SerializeField] private GameObject deck_cell_layout_group;
    private List<List<GameObject>> rangeCells = new List<List<GameObject>>();
    private int range_display_cell_number_per_row = 7;

    public void Setup(character_code code) 
    {
        this.code = code;
        character = CharacterBuilder.instance.MakeSpumObj(true).Code(code).build();
        character.gameObject.transform.parent = transform;
        character.gameObject.transform.localPosition = character_spawnpoint;
        character.gameObject.transform.localScale = character_scale;
        level_and_name_tmp.text = string.Format("{0} Lv. {1}", character.Character_name, character.Level);
        description_tmp.text = string.Format("ĳ���� �Ұ�\n<style=\"nexonRegularOutline\"><size=\"48\">{0}</size></style>", character.Description);
        status_tmp.text = string.Format("�������ͽ�" +
            "<style=\"nexonRegularOutline\"><size=\"48\">\n" +
            "�ִ� ü��:</style> {0}<style=\"nexonRegularOutline\">\n" +
            "�ִ� ���ŷ�:</style> {1}\n</size>", character.get_max_health(), character.get_max_willpower());

        // �̵� ���� �� ��ȯ
        rangeCells.Clear();
        for (int row = 0; row < range_display_cell_number_per_row; row++) 
        {
            rangeCells.Add(new List<GameObject>());
            for (int col = 0; col < range_display_cell_number_per_row; col++) 
            {
                GameObject cell = Instantiate(cell_prefab, move_range_display.transform);
                rangeCells[row].Add(cell);
            }
        }

        // �̵� ���� �� ĥ�ϱ�
        foreach (coordinate coordinate in character.Move_range) 
        {
            int row = range_display_cell_number_per_row / 2 - coordinate.y;
            int col = range_display_cell_number_per_row / 2 + coordinate.x;

            // �ʷ�
            rangeCells[row][col].GetComponent<Image>().color = new Color(0.5294f, 1f, 0.4509f, 1f);
        }
        // �Ķ�
        rangeCells[range_display_cell_number_per_row / 2][range_display_cell_number_per_row / 2].GetComponent<Image>().color = new Color(0.4509f, 0.5294f, 1f, 1f);

        // �� �ҷ�����
        foreach (skillcard_code skill_code in character.Deck) 
        {
            deckCell deck_cell = Instantiate(deck_cell_prefab, deck_cell_layout_group.transform).GetComponent<deckCell>();
            deck_cell.Setup(skill_code);
        }
    }

    public void DestroyPopup()
    {
        rangeCells.Clear();
        Destroy(gameObject);
    }
}
