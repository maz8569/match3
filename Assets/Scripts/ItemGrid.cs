using System;
using UnityEngine;

// Represents an Item object in the grid
public class ItemGrid
{
    public event EventHandler OnDestroyed;

    private ItemSO item;
    private int x, y;

    public ItemGrid(ItemSO item, int x, int y)
    {
        this.item = item;
        this.x = x;
        this.y = y;
    }

    public ItemSO Item { get => item; set => item = value; }

    public Vector3 GetWorldPosition()
    {
        return new Vector3(x, y);
    }

    public void SetItemXY(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void Destroy()
    {
        OnDestroyed?.Invoke(this, EventArgs.Empty);
    }

    public override string ToString()
    {
        return item.name;
    }
}
