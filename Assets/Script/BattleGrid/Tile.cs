using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour
{
    [SerializeField] private Grid _grid;


    public void set_grid(Grid grid) 
    { 
        _grid = grid;
    }
    public void set_pos(int x, int y)
    {
        var worldpos = _grid.GetCellCenterWorld(new Vector3Int(x, y));
        transform.position = worldpos;
    }

}
