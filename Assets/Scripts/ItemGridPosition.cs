using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGridPosition
{

    private ItemGrid itemGrid;

    private BaseGrid<ItemGridPosition> grid;
    private int x, y;

    public ItemGridPosition(BaseGrid<ItemGridPosition> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void SetItemGrid(ItemGrid itemGrid)
    {
        this.itemGrid = itemGrid;
        grid.TriggerGridObjectChanged(x, y);
    }

    public int X => x;
    public int Y => y;

    public Vector3 GetWorldPosition()
    {
        return grid.GetWorldPosition(x, y);
    }

    public ItemGrid GetItemGrid()
    {
        return itemGrid;
    }

    public void ClearItemGrid()
    {
        itemGrid = null;
    }

    public void DestroyItem()
    {
        itemGrid?.Destroy();
        grid.TriggerGridObjectChanged(x, y);
    }

    public bool IsEmpty()
    {
        return itemGrid == null;
    }

    public override string ToString()
    {
        return itemGrid?.ToString();
    }

    public void SetMaterial(Material highlitedSpriteMaterial)
    {
        itemGrid?.SetMaterial(highlitedSpriteMaterial);
    }
}
