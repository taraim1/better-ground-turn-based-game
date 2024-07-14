using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;

public class BattleGridManager : Singletone<BattleGridManager>
{
    

    public enum boardCell 
    { 

        empty,
        player,
        enemy,
        obstacle,
        NOT_cell // �ǿ� �� ���°� (����, �� �ٱ� ��)
    }


    [System.Serializable]
    public struct boardRow 
    {
        public List<boardCell> row;
    }

    [SerializeField] StageSettingSO _stageSO;
    private Grid _grid;
    private GameObject _gridObj;
    private List<List<Tile>> _tiles = new List<List<Tile>>(); // Ÿ�ϵ�, ��ĭ�� null ��
    [SerializeField] private GameObject _cell_prefab;
    [SerializeField] private List<boardRow> _board = new List<boardRow>(); // ���� ��



    // �������� ���� �� �ҷ��ͼ� �����ϴ� �޼ҵ�
    public void set_board(int stage_index) 
    {


        // ���� �� �ҷ�����
        _board.Clear();
        foreach (var row in _stageSO.stage_Settings[stage_index].board) 
        {
            boardRow tmp;
            tmp.row = new List<boardCell>();
            foreach (var cell in row.row) 
            { 
                tmp.row.Add(cell);
            }
            _board.Add(tmp);
        }

        // �׸��� ������Ʈ �̵���Ű��
        int max_col_count = 0;
        foreach (boardRow Row in _board) 
        {
            if (Row.row.Count > max_col_count) 
            { 
                max_col_count = Row.row.Count;
            }
        }
        _gridObj.transform.position = new Vector3(-0.5f * max_col_count, -0.5f * (_board.Count - 1), 0);

        // Ÿ�� ����Ʈ �ʱ�ȭ
        _tiles.Clear();
        for (int i = 0; i < _board.Count; i++) 
        {
            _tiles.Add(new List<Tile>());
        }



        // ĭ ��ȯ�ϱ�
        for (int y = 0; y < _board.Count; y++) 
        {
            for (int x = 0; x < _board[_board.Count - y - 1].row.Count; x++) 
            {
                boardCell cell = get_cell(x, y);

                if (cell == boardCell.empty)
                {
                    GameObject obj = Instantiate(_cell_prefab, _gridObj.transform);
                    Tile tile = obj.GetComponent<Tile>();
                    _tiles[_board.Count - y - 1].Add(tile);
                    tile.set_grid(_grid);
                    tile.set_pos(x, y);
                }
                else if (cell == boardCell.NOT_cell) 
                {
                    _tiles[_board.Count - y - 1].Add(null);
                }
            }
        }
    }


    // ��ǥ������ ��ǥ�� �ָ� ����Ʈ�� �ε����� �ٲ��ִ� �޼ҵ�, ��ȿ���� ���� ��ǥ�� (-1, -1) ��ȯ
    private Tuple<int, int> convert_xy_to_index(int x, int y) 
    {
        // ��ǥ�� ���� ��������� �� ��
        if (x < 0 || y < 0) { return Tuple.Create(-1, -1); }

        // ����Ʈ �ε��� �Ѿ�� �� ��
        if (y >= _board.Count) { return Tuple.Create(-1, -1); }
        if (x >= _board[_board.Count - y - 1].row.Count) { return Tuple.Create(-1, -1); }

        // �� ��ȯ
        return Tuple.Create(_board.Count - y - 1, x);
    }


    // ��ǥ �ָ� �ش��ϴ� ĭ�� �� ��ȯ�ϴ� �޼ҵ�, ��ȿ���� ���� ��ǥ�� NOT_cell ��ȯ
    public boardCell get_cell(int x, int y) 
    {
        Tuple<int, int> cell_ind = convert_xy_to_index(x, y);

        if (cell_ind.Item1 == -1 && cell_ind.Item2 == -1) 
        {
            return boardCell.NOT_cell;
        }

        return _board[cell_ind.Item1].row[cell_ind.Item2];
    }

    // ��ǥ �ָ� �ش� ĭ�� ����� ��ġ ��ȯ�ϴ� �޼ҵ�, ��ȿ���� ���� ��ǥ�� null ��ȯ
    public List<float> get_cell_pos(int x, int y) 
    {
        Tuple<int, int> cell_ind = convert_xy_to_index(x, y);
        if (cell_ind.Item1 == -1 && cell_ind.Item2 == -1)
        {
            return null;
        }

        Vector3 pos = _tiles[cell_ind.Item1][cell_ind.Item2].get_pos();
        return new List<float>() { pos.x, pos.y };
    }

    // ��ǥ���� �� �ٲٴ� �޼ҵ�, ��ȿ���� ���� ��ǥ�� �� �ٲ�
    public void change_cell(int x, int y, boardCell cellType) 
    { 
        Tuple<int, int> ind = convert_xy_to_index(x, y);
        if (ind.Item1 == -1 && ind.Item2 == -1) 
        {
            return;
        }

        _board[ind.Item1].row[ind.Item2] = cellType;
        _tiles[ind.Item1][ind.Item2].change_sprite(cellType);
    }

    // �׸��� ������Ʈ ã�� �޼ҵ�
    public void find_grid() 
    {
        _gridObj = GameObject.FindGameObjectWithTag("BattleGrid");
        _grid = _gridObj.GetComponent<Grid>();
    }


    private void Check_scene(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "Stage_show" || scene.name == "Battle")
        {
            find_grid();
            set_board(StageManager.instance.stage_index);
        }
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
