using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static LevelSO;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private LevelSO levelSO;
    [SerializeField] private Transform pfItemGridVisual;
    [SerializeField] private Transform pfBackgroundGridVisual;
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TouchManager touchManager;

    private BaseGrid<GridPosition> grid;

    public ItemSO currentItem;


    private void OnEnable()
    {
        touchManager.OnDrag += TouchManager_OnDrag;
    }

    private void OnDisable()
    {
        touchManager.OnDrag -= TouchManager_OnDrag;
    }

    private void TouchManager_OnDrag(object sender, Vector3 touchPosition)
    {
        grid.GetXY(touchPosition, out int x, out int y);

        Debug.Log($"Tap at x = {x}, y = {y}. Changing to {currentItem.ItemName}");

        if(IsValidPosition(x, y))
        {
            grid.GetGridObject(x, y).SetItemSO(currentItem);
        }
    }

    private void Awake()
    {
        grid = new BaseGrid<GridPosition>(levelSO.width, levelSO.height, levelSO.cellSize, Vector3.zero, (BaseGrid<GridPosition> g, int x, int y) => new GridPosition(levelSO, g, x, y));

        levelName.text = levelSO.name;

        if(levelSO.levelGridPositions == null || levelSO.levelGridPositions.Count != levelSO.width * levelSO.height)
        {
            Debug.Log("Creating new level...");

            levelSO.levelGridPositions = new List<LevelSO.LevelGridPosition>();

            for(int x = 0; x < grid.Width; x++)
            {
                for(int y = 0; y < grid.Height; y++)
                {
                    ItemSO item = levelSO.items[Random.Range(0, levelSO.items.Count)];

                    LevelSO.LevelGridPosition levelGridPosition = new LevelSO.LevelGridPosition { x = x, y = y, itemSO = item };
                    levelSO.levelGridPositions.Add(levelGridPosition);

                    CreateVisual(grid.GetGridObject(x, y), levelGridPosition);
                }
            }
        }
        else
        {
            Debug.Log("Loading level...");
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    LevelSO.LevelGridPosition levelGridPosition = null;

                    foreach (LevelSO.LevelGridPosition tmpLevelGridPosition in levelSO.levelGridPositions)
                    {
                        if (tmpLevelGridPosition.x == x && tmpLevelGridPosition.y == y)
                        {
                            levelGridPosition = tmpLevelGridPosition;
                            break;
                        }
                    }

                    if (levelGridPosition == null)
                    {
                        Debug.LogError("Error! Null!");
                    }

                    CreateVisual(grid.GetGridObject(x, y), levelGridPosition);
                }
            }
        }

    }

    private void CreateVisual(GridPosition gridPosition, LevelSO.LevelGridPosition levelGridPosition)
    {
        Transform itemGridVisualTransform = Instantiate(pfItemGridVisual, gridPosition.GetWorldPosition(), Quaternion.identity);
        Transform bgGridVisualTransform = Instantiate(pfBackgroundGridVisual, gridPosition.GetWorldPosition(), Quaternion.identity);

        gridPosition.spriteRenderer = itemGridVisualTransform.GetChild(0).GetComponent<SpriteRenderer>();
        gridPosition.backgroundGameObject = bgGridVisualTransform.gameObject;
        gridPosition.levelGridPosition = levelGridPosition;

        gridPosition.SetItemSO(levelGridPosition.itemSO);
    }

    private bool IsValidPosition(int x, int y)
    {
        if (x < 0 || y < 0 ||
            x >= grid.Width || y >= grid.Height)
        {
            // Invalid position
            return false;
        }
        else
        {
            return true;
        }
    }

    public void SetCurrentItemSO(ItemSO itemSO)
    {
        currentItem = itemSO;
    }

    public LevelSO GetLevelSO()
    {
        return levelSO;
    }

    private class GridPosition
    {
        public SpriteRenderer spriteRenderer;
        public LevelSO.LevelGridPosition levelGridPosition;
        public GameObject backgroundGameObject;

        private LevelSO levelSO;
        private BaseGrid<GridPosition> grid;
        private int x;
        private int y;

        public GridPosition(LevelSO levelSO, BaseGrid<GridPosition> grid, int x, int y)
        {
            this.levelSO = levelSO;
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public Vector3 GetWorldPosition()
        {
            return grid.GetWorldPosition(x, y);
        }

        public void SetItemSO(ItemSO itemSO)
        {

            spriteRenderer.sprite = itemSO.Sprite;
            levelGridPosition.itemSO = itemSO;
            if (itemSO.ItemName.Equals("Blocked"))
            {
                backgroundGameObject.SetActive(false);
            }
            else
            {
                backgroundGameObject.SetActive(true);
            }

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(levelSO);
#endif
        }

    }

}
