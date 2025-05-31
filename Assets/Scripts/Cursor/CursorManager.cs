using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CursorManager : MonoBehaviour
{
    public Sprite normal, tool, seed, item;

    private Sprite currentSprite;//儲存當前鼠標

    private Image currentImage;

    private RectTransform cursorCanvas;
    private void OnEnable()
    {
        EventHandler.ItemSelectEvent += OnItemSelectedEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectEvent -= OnItemSelectedEvent;
    }

    private void Start()
    {
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        currentImage = cursorCanvas.GetChild(0).GetComponent<Image>();

        currentSprite = normal;
        SetCursorImage(normal);
    }

    private void Update()
    {
        if (cursorCanvas == null) return;

        currentImage.transform.position = Input.mousePosition;

        if(!InteractWithUI())
        {
            SetCursorImage(currentSprite);
        }
        else
        {
            SetCursorImage(normal);
        }

    }
    /// <summary>
    /// 設置鼠標圖片
    /// </summary>
    /// <param name="sprite"></param>
    private void SetCursorImage(Sprite sprite)
    {
        currentImage.sprite = sprite;
        currentImage.color=new Color(1,1,1,1);

    }
    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        if (!isSelected)
        {
            currentSprite = normal;
        }
        else //物品被選中才切換圖片
        {
            //WORKFLOW: 添加所有類型對應圖片
            currentSprite = itemDetails.itemType switch
            {
                ItemType.Seed => seed,
                ItemType.Commodity => item,
                ItemType.CollectTool => tool,
                _ => normal
            };
        }

    }

    /// <summary>
    /// 是否于UI互動
    /// </summary>
    /// <returns></returns>
    private bool InteractWithUI()
    {
        if(EventSystem.current!=null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }
}
