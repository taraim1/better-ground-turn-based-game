using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    [SerializeField] private Grid _grid;


    private SpriteRenderer _renderer;
    private BattleGridManager.boardCell _cellType;
    private int _x, _y;
    private TileColor original_color = TileColor.white;

    public enum TileColor 
    { 
        white,
        red,
        green,
        blue
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

    public coordinate get_coordinate() 
    {
        return new coordinate(_x, _y);
    }

    public void set_type(BattleGridManager.boardCell cellType) 
    {
        _cellType = cellType;
        change_original_color_by_type(_cellType);
    }

    public BattleGridManager.boardCell get_type() 
    {
        return _cellType;
    }

    private void change_original_color_by_type(BattleGridManager.boardCell cellType) 
    {
        switch (cellType) 
        {
            case BattleGridManager.boardCell.empty:
                original_color = TileColor.white;
                change_color(TileColor.white);
                break;
            case BattleGridManager.boardCell.player:
                original_color = TileColor.blue;
                change_color(TileColor.blue);
                break;
            case BattleGridManager.boardCell.enemy:
                original_color = TileColor.red;
                change_color(TileColor.red);
                break;
            default:
                original_color = TileColor.white;
                break;
        }
    }

    public void change_color(TileColor color) 
    {
        switch (color) 
        {
            case TileColor.white:
                _renderer.color = new Color(1f, 1f, 1f, 1f);
                break;
            case TileColor.red:
                _renderer.color = new Color(1f, 0.4509f, 0.5294f, 1f);
                break;
            case TileColor.green:
                _renderer.color = new Color(0.5294f, 1f, 0.4509f, 1f);
                break;
            case TileColor.blue:
                _renderer.color = new Color(0.4509f, 0.5294f, 1f, 1f);
                break;
        }
    }

    public void revert_to_original_color() 
    {
        change_color(original_color);
    }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }
}
