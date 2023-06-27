using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGridVisual
{
    private Transform transform;
    private ItemGrid itemGrid;
    private float cellSize;
    private SpriteRenderer spriteRenderer;

    public ItemGridVisual(Transform transform, ItemGrid itemGrid, float cellSize)
    {
        this.transform = transform;
        this.itemGrid = itemGrid;
        this.cellSize = cellSize;

        itemGrid.OnDestroyed += DestroyItemGrid;
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        itemGrid.OnMaterialChanged += ItemGrid_OnMaterialChanged;
    }

    private void ItemGrid_OnMaterialChanged(object sender, Material e)
    {
        spriteRenderer.material = e;
    }

    private void DestroyItemGrid(object sender, System.EventArgs e)
    {
        Object.Destroy(transform.gameObject, 0.1f);
    }

    public void Update()
    {
        Vector3 targetPosition = itemGrid.GetWorldPosition() * cellSize;
        Vector3 moveDir = (targetPosition - transform.position);
        float moveSpeed = 10f;
        transform.position += moveSpeed * Time.deltaTime * moveDir;
    }

}
