using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

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
    private List<Tile> _tiles = new List<Tile>();
    [SerializeField] private GameObject _cell_prefab;
    [SerializeField] private List<boardRow> _board;
    // 게임 판


    // 스테이지 보드 값 불러와서 세팅하는 메소드
    public void set_board(int stage_index) 
    {
        // 타일 초기화
        _tiles.Clear();

        // 보드 값 불러오기
        _board = _stageSO.stage_Settings[stage_index].board.ToList();

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

        // 칸 소환하기
        for (int y = 0; y < _board.Count; y++) 
        {
            for (int x = 0; x < _board[_board.Count - y - 1].row.Count; x++) 
            {
                if (get_cell(x, y) == boardCell.empty) 
                {
                    GameObject obj = Instantiate(_cell_prefab, _gridObj.transform);
                    Tile tile = obj.GetComponent<Tile>();
                    _tiles.Add(tile);
                    tile.set_grid(_grid);
                    tile.set_pos(x, y);
                }
            }
        }
    }


    // 좌표평면상의 좌표를 주면 리스트의 인덱스로 바꿔주는 메소드, 유효하지 않은 좌표면 (-1, -1) 반환
    private Tuple<int, int> convert_xy_to_index(int x, int y) 
    {
        // 좌표에 음수 들어있으면 안 됨
        if (x < 0 || y < 0) { return Tuple.Create(-1, -1); }

        // 리스트 인덱스 넘어가면 안 됨
        if (y >= _board.Count) { return Tuple.Create(-1, -1); }
        if (x >= _board[_board.Count - y - 1].row.Count) { return Tuple.Create(-1, -1); }

        // 값 반환
        return Tuple.Create(_board.Count - y - 1, x);
    }


    // 좌표 주면 해당하는 칸의 값 반환하는 메소드, 유효하지 않은 좌표면 NOT_cell 반환
    public boardCell get_cell(int x, int y) 
    {
        Tuple<int, int> cell_ind = convert_xy_to_index(x, y);

        if (cell_ind.Item1 == -1 && cell_ind.Item2 == -1) 
        {
            return boardCell.NOT_cell;
        }

        return _board[cell_ind.Item1].row[cell_ind.Item2];
    }



    // 그리드 오브젝트 찾는 메소드
    public void find_grid() 
    {
        _gridObj = GameObject.FindGameObjectWithTag("BattleGrid");
        _grid = _gridObj.GetComponent<Grid>();
    }


    private void Check_scene(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "Stage_show")
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
