using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

// Is responsible for Match3 logic
public class Match3 : MonoBehaviour, IDataPesristence
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
    public event EventHandler<Vector3> OnFingerMove;
    public event EventHandler OnItemPositionDestroyed;
    public event EventHandler<OnNewItemGridSpawnedEventArgs> OnNewItemGridSpawned;
    public event EventHandler<OnLevelSetEventArgs> OnLevelSet;
    public event EventHandler OnMove;
    public event EventHandler<OnScoreChangedEventArgs> OnScoreChanged;
    public event EventHandler OnNewItemChanged;

    [SerializeField] public LevelSO levelSO;

    private int gridWidth;
    private int gridHeight;
    private BaseGrid<ItemGridPosition> grid;
    public int score; //TODO: change back to private
    public int checkedStars = 0; //TODO: change back to private

    private RecipeSO currentRecipe;

    private List<int2> possibleMoves;
    private List<int2> chosenItemsPos;
    private Dictionary<ItemSO, int> chosenItems;

    private int levelNumber;

    public float offset = 0.33f;

    private void Awake()
    {
        possibleMoves = new List<int2>();
        chosenItemsPos = new List<int2>();
        chosenItems = new Dictionary<ItemSO, int>();
    }

    private void OnEnable()
    {
        var progressManager = FindObjectOfType<ProgressManager>();
        if(progressManager != null)
        {
            levelNumber = progressManager.currentDay;
            levelSO = progressManager.currentLevelScriptableObj;
        }
        else
        {
            levelNumber = 1;
        }

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
        ClearLists();

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

        OnFingerMove?.Invoke(this, pos);

        ItemGridPosition temp = grid.GetGridObject(pos, cellSize * offset - cellDistance);
        
        if(temp == null) return;
        if (temp.GetItemGrid() == null) return;

        int2 itemPos = new int2(temp.X, temp.Y);

        if (!IsMovePossible(itemPos)) return;

        if (!chosenItemsPos.Contains(itemPos) && temp.GetItemGrid()?.Item.name != "Blocked" && Time.timeScale != 0)
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
            //Debug.Log("SamePosition");
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
        OnMove?.Invoke(this, EventArgs.Empty); //CHECK

        chosenItems.Clear();
        chosenItemsPos.Clear();

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
        RunBoardCheck((ItemGridPosition itemGridPosition, int x, int y) =>
        {
            for (int i = y - 1; i >= 0; i--)
            {
                if (!CheckIsEmptyItem(ref itemGridPosition, x, i))
                {
                    break;
                }
            }

            return true;
        });

        RunBoardCheck((ItemGridPosition itemGridPosition, int x, int y) =>
        {
            for (int i = y - 1; i >= 0; i--)
            {
                ItemGridPosition nextItemGridPosition = grid.GetGridObject(x, y);

                if (!nextItemGridPosition.IsEmpty())
                {
                    CheckIsEmptyItem(ref itemGridPosition, x - 1, i);
                    CheckIsEmptyItem(ref itemGridPosition, x + 1, i);
                }
            }

            return true;
        });

        RunBoardCheck((ItemGridPosition itemGridPosition, int x, int y) =>
        {
            for (int i = y - 1; i >= 0; i--)
            {
                if (!CheckIsEmptyItem(ref itemGridPosition, x, i))
                {
                    break;
                }
            }

            return true;
        });
    }

    private void RunBoardCheck(Func<ItemGridPosition, int, int, bool> method)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                ItemGridPosition itemGridPosition = grid.GetGridObject(x, y);

                if (!itemGridPosition.IsEmpty() && itemGridPosition.GetItemGrid()?.Item.name != "Blocked")
                {
                    method(itemGridPosition, x, y);
                }
            }
        }
    }

    private bool CheckIsEmptyItem(ref ItemGridPosition itemGridPosition, int x, int y)
    {
        ItemGridPosition nextItemGridPosition = grid.GetGridObject(x, y);

        if (nextItemGridPosition.IsEmpty())
        {
            itemGridPosition.GetItemGrid().SetItemXY(x, y);
            nextItemGridPosition.SetItemGrid(itemGridPosition.GetItemGrid());
            itemGridPosition.ClearItemGrid();

            itemGridPosition = nextItemGridPosition;
            return true;
        }

        return false;
    }

    public void CalculateScore()
    {
        score += chosenItemsPos.Count - 2;

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

    public void LoadData(LevelData levelData)
    {

    }

    public void SaveData(ref LevelData levelData)
    {
        Debug.Log("saved");

        if (levelData.checkedStars.ContainsKey(levelNumber))
        {
            if (levelData.checkedStars[levelNumber] < checkedStars)
            {
                levelData.checkedStars[levelNumber] = checkedStars;
            }
        }
        else
        {
            levelData.checkedStars.Add(levelNumber, checkedStars);
        }
    }

    public Dictionary<ItemSO, int> GetSelectedItems()
    {
        return chosenItems;
    }
}
