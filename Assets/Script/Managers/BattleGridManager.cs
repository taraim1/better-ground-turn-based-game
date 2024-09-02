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
        NOT_cell // 판에 안 들어가는곳 (구멍, 판 바깥 등)
    }


    [System.Serializable]
    public struct boardRow 
    {
        public List<boardCell> row;
    }

    [SerializeField] StageSettingSO _stageSO;
    private Grid _grid;
    private GameObject _gridObj;
    private List<List<Tile>> _tiles = new List<List<Tile>>(); // 타일들, 빈칸엔 null 들어감
    [SerializeField] private GameObject _cell_prefab;
    [SerializeField] private List<boardRow> _board = new List<boardRow>(); // 게임 판



    // 스테이지 보드 값 불러와서 세팅하는 메소드
    public void set_board(int stage_index) 
    {


        // 보드 값 불러오기
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

        // 그리드 오브젝트 이동시키기
        int max_col_count = 0;
        foreach (boardRow Row in _board) 
        {
            if (Row.row.Count > max_col_count) 
            { 
                max_col_count = Row.row.Count;
            }
        }
        _gridObj.transform.position = new Vector3(-0.5f * max_col_count, -0.5f * (_board.Count - 1), 0);

        // 타일 리스트 초기화
        _tiles.Clear();
        for (int i = 0; i < _board.Count; i++) 
        {
            _tiles.Add(new List<Tile>());
        }



        // 칸 소환하기
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


    // 좌표평면상의 좌표를 주면 리스트의 인덱스로 바꿔주는 메소드, 유효하지 않은 좌표면 (-1, -1) 반환
    private Tuple<int, int> convert_coordinate_to_index(coordinate coordinate) 
    {
        int x = coordinate.x;
        int y = coordinate.y;
        // 좌표에 음수 들어있으면 안 됨
        if (x < 0 || y < 0) { return Tuple.Create(-1, -1); }

        // 리스트 인덱스 넘어가면 안 됨
        if (y >= _board.Count) { return Tuple.Create(-1, -1); }
        if (x >= _board[_board.Count - y - 1].row.Count) { return Tuple.Create(-1, -1); }

        // 값 반환
        return Tuple.Create(_board.Count - y - 1, x);
    }


    // 좌표 주면 해당하는 칸의 값 반환하는 메소드, 유효하지 않은 좌표면 NOT_cell 반환
    public boardCell get_tile(coordinate coordinate) 
    {
        Tuple<int, int> cell_ind = convert_coordinate_to_index(coordinate);

        if (cell_ind.Item1 == -1 && cell_ind.Item2 == -1) 
        {
            return boardCell.NOT_cell;
        }

        return _board[cell_ind.Item1].row[cell_ind.Item2];
    }

    // 좌표 주면 해당 칸의 월드상 위치 반환하는 메소드, 유효하지 않은 좌표면 0, 0 반환
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

    // 좌표상의 타일 타입 바꾸는 메소드, 유효하지 않은 좌표면 안 바꿈
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

    // 좌표상의 타일 색 바꾸는 메소드, 유효하지 않은 좌표면 안 바꿈
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

    // 좌표상의 타일 색을 원래 색으로 바꾸는 메소드, 유효하지 않은 좌표면 안 바꿈
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



    // 월드 좌표와 가장 가까운 셀 찾아주는 메소드 (exclude_filter에 들어간 타입은 제외, include_filter에 들어간 칸이 있으면 그 칸 중에서만 찾음)
    public coordinate get_nearest_tile(Vector2 pos, List<boardCell> exclude_filter, List<coordinate> include_filter) 
    {
        coordinate min_coordinate = new coordinate();
        float min_distance = 1000000f;

        foreach (List<Tile> row in _tiles) 
        {
            foreach (Tile tile in row)
            { 
                // 빈 타일 or 예외 리스트에 있는 거 제외
                if (tile == null || exclude_filter.Contains(tile.get_type())) continue;

                // 필수 포함 리스트에 들어가있는지 검사
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

    // 그리드 오브젝트 찾는 메소드
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
        // 범위를 보여주는 중이었다면 원래 색으로
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
