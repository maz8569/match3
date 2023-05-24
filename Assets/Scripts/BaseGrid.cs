using System;
using UnityEngine;

// Base class for generic grid
public class BaseGrid<TGridObject>
{
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;


    public BaseGrid(int width, int height, float cellSize, Vector3 originPosition, Func<BaseGrid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }
    }

    public int Width => width;
    public int Height => height;
    public float CellSize => cellSize;

    public Vector3 GetWorldPosition(int x, int y) => new Vector3(x, y) * cellSize + originPosition;

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void GetXY(Vector3 worldPosition, float cellDistance, out int x, out int y)
    {
        Vector3 cellWorld = worldPosition - originPosition;

        x = Mathf.FloorToInt(cellWorld.x / cellSize);
        y = Mathf.FloorToInt(cellWorld.y / cellSize);

        Vector3 temp = GetWorldPosition(x, y);

        float newCell = cellSize - cellDistance;

        if(temp.x + newCell < cellWorld.x || temp.x - newCell > cellWorld.x ||
            temp.y + newCell < cellWorld.y || temp.y - newCell > cellWorld.y)
        {
            x = -1;
            y = -1;
        }
    }

    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            TriggerGridObjectChanged(x, y);
        }
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y});
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        GetXY(worldPosition, out int x, out int y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default;
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition, float cellDistance)
    {
        GetXY(worldPosition, cellDistance, out int x, out int y);

        if (x == -1) return default;

        return GetGridObject(x, y);
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return GetGridObject(x, y);
    }

}
