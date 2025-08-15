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
using UnityEngine.SocialPlatforms;

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
                coordinate coordinate = new coordinate { x = x, y = y };
                boardCell cell = get_tile(coordinate);

                if (cell == boardCell.empty)
                {
                    GameObject obj = Instantiate(_cell_prefab, _gridObj.transform);
                    Tile tile = obj.GetComponent<Tile>();
                    _tiles[_board.Count - y - 1].Add(tile);
                    tile.set_grid(_grid);
                    tile.set_pos(x, y);
                    set_tile_type(coordinate, boardCell.empty);
                }
                else if (cell == boardCell.NOT_cell) 
                {
                    _tiles[_board.Count - y - 1].Add(null);
                }
            }
        }
    }


    // ��ǥ������ ��ǥ�� �ָ� ����Ʈ�� �ε����� �ٲ��ִ� �޼ҵ�, ��ȿ���� ���� ��ǥ�� (-1, -1) ��ȯ
    private Tuple<int, int> convert_coordinate_to_index(coordinate coordinate) 
    {
        int x = coordinate.x;
        int y = coordinate.y;
        // ��ǥ�� ���� ��������� �� ��
        if (x < 0 || y < 0) { return Tuple.Create(-1, -1); }

        // ����Ʈ �ε��� �Ѿ�� �� ��
        if (y >= _board.Count) { return Tuple.Create(-1, -1); }
        if (x >= _board[_board.Count - y - 1].row.Count) { return Tuple.Create(-1, -1); }

        // �� ��ȯ
        return Tuple.Create(_board.Count - y - 1, x);
    }


    // ��ǥ �ָ� �ش��ϴ� ĭ�� �� ��ȯ�ϴ� �޼ҵ�, ��ȿ���� ���� ��ǥ�� NOT_cell ��ȯ
    public boardCell get_tile(coordinate coordinate) 
    {
        Tuple<int, int> cell_ind = convert_coordinate_to_index(coordinate);

        if (cell_ind.Item1 == -1 && cell_ind.Item2 == -1) 
        {
            return boardCell.NOT_cell;
        }

        return _board[cell_ind.Item1].row[cell_ind.Item2];
    }

    // ��ǥ �ָ� �ش� ĭ�� ����� ��ġ ��ȯ�ϴ� �޼ҵ�, ��ȿ���� ���� ��ǥ�� 0, 0 ��ȯ
    public Vector2 get_tile_pos(coordinate coordinate) 
    {
        Tuple<int, int> cell_ind = convert_coordinate_to_index(coordinate);
        if (cell_ind.Item1 == -1 && cell_ind.Item2 == -1)
        {
            return new Vector2(0, 0);
        }

        Vector3 pos = _tiles[cell_ind.Item1][cell_ind.Item2].get_pos();
        return new Vector2(pos.x, pos.y);
    }

    // ��ǥ���� Ÿ�� Ÿ�� �ٲٴ� �޼ҵ�, ��ȿ���� ���� ��ǥ�� �� �ٲ�
    public void set_tile_type(coordinate coordinate, boardCell cellType) 
    { 
        Tuple<int, int> ind = convert_coordinate_to_index(coordinate);
        if (ind.Item1 == -1 && ind.Item2 == -1) 
        {
            return;
        }

        _board[ind.Item1].row[ind.Item2] = cellType;
        _tiles[ind.Item1][ind.Item2].set_type(cellType);
    }

    // ��ǥ���� Ÿ�� �� �ٲٴ� �޼ҵ�, ��ȿ���� ���� ��ǥ�� �� �ٲ�
    public void set_tile_color(coordinate coordinate, Tile.TileColor color)
    {
        Tuple<int, int> ind = convert_coordinate_to_index(coordinate);
        if (ind.Item1 == -1 && ind.Item2 == -1)
        {
            return;
        }

        if (_tiles[ind.Item1][ind.Item2] != null) 
        {
            _tiles[ind.Item1][ind.Item2].change_color(color);
        }
    }

    // ��ǥ���� Ÿ�� ���� ���� ������ �ٲٴ� �޼ҵ�, ��ȿ���� ���� ��ǥ�� �� �ٲ�
    public void revert_tile_color_to_original(coordinate coordinate)
    {
        Tuple<int, int> ind = convert_coordinate_to_index(coordinate);
        if (ind.Item1 == -1 && ind.Item2 == -1)
        {
            return;
        }

        if (_tiles[ind.Item1][ind.Item2] != null)
        {
            _tiles[ind.Item1][ind.Item2].revert_to_original_color();
        }
    }



    // ���� ��ǥ�� ���� ����� �� ã���ִ� �޼ҵ� (exclude_filter�� �� Ÿ���� ����, include_filter�� �� ĭ�� ������ �� ĭ �߿����� ã��)
    public coordinate get_nearest_tile(Vector2 pos, List<boardCell> exclude_filter, List<coordinate> include_filter) 
    {
        coordinate min_coordinate = new coordinate();
        float min_distance = 1000000f;

        foreach (List<Tile> row in _tiles) 
        {
            foreach (Tile tile in row)
            { 
                // �� Ÿ�� or ���� ����Ʈ�� �ִ� �� ����
                if (tile == null || exclude_filter.Contains(tile.get_type())) continue;

                // �ʼ� ���� ����Ʈ�� ���ִ��� �˻�
                if (include_filter != null) 
                {
                    if (!include_filter.Contains(tile.get_coordinate())) continue;
                }

                float distance = math.sqrt(math.pow(pos.x - tile.gameObject.transform.position.x, 2) + math.pow(pos.y - tile.gameObject.transform.position.y, 2));
                if (distance < min_distance) 
                {
                    min_distance = distance;
                    min_coordinate = tile.get_coordinate();
                }
            }
        }

        return min_coordinate;
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

    private void OnCardDestoryed(card card) 
    {
        // ������ �����ִ� ���̾��ٸ� ���� ������
        if (card._isShowingRange)
        {
            foreach (coordinate coordinate in card.usable_tiles)
            {
                set_tile_color(coordinate, Tile.TileColor.white);
            }
        }
    }

    private void OnCharacterDied(Character character) 
    {
        set_tile_type(character.Coordinate, boardCell.empty);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += Check_scene;
        ActionManager.card_destroyed += OnCardDestoryed;
        ActionManager.character_died += OnCharacterDied;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Check_scene;
        ActionManager.card_destroyed -= OnCardDestoryed;
        ActionManager.character_died -= OnCharacterDied;
    }
}
