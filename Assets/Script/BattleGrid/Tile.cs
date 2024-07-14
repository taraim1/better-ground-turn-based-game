using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private Sprite _normal_sprite;
    [SerializeField] private Sprite _player_sprite;
    [SerializeField] private Sprite _enemy_sprite;

    private SpriteRenderer _renderer;
    public void set_grid(Grid grid) 
    { 
        _grid = grid;
    }
    public void set_pos(int x, int y)
    {
        var worldpos = _grid.GetCellCenterWorld(new Vector3Int(x, y));
        transform.position = worldpos;
    }

    public Vector3 get_pos() 
    {
        return transform.position;
    }

    public void change_sprite(BattleGridManager.boardCell cellType) 
    {
        switch (cellType) 
        {
            case BattleGridManager.boardCell.empty:
                _renderer.sprite = _normal_sprite;
                break;
            case BattleGridManager.boardCell.player:
                _renderer.sprite = _player_sprite;
                break;
            case BattleGridManager.boardCell.enemy:
                _renderer.sprite = _enemy_sprite;
                break;
        }
    }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
}
