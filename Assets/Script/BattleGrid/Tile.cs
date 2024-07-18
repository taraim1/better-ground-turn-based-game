using System;
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
    [SerializeField] private Sprite _green_sprite;

    private SpriteRenderer _renderer;
    private BattleGridManager.boardCell _cellType;
    private int _x, _y;
    private Sprite _origin_sprite;

    public enum TileColor 
    { 
        original,
        red,
        green
    }

    public void set_grid(Grid grid) 
    { 
        _grid = grid;
    }
    public void set_pos(int x, int y)
    {
        var worldpos = _grid.GetCellCenterWorld(new Vector3Int(x, y));
        transform.position = worldpos;
        _x = x;
        _y = y;
    }

    public Vector3 get_pos() 
    {
        return transform.position;
    }

    public Tuple<int, int> get_coordinate() 
    {
        return Tuple.Create(_x, _y);
    }

    public void set_type(BattleGridManager.boardCell cellType) 
    {
        _cellType = cellType;
        change_sprite(_cellType);
    }

    public BattleGridManager.boardCell get_type() 
    {
        return _cellType;
    }

    private void change_sprite(BattleGridManager.boardCell cellType) 
    {
        switch (cellType) 
        {
            case BattleGridManager.boardCell.empty:
                _renderer.sprite = _normal_sprite;
                _origin_sprite = _normal_sprite;
                break;
            case BattleGridManager.boardCell.player:
                _renderer.sprite = _player_sprite;
                _origin_sprite = _player_sprite;
                break;
            case BattleGridManager.boardCell.enemy:
                _renderer.sprite = _enemy_sprite;
                _origin_sprite = _enemy_sprite;
                break;
        }
    }

    public void set_color(TileColor color) 
    {
        switch (color) 
        {
            case TileColor.original:
                _renderer.sprite = _normal_sprite;
                break;
            case TileColor.red:
                _renderer.sprite = _enemy_sprite;
                break;
            case TileColor.green:
                _renderer.sprite = _green_sprite;
                break;
        }
    }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
}
