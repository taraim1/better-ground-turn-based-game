using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class MoveTree_RandomMove : MoveTree
    {
        public MoveTree_RandomMove(string name, EnemyCharacter character) : base(name, character) { }

        public override void Move()
        {
            if (!character.IsMovable) return;

            // 갈 수 있는 곳들 중 랜덤으로 움직임
            List<coordinate> movable_tiles = character.get_movable_tiles();
            int rand_index = Random.Range(0, movable_tiles.Count);
            BattleGridManager.instance.set_tile_type(character.Coordinate, BattleGridManager.boardCell.empty);
            character.Coordinate = movable_tiles[rand_index];
            BattleGridManager.instance.set_tile_type(character.Coordinate, BattleGridManager.boardCell.enemy);
            character.transform.position = BattleGridManager.instance.get_tile_pos(character.Coordinate);

        }
    }
}
