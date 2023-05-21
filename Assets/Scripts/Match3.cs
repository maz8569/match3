using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

// Is responsible for Match3 logic
public class Match3 : MonoBehaviour
{

    public class OnNewItemGridSpawnedEventArgs : EventArgs
    {
        public ItemGrid itemGrid;
        public ItemGridPosition itemGridPosition;
    }

    public class OnLevelSetEventArgs : EventArgs
    {
        public LevelSO levelSO;
        public BaseGrid<ItemGridPosition> grid;
    }

    public float cellSize = 0.5f;
    public float cellDistance = 0.1f;

    public event EventHandler OnWin;
    public event EventHandler OnItemPositionDestroyed;
    public event EventHandler<OnNewItemGridSpawnedEventArgs> OnNewItemGridSpawned;
    public event EventHandler<OnLevelSetEventArgs> OnLevelSet;
    public event EventHandler OnMove;
    public event EventHandler OnScoreChanged;
    public event EventHandler OnNewItemChanged;

    [SerializeField] private LevelSO levelSO;

    private int gridWidth;
    private int gridHeight;
    private BaseGrid<ItemGridPosition> grid;
    private int score;

    private RecipeSO currentRecipe;

    public List<int2> chosenItemsPos;
    public Dictionary<ItemSO, int> chosenItems;

    private void Awake()
    {
        chosenItemsPos = new List<int2>();
        chosenItems = new Dictionary<ItemSO, int>();
    }

    private void Start()
    {
        SetLevelSO(levelSO);
    }

    public LevelSO GetLevelSO()
    {
        return levelSO;
    }

    public void SetLevelSO(LevelSO levelSO)
    {
        var random = new System.Random();
        this.levelSO = levelSO;

        gridWidth = levelSO.width; 
        gridHeight = levelSO.height;
        cellSize = levelSO.cellSize;
        cellDistance = levelSO.cellDistance;

        grid = new BaseGrid<ItemGridPosition>(gridWidth, gridHeight, cellSize, Vector3.zero, (BaseGrid<ItemGridPosition> g, int x, int y) => new ItemGridPosition(g, x, y));

        for(int x = 0; x < gridWidth; x++)
        {
            for(int y = 0; y < gridHeight; y++)
            {
                ItemSO item = levelSO.items[random.Next(levelSO.items.Count)];
                ItemGrid itemGrid = new ItemGrid(item, x, y);
                grid.GetGridObject(x, y).SetItemGrid(itemGrid);
                //Debug.Log($"x = {x}, y = {y} " + item.name);
            }
        }

        score = 0;

        OnLevelSet?.Invoke(this, new OnLevelSetEventArgs { levelSO = levelSO, grid = grid });

    }

    private ItemSO GetItemSO(int x, int y)
    {
        if (!IsValidPosition(x, y)) return null;

        ItemGridPosition itemGridPosition = grid.GetGridObject(x, y);

        if(itemGridPosition == null) return null;

        return itemGridPosition.GetItemGrid().Item;
    }

    private bool IsValidPosition(int x, int y)
    {
        if (x < 0 || y < 0 ||
            x >= gridWidth || y >= gridHeight)
        {
            // Invalid position
            return false;
        }
        else
        {
            return true;
        }
    }

    public void PrintItemType(Vector3 pos)
    {
        ItemGridPosition temp = grid.GetGridObject(pos);
        
        if(temp == null) return;

        int2 itemPos = new int2(temp.X, temp.Y);

        if (!chosenItemsPos.Contains(itemPos))
        {
            chosenItemsPos.Add(new int2(temp.X, temp.Y));

            if (!chosenItems.ContainsKey(temp.GetItemGrid().Item))
            {
                chosenItems.Add(temp.GetItemGrid().Item, 1);
            }
            else
            {
                chosenItems[temp.GetItemGrid().Item] += 1;
            }

            CheckRecipe();

            OnNewItemChanged?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (chosenItemsPos.Count < 2) return;

        if (chosenItemsPos[^2].Equals(itemPos))
        {
            ItemSO item = GetItemSO(chosenItemsPos[^1].x, chosenItemsPos[^1].y);

            chosenItemsPos.RemoveAt(chosenItemsPos.Count - 1);

            chosenItems[item] -= 1;

            if(chosenItems[item] == 0)
            {
                chosenItems.Remove(item);
            }

            CheckRecipe();

            OnNewItemChanged?.Invoke(this, EventArgs.Empty);

        }

    }

    public RecipeSO GetLastChosen()
    {
        return currentRecipe;
    }

    public void ClearRecipe()
    {
        currentRecipe = null;
    }

    public Vector2 GetLastChosenItemPosition()
    {
        if (chosenItemsPos.Count == 0)
        {
            return new Vector2(-1, -1);
        }

        return grid.GetWorldPosition(chosenItemsPos[^1].x, chosenItemsPos[^1].y) + new Vector3(grid.CellSize, grid.CellSize) * 0.5f;
    }

    public int GetChosenItemsPositionCount()
    {
        return chosenItemsPos.Count;
    }

    public void ClearLists()
    {
        chosenItems.Clear();
        chosenItemsPos.Clear();

        OnMove?.Invoke(this, EventArgs.Empty);
    }

    public void DestroyChosenItems()
    {
        foreach(var item in chosenItemsPos)
        {
            grid.GetGridObject(item.x, item.y).DestroyItem();
            OnItemPositionDestroyed?.Invoke(grid.GetGridObject(item.x, item.y), EventArgs.Empty);
            grid.GetGridObject(item.x, item.y).ClearItemGrid();
        }
    }

    public void SpawnNewMissingItems()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                ItemGridPosition itemGridPosition = grid.GetGridObject(x, y);

                if (itemGridPosition.IsEmpty())
                {
                    ItemSO item = levelSO.items[UnityEngine.Random.Range(0, levelSO.items.Count)];
                    ItemGrid itemGrid = new ItemGrid(item, x, y);

                    itemGridPosition.SetItemGrid(itemGrid);

                    OnNewItemGridSpawned?.Invoke(itemGrid, new OnNewItemGridSpawnedEventArgs { itemGrid = itemGrid, itemGridPosition = itemGridPosition });
                }
            }
        }
    }

    public void CheckRecipe()
    {
        foreach(var recipe in levelSO.recipes)
        {
            var list = recipe.GetItemList();

            if (list.Count == chosenItems.Count && !list.Except(chosenItems.Keys).Any())
            {
                currentRecipe = recipe;
                return;
            }

        }
        currentRecipe = null;
        //Debug.Log(chosenItems.ToString());
    }

    public void FallItemsIntoEmpty()
    {
        for(int x = 0; x < gridWidth; x++)
        {
            for(int y = 0; y < gridHeight; y++)
            {
                ItemGridPosition itemGridPosition = grid.GetGridObject(x, y);

                if (!itemGridPosition.IsEmpty())
                {
                    for(int i = y - 1; i >= 0; i--)
                    {
                        ItemGridPosition nextItemGridPosition = grid.GetGridObject(x, i);
                        if (nextItemGridPosition.IsEmpty())
                        {
                            itemGridPosition.GetItemGrid().SetItemXY(x, i);
                            nextItemGridPosition.SetItemGrid(itemGridPosition.GetItemGrid());
                            itemGridPosition.ClearItemGrid();

                            itemGridPosition = nextItemGridPosition;
                        }
                        else
                        {
                            // Next Grid Position is not empty, stop looking
                            break;
                        }
                    }
                }
            }
        }
    }

}
