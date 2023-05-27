using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorUI : MonoBehaviour
{
    public List<ItemSO> items;
    [SerializeField] private GameObject pfItemUI;
    [SerializeField] private LevelEditor levelEditor;

    private void Awake()
    {
        items.AddRange(levelEditor.GetLevelSO().items);

        int x = 0;
        int y = 0;

        foreach(var item in items)
        {
            GameObject itemBut = Instantiate(pfItemUI, transform.GetChild(1));
            itemBut.GetComponent<Button>().onClick.AddListener(() => levelEditor.SetCurrentItemSO(item));

            Vector2 position = new Vector2(-400 + x * 200, 1050 - y * 200);

            itemBut.GetComponent<RectTransform>().anchoredPosition = position;
            itemBut.transform.GetChild(0).GetComponent<Image>().sprite = item.Sprite;
            itemBut.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.ItemName;

            x++;

            if(x > 4)
            {
                x = 0;
                y++;
            }

        }    
    }
}
