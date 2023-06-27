using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Match3Visual : MonoBehaviour
{
    public enum State
    {
        Busy,
        WaitingForUser,
    }

    public event EventHandler OnStateChanged;

    [SerializeField] private Transform pfItemGridVisual;
    [SerializeField] private Transform pfBackgroundGridVisual;
    [SerializeField] private Match3 match3;
    [SerializeField] private TouchManager touchManager;
    [SerializeField] private DiningHall _diningHall; //TODO: hotfix, check if couldn't be done better

    private BaseGrid<ItemGridPosition> grid;
    private Dictionary<ItemGrid, ItemGridVisual> itemGridDictionary;

    private bool isSetup; 
    private State state;
    private float busyTimer;
    private Action onBusyTimerElapsedAction;

    private void Awake()
    {
        state = State.Busy;

        match3.OnLevelSet += Match3_OnLevelSet;
    }

    private void Match3_OnLevelSet(object sender, Match3.OnLevelSetEventArgs e)
    {
        Setup(sender as Match3, e.grid);
    }

    public void Setup(Match3 match3, BaseGrid<ItemGridPosition> grid)
    {
        this.match3 = match3;
        this.grid = grid;

        match3.OnItemPositionDestroyed += ItemPositionDestroyed;
        match3.OnNewItemGridSpawned += ItemSpawned;

        itemGridDictionary = new Dictionary<ItemGrid, ItemGridVisual>();

        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                ItemGridPosition itemGridPosition = grid.GetGridObject(x, y);
                ItemGrid itemGrid = itemGridPosition.GetItemGrid();

                Vector3 position = grid.GetWorldPosition(x, y);
                position = new Vector3(position.x, position.y);

                // Visual Transform
                Transform itemGridVisualTransform = Instantiate(pfItemGridVisual, position, Quaternion.identity, transform.GetChild(0));
                itemGridVisualTransform.Find("sprite").GetComponent<SpriteRenderer>().sprite = itemGrid.Item.Sprite;
                float scale = match3.cellSize - match3.cellDistance * 2;
                itemGridVisualTransform.GetChild(0).localScale = new Vector3(scale, scale, scale);

                ItemGridVisual itemGridVisual = new ItemGridVisual(itemGridVisualTransform, itemGrid, grid.CellSize);

                itemGridDictionary[itemGrid] = itemGridVisual;
                if (itemGrid.Item.ItemName != "Blocked")
                {
                    // Background Grid Visual
                    Transform bgVisual = Instantiate(pfBackgroundGridVisual, grid.GetWorldPosition(x, y), Quaternion.identity, transform.GetChild(1));
                    scale *= 1.5f;
                    bgVisual.GetChild(0).localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    scale *= 1.5f;
                    itemGridVisualTransform.GetChild(0).localScale = new Vector3(scale, scale, scale);
                }

            }
        }

        isSetup = true;
    }

    private void ItemPositionDestroyed(object sender, System.EventArgs e)
    {
        if (sender is ItemGridPosition itemGridPosition && itemGridPosition.GetItemGrid() != null)
        {
            itemGridDictionary.Remove(itemGridPosition.GetItemGrid());
        }
    }

    private void ItemSpawned(object sender, Match3.OnNewItemGridSpawnedEventArgs e)
    {
        Vector3 position = e.itemGridPosition.GetWorldPosition();
        position = new Vector3(position.x, position.y + 0.5f);

        Transform itemGridVisualTransform = Instantiate(pfItemGridVisual, position, Quaternion.identity, transform.GetChild(0));
        itemGridVisualTransform.Find("sprite").GetComponent<SpriteRenderer>().sprite = e.itemGrid.Item.Sprite;
        float scale = match3.cellSize - match3.cellDistance * 2;
        itemGridVisualTransform.GetChild(0).localScale = new Vector3(scale, scale, scale);

        ItemGridVisual itemGridVisual = new ItemGridVisual(itemGridVisualTransform, e.itemGrid, grid.CellSize);

        itemGridDictionary[e.itemGrid] = itemGridVisual;

    }

    private void OnEnable()
    {
        touchManager.OnDrag += Drag;
        touchManager.OnDragCancelled += DragCancelled;
    }

    private void OnDisable()
    {
        touchManager.OnDrag -= Drag;
        touchManager.OnDragCancelled -= DragCancelled;
    }

    private void Drag(object sender, Vector3 position)
    {
        match3.PrintItemType(position);
    }

    private void DragCancelled(object sender, System.EventArgs e)
    {
        if(match3.GetChosenItemsPositionCount() > 2)
        {
            match3.CheckRecipe();
            if (match3.CurrentRecipe != null && _diningHall.GetWantedDishes().Contains(match3.CurrentRecipe))
            {
                match3.CalculateScore();
                match3.DestroyChosenItems();
                match3.FallItemsIntoEmpty();
                match3.SpawnNewMissingItems();
            }
        }
        else
        {
            match3.ClearRecipe();
        }

        match3.ClearLists();
    }

    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public State GetState()
    {
        return state;
    }

    private void Update()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach(ItemGrid itemGrid in itemGridDictionary.Keys)
        {
            itemGridDictionary[itemGrid].Update();
        }
    }

}
