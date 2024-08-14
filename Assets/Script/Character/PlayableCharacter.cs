using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayableCharacter : Character, Iclickable
{
    /* 
     * �ʵ� �� ���ٿ� �޼ҵ��
     */

    private bool is_character_unlocked;
    public bool Is_character_unlocked { get { return is_character_unlocked; } }

    /* 
    * �޼ҵ�
    */

    public void OnClick() 
    {
        // ĳ������ �� ������
        CardManager.instance.clear_highlighted_card();
        CardManager.instance.Change_active_hand(Character_index);
        CardManager.instance.Set_origin_order(CardManager.instance.active_index);

        // �巡�� ���� ����
        StartCoroutine(detect_drag_start());
    }

    public override void Save_data()
    {
        base.Save_data();
        PlayableCharacterData playableData = DataSO.PlayerData[code];
        playableData.is_character_unlocked = is_character_unlocked;
    }

    public override void Load_data()
    {
        base.Load_data();
        PlayableCharacterData playableData = DataSO.PlayerData[code];
        is_character_unlocked = playableData.is_character_unlocked;
    }

    protected override void turn_start()
    {
        base.turn_start();
        isMovable = true;
    }

    public override bool check_enemy()
    {
        return false;
    }

    private void On_drag_end()
    {
        // �̵� ������ ĭ���� ���� ������ ǥ��
        foreach (coordinate coordinate in current_movable_tiles)
        {
            BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.original);
        }

        // ���� ĭ�� ���� ������ ǥ��
        BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.original);

        // ���� ����� �� ĭ ��ǥ�� ã�� (���� ĭ ����)
        coordinate nearest_tile = BattleGridManager.instance.get_nearest_tile(gameObject.transform.position, moveFilter, current_movable_tiles);

        // �� ĭ�� ĳ���� ĭ����
        BattleGridManager.instance.set_tile_type(nearest_tile, BattleGridManager.boardCell.player);

        // ���� ĭ ���� (���� ĭ�� �ٸ��� �̵� �Ұ� ���·�)
        if (coordinate != nearest_tile)
        {
            isMovable = false;
        }
        coordinate = nearest_tile;

        ActionManager.character_drag_ended?.Invoke();
    }

    // �巡�� ����
    private IEnumerator detect_drag_start()
    {
        float dragging_time = 0;

        if (isPanic) { yield break; } // �д��̸� ����
        if (!isMovable) { yield break; } // �̵� �Ұ��� ����

        while (true)
        {
            // ���콺 ���� ���� (�������� �� ���� �� �־ getmousebutton���� ��)
            if (!Input.GetMouseButton(0))
            {
                yield break;
            }

            // ���콺�� �� �� ���·� ���� �ð��� ������ �巡�� ��� ����
            if (dragging_time >= Util.drag_time_standard)
            {
                // �̵� ������ ĭ ����
                current_movable_tiles = get_movable_tiles();
                current_movable_tiles.Add(coordinate);

                // �̵� ������ ĭ���� �ʷϻ����� ǥ��
                foreach (coordinate coordinate in current_movable_tiles)
                {
                    BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.green);
                }


                // ���� ĭ�� �� ĭ���� ����� �ʷϻ�����
                BattleGridManager.instance.set_tile_type(coordinate, BattleGridManager.boardCell.empty);
                BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.green);
                ActionManager.character_drag_started?.Invoke();


                // �巡�� ����
                StartCoroutine(Ondrag());

                yield break;
            }

            dragging_time += 0.01f;
            yield return new WaitForSeconds(0.01f);

        }
    }

    // �巡�� ���� ����
    private IEnumerator Ondrag()
    {
        while (true)
        {
            // ���콺 �����Ϳ� ���� ����� Ÿ�Ϸ� �̵� (�̵��� �� �ִ� ĭ�� �߿���)
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            coordinate nearest_tile = BattleGridManager.instance.get_nearest_tile(mousePos, moveFilter, current_movable_tiles);
            var tileCoor = BattleGridManager.instance.get_tile_pos(nearest_tile);
            transform.position = new Vector3(tileCoor[0], tileCoor[1], transform.position.z);

            // �巡�� ���� ����
            if (!Input.GetMouseButton(0))
            {
                On_drag_end();
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
