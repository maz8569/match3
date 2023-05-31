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

    public class OnScoreChangedEventArgs : EventArgs
    {
        public int currentScore;
        public float progress;
        public int checkedStars;
    }

    public float cellSize = 0.5f;
    public float cellDistance = 0.1f;

    public event EventHandler OnWin;
    public event EventHandler OnItemPositionDestroyed;
    public event EventHandler<OnNewItemGridSpawnedEventArgs> OnNewItemGridSpawned;
    public event EventHandler<OnLevelSetEventArgs> OnLevelSet;
    public event EventHandler OnMove;
    public event EventHandler<OnScoreChangedEventArgs> OnScoreChanged;
    public event EventHandler OnNewItemChanged;

    [SerializeField] private LevelSO levelSO;

    private int gridWidth;
    private int gridHeight;
    private BaseGrid<ItemGridPosition> grid;
    private int score;
    private int checkedStars;

    private RecipeSO currentRecipe;

    private List<int2> possibleMoves;
    private List<int2> chosenItemsPos;
    private Dictionary<ItemSO, int> chosenItems;

    private void Awake()
    {
        possibleMoves = new List<int2>();
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
                ItemSO item = levelSO.levelGridPositions.Find(gridPos => gridPos.x == x && gridPos.y == y)?.itemSO;

                if(item.name == "Empty" || item == null) item = levelSO.items[UnityEngine.Random.Range(0, levelSO.items.Count)];

                ItemGrid itemGrid = new ItemGrid(item, x, y);
                grid.GetGridObject(x, y).SetItemGrid(itemGrid);
            }
        }

        score = 0;

        OnLevelSet?.Invoke(this, new OnLevelSetEventArgs { levelSO = levelSO, grid = grid });

        var temp = Time.realtimeSinceStartup;
        Debug.Log(CheckBoard());
        Debug.Log(("Time for CheckBoard: "+(Time.realtimeSinceStartup - temp).ToString("f6")));

    }

    /// <summary>
    /// Checks if there is at least one legal combination in the grid.
    /// </summary>
    private bool CheckBoard() //TODO: optimize (ignore already checked combinations)
    {
        bool retVal = false;

        for(int i = 0; i < gridHeight; i++){
            for(int j = 0; j < gridWidth; j++){

                chosenItems.Add(grid.GetGridObject(i, j).GetItemGrid().Item, 1);
                
                retVal |= CheckBoardHelper(i + 1, j, 2);
                retVal |= CheckBoardHelper(i, j + 1, 2);

                if(retVal)
                {
                    return true;
                }

                chosenItems.Clear();
                currentRecipe = null;
                
            }
        }

        return false;
    }

    /// <summary>
    /// Helper recurrency function for CheckBoard.
    /// </summary>
    /// <param name="x">The x-coordinate on the grid.</param>
    /// <param name="y">The y-coordinate on the grid.</param>
    /// <param name="depth">The current length of the chain.</param>
    private bool CheckBoardHelper(int x, int y, int depth)//TODO: merge with CheckBoard, check diagonally
    {
        bool retVal = false;

        if(!IsValidPosition(x, y))
        {
            return false;
        } 
        else 
        {
            AddItemToChosenList(grid.GetGridObject(x, y).GetItemGrid().Item);

            if(depth == 3)
            {
                CheckRecipe();

                RemoveItemFromChosenList(grid.GetGridObject(x, y).GetItemGrid().Item);

                if(currentRecipe != null)
                {
                    currentRecipe = null;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                retVal |= CheckBoardHelper(x + 1, y, depth + 1);
                retVal |= CheckBoardHelper(x, y + 1, depth + 1);

                RemoveItemFromChosenList(grid.GetGridObject(x, y).GetItemGrid().Item);
            }
        }

        return retVal;
    }

    private void RemoveItemFromChosenList(ItemSO item)
    {
        if(!chosenItems.ContainsKey(item))
        {
            return;
        }
        else if (chosenItems[item] > 1)
        {
            chosenItems[item] -= 1;
        }
        else
        {
            chosenItems.Remove(item);
        }
    }

    private void AddItemToChosenList(ItemSO item)
    {
        if (!chosenItems.ContainsKey(item))
        {
            chosenItems.Add(item, 1);
        }
        else
        {
            chosenItems[item] += 1;
        }
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
        ItemGridPosition temp = grid.GetGridObject(pos, cellSize * 0.5f - cellDistance);
        
        if(temp == null) return;
        if (temp.GetItemGrid() == null) return;

        int2 itemPos = new int2(temp.X, temp.Y);

        if (!IsMovePossible(itemPos)) return;

        if (!chosenItemsPos.Contains(itemPos))
        {
            chosenItemsPos.Add(new int2(temp.X, temp.Y));

            AddItemToChosenList(temp.GetItemGrid().Item);

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

    public bool IsMovePossible(int2 currentPos)
    {
        if (chosenItemsPos.Count == 0) return true;

        if (chosenItemsPos[^1].Equals(currentPos))
        {
            Debug.Log("SamePosition");
            return false;
        }

        UpdatePossibleMoves();

        return possibleMoves.Contains(currentPos);
    }

    public void UpdatePossibleMoves()
    {
        possibleMoves.Clear();

        possibleMoves.Add(chosenItemsPos[^1] + new int2(1, 0));
        possibleMoves.Add(chosenItemsPos[^1] + new int2(0, 1));
        possibleMoves.Add(chosenItemsPos[^1] + new int2(-1, 0));
        possibleMoves.Add(chosenItemsPos[^1] + new int2(0, -1));


        possibleMoves.Add(chosenItemsPos[^1] + new int2(1, 1));
        possibleMoves.Add(chosenItemsPos[^1] + new int2(1, -1));
        possibleMoves.Add(chosenItemsPos[^1] + new int2(-1, 1));
        possibleMoves.Add(chosenItemsPos[^1] + new int2(-1, -1));
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

                if (!itemGridPosition.IsEmpty() && itemGridPosition.GetItemGrid()?.Item.name != "Blocked")
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
                    }
                }
            }
        }
    }

    public void CalculateScore()
    {
        score += 30;

        float fillAmount = (float)score / levelSO.targetScore;
        checkedStars = 0;

        foreach (var score in levelSO.starsScore)
        {
            if(fillAmount >= score)
            {
                checkedStars++;
            }
        }

        OnScoreChanged?.Invoke(this, new OnScoreChangedEventArgs { currentScore = score, progress = fillAmount, checkedStars = checkedStars });
    }

}
