using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayableCharacter : Character, Iclickable
{
    /* 
     * 필드 및 접근용 메소드들
     */

    private bool is_character_unlocked;
    public bool Is_character_unlocked { get { return is_character_unlocked; } }

    /* 
    * 메소드
    */

    public void OnClick() 
    {
        // 캐릭터의 패 보여줌
        CardManager.instance.clear_highlighted_card();
        CardManager.instance.Change_active_hand(Character_index);
        CardManager.instance.Set_origin_order(CardManager.instance.active_index);

        // 드래그 감지 시작
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
        // 이동 가능한 칸들을 원래 색으로 표시
        foreach (coordinate coordinate in current_movable_tiles)
        {
            BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.original);
        }

        // 현재 칸을 원래 색으로 표시
        BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.original);

        // 가장 가까운 빈 칸 좌표를 찾음 (원래 칸 포함)
        coordinate nearest_tile = BattleGridManager.instance.get_nearest_tile(gameObject.transform.position, moveFilter, current_movable_tiles);

        // 그 칸을 캐릭터 칸으로
        BattleGridManager.instance.set_tile_type(nearest_tile, BattleGridManager.boardCell.player);

        // 현재 칸 변경 (원래 칸과 다르면 이동 불가 상태로)
        if (coordinate != nearest_tile)
        {
            isMovable = false;
        }
        coordinate = nearest_tile;

        ActionManager.character_drag_ended?.Invoke();
    }

    // 드래그 감지
    private IEnumerator detect_drag_start()
    {
        float dragging_time = 0;

        if (isPanic) { yield break; } // 패닉이면 멈춤
        if (!isMovable) { yield break; } // 이동 불가면 멈춤

        while (true)
        {
            // 마우스 떼면 멈춤 (프레임이 안 맞을 수 있어서 getmousebutton으로 함)
            if (!Input.GetMouseButton(0))
            {
                yield break;
            }

            // 마우스를 안 뗀 상태로 일정 시간이 지나면 드래그 기능 시작
            if (dragging_time >= Util.drag_time_standard)
            {
                // 이동 가능한 칸 갱신
                current_movable_tiles = get_movable_tiles();
                current_movable_tiles.Add(coordinate);

                // 이동 가능한 칸들을 초록색으로 표시
                foreach (coordinate coordinate in current_movable_tiles)
                {
                    BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.green);
                }


                // 현재 칸을 빈 칸으로 만들고 초록색으로
                BattleGridManager.instance.set_tile_type(coordinate, BattleGridManager.boardCell.empty);
                BattleGridManager.instance.set_tile_color(coordinate, Tile.TileColor.green);
                ActionManager.character_drag_started?.Invoke();


                // 드래그 시작
                StartCoroutine(Ondrag());

                yield break;
            }

            dragging_time += 0.01f;
            yield return new WaitForSeconds(0.01f);

        }
    }

    // 드래그 해제 감지
    private IEnumerator Ondrag()
    {
        while (true)
        {
            // 마우스 포인터에 가장 가까운 타일로 이동 (이동할 수 있는 칸들 중에서)
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            coordinate nearest_tile = BattleGridManager.instance.get_nearest_tile(mousePos, moveFilter, current_movable_tiles);
            var tileCoor = BattleGridManager.instance.get_tile_pos(nearest_tile);
            transform.position = new Vector3(tileCoor[0], tileCoor[1], transform.position.z);

            // 드래그 해제 감지
            if (!Input.GetMouseButton(0))
            {
                On_drag_end();
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
