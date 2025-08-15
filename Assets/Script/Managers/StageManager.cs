using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using Unity.PlasticSCM.Editor.WebApi;

public class StageManager : Singletone<StageManager>
{
    public int stage_index;
    // �������� ���� ������ ���� ��� ĳ���Ͱ� ��
    public List<Character> characters = new List<Character>();

    [SerializeField]
    private StageSettingSO stageSettingSO;

    [SerializeField]
    private GameObject loot_prefab; // ���� ���� ������ �� ���� ������ 

    private void Check_scene(Scene scene, LoadSceneMode loadSceneMode)
    {
        characters.Clear();
        if (scene.name == "Stage_show")
        {
            // ��� �ҷ�����
            Instantiate(stageSettingSO.stage_Settings[stage_index].background_prefab);

            // ĳ���� ����
            Reload_characters();
        }
    }

    // ���� �������� ���� �� �ʿ��� ���� ���� ��������
    public int get_holy_water_requirement() 
    {
        return stageSettingSO.stage_Settings[stage_index].holy_water_requirement;
    }

    // ĳ���� �����
    public void Reload_characters() 
    {

        foreach (Character character in characters) 
        {
            character.Kill();
        }
        characters.Clear();
        CharacterManager.instance.spawn_stage_show_character(stage_index);
    }

    // ���� ���� ��� �� ȭ�鿡 ǥ��
    public IEnumerator calc_and_show_battle_loot() 
    {
        // ������ ��ȭ ���� ����Ǵ� �κ�
        resource_code[] loots = stageSettingSO.stage_Settings[stage_index].loots;
        int[] minNum = stageSettingSO.stage_Settings[stage_index].minLootNumber;
        int[] maxNum = stageSettingSO.stage_Settings[stage_index].maxLootNumber;

        List<int> result = new List<int>();
        for (int i = 0; i < loots.Length; i++) 
        {
            resource_code code = loots[i];
            int current = ResourceManager.instance.GetResourceValue(code);

            result.Add(UnityEngine.Random.Range(minNum[i], maxNum[i] + 1));
            ResourceManager.instance.SetResourceValue(code, current + result[i]);
        }

        // ���â�� �ߴ� �κ�
        yield return new WaitForSeconds(0.5f);

        GameObject loots_layoutgroup = GameObject.Find("Battle Result Canvas").transform.Find("Loots LayoutGroup").gameObject;

        for (int i = 0; i < loots.Length; i++)
        {
            resource_code code = loots[i];
            GameObject lootObj = Instantiate(loot_prefab, loots_layoutgroup.transform);
            loot loot = lootObj.GetComponent<loot>();
            loot.Set_name(ResourceManager.instance.GetResourceName(code));
            loot.Set_quantity(result[i]);
            yield return new WaitForSeconds(0.25f);
        }
        yield break;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += Check_scene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Check_scene;
    }
}
