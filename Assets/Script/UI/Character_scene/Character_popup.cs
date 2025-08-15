using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject character_deck_cell_prefab;
    [SerializeField] private GameObject card_collection_deck_cell_prefab;
    [SerializeField] private GameObject deck_cell_layout_group;
    [SerializeField] private GameObject skill_card_collection_layout_group;
    [SerializeField] private Skill_search skill_search;
    [SerializeField] private ContentsShowController contents_show_controller;
    [SerializeField] private Deck_changer deck_Changer;
    private List<List<GameObject>> rangeCells = new List<List<GameObject>>();
    private List<deckCell> Character_deckCells = new List<deckCell>();
    private int range_display_cell_number_per_row = 7;

    public void Setup(character_code code) 
    {
        this.code = code;
        character = CharacterBuilder.instance.MakeSpumObj(true).Code(code).build();
        character.gameObject.transform.parent = transform;
        character.gameObject.transform.localPosition = character_spawnpoint;
        character.gameObject.transform.localScale = character_scale;
        contents_show_controller.AddObj(0, character.gameObject);
        level_and_name_tmp.text = string.Format("{0} Lv. {1}", character.Character_name, character.Level);
        description_tmp.text = string.Format("캐릭터 소개\n<style=\"nexonRegularOutline\"><size=\"48\">{0}</size></style>", character.Description);
        status_tmp.text = string.Format("스테이터스" +
            "<style=\"nexonRegularOutline\"><size=\"48\">\n" +
            "최대 체력:</style> {0}<style=\"nexonRegularOutline\">\n" +
            "최대 정신력:</style> {1}\n</size>", character.get_max_health(), character.get_max_willpower());

        // 이동 범위 셀 소환
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

        // 이동 범위 셀 칠하기
        foreach (coordinate coordinate in character.Move_range) 
        {
            int row = range_display_cell_number_per_row / 2 - coordinate.y;
            int col = range_display_cell_number_per_row / 2 + coordinate.x;

            // 초록
            rangeCells[row][col].GetComponent<Image>().color = new Color(0.5294f, 1f, 0.4509f, 1f);
        }
        // 파랑
        rangeCells[range_display_cell_number_per_row / 2][range_display_cell_number_per_row / 2].GetComponent<Image>().color = new Color(0.4509f, 0.5294f, 1f, 1f);

        // 덱 불러오기
        foreach (var item in character.Deck.Select((code, index) => (code, index))) 
        {
            deckCell deck_cell = Instantiate(character_deck_cell_prefab, deck_cell_layout_group.transform).GetComponent<deckCell>();
            deck_cell.Setup(item.code);
            deckcell_character_deck_module deckModule = deck_cell.gameObject.GetComponent<deckcell_character_deck_module>();
            deckModule.Setup(deck_Changer, item.index);

            Character_deckCells.Add(deck_cell);
        }

        // 언락된 카드 불러오기
        skill_search.Reset();
        skill_search.unlocked(true);
        foreach (skillcard_code unlocked_code in skill_search.search()) 
        {
            deckCell card_cell = Instantiate(card_collection_deck_cell_prefab, skill_card_collection_layout_group.transform).GetComponent<deckCell>();
            card_cell.Setup(unlocked_code);
            deckCell_card_collection_module deckModule = card_cell.gameObject.GetComponent<deckCell_card_collection_module>();
            deckModule.Setup(deck_Changer, unlocked_code, this);
        }

        // 덱체인저 설정
        deck_Changer.set_character(character);
    }

    // 캐릭터 덱셀 갱신
    public void Update_character_deckCell() 
    {
        foreach (var item in character.Deck.Select((code, index) => (code, index)))
        {
            Character_deckCells[item.index].Setup(item.code);
        }
    }

    public void DestroyPopup()
    {
        rangeCells.Clear();
        Destroy(gameObject);
    }
}
