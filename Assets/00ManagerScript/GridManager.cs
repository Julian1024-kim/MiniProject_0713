using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    // 깃허브 확인용 주석
    [Header("그리드 설정")]
    public int columns = 9;
    public int rows = 5;
    public float cellSize = 2;       // 칸 간격
    public Vector2 originOffset;        // 그리드 시작 위치

    [Header("셀 프리팹")]
    public GameObject cellPrefab;

    public Cell[,] grid;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        grid = new Cell[columns, rows];

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // 위치 계산
                Vector2 pos = new Vector2(x * cellSize + originOffset.x, y * cellSize + originOffset.y );

                // 셀 생성
                GameObject cellObj = Instantiate(cellPrefab, pos, Quaternion.identity);
                cellObj.name = $"Cell ({x},{y})";
                cellObj.transform.parent = transform;

                // 배열에 저장
                grid[x, y] = cellObj.GetComponent<Cell>();
            }
        }
    }
    public void ResetAllCells()
    {
        if (grid == null) return;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y].isOccupied = false;
                    grid[x, y].plantOnCell = null;
                }
            }
        }
    }
}
