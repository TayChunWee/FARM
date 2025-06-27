using Farm.CropPlant;
using Farm.Map;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    public Sprite normal, tool, seed, item;

    private Sprite currentSprite;//儲存當前鼠標

    private Image cursorImage;

    private RectTransform cursorCanvas;

    //鼠标检测
    private Camera mainCamera;

    private Grid currentGrid;

    private Vector3 mouseWorldPos;

    private Vector3Int mouseGridPos;

    private bool cursorEnable;

    private bool cursorPositionValid;


    private ItemDetails currentItem;

    private Transform PlayerTransform => FindObjectOfType<Player>().transform;


    private void OnEnable()
    {
        EventHandler.ItemSelectEvent += OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadedEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadedEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
    }


    private void Start()
    {
        //先注釋掉因爲在build中順序不對
        //cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas").GetComponent<RectTransform>();
        //cursorImage = GameObject.Find("Cursor Image").GetComponent<Image>();
        //
        //currentSprite = normal;
        //SetCursorImage(normal);
        //
        //mainCamera = Camera.main;
    }

    private void Update()
    {
        if (cursorCanvas == null) return;

        cursorImage.transform.position = Input.mousePosition;

        if(!InteractWithUI() && cursorEnable)
        {
            SetCursorImage(currentSprite);
            CheckCursorValid();
            CheckPlayerInput();
        }
        else
        {
            SetCursorImage(normal);
        }

    }

    private void CheckPlayerInput()
    {
        if(Input.GetMouseButtonDown(0) && cursorPositionValid)
        {
            //执行方法
            EventHandler.CallMouseClickedEvent(mouseWorldPos, currentItem);
        }
    }

    
    private void OnBeforeSceneUnloadedEvent()
    {
        cursorEnable = false;
    }

    //先注釋掉因爲在build中順序不對，改成使用下面的
    //private void OnAfterSceneLoadedEvent()
    //{
    //    currentGrid = FindObjectOfType<Grid>();
    //    cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas")?.GetComponent<RectTransform>();
    //    if (cursorCanvas != null)
    //    {
    //        cursorImage = cursorCanvas.GetComponentInChildren<Image>();
    //    }
    //    else
    //    {
    //        Debug.LogWarning("CursorCanvas not found after scene load!");
    //    }
    //
    //}
    private void OnAfterSceneLoadedEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
        cursorCanvas = GameObject.FindGameObjectWithTag("CursorCanvas")?.GetComponent<RectTransform>();
        if (cursorCanvas != null)
        {
            cursorImage = cursorCanvas.GetComponentInChildren<Image>();
            if (cursorImage == null) Debug.LogError("Cursor Image not found in CursorCanvas!");
        }
        else
        {
            Debug.LogError("CursorCanvas not found after scene load!");
        }

        currentSprite = normal;
        SetCursorImage(normal);

        mainCamera = Camera.main;
        if (mainCamera == null) Debug.LogError("Main Camera not found!");
    }
    #region 设置鼠标样式
    /// <summary>
    /// 設置鼠標圖片
    /// </summary>
    /// <param name="sprite"></param>
    private void SetCursorImage(Sprite sprite)
    {
        cursorImage.sprite = sprite;
        cursorImage.color=new Color(1,1,1,1);

    }
    /// <summary>
    /// 设置鼠标可用
    /// </summary>
    private void SetCursorValid()
    {
        cursorPositionValid = true;
        cursorImage.color = new Color(1, 1, 1, 1);
    }
    /// <summary>
    /// 设置鼠标不可用
    /// </summary>
    private void SetCursorInvalid()
    {
        cursorPositionValid = false;
        cursorImage.color = new Color(1, 0, 0, 0.4f);
    }
    #endregion

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {

        if (!isSelected)
        {
            currentItem = null;
            cursorEnable = false;
            currentSprite = normal;
        }
        else //物品被選中才切換圖片
        {
            currentItem = itemDetails;
            //WORKFLOW: 添加所有類型對應圖片
            currentSprite = itemDetails.itemType switch
            {
                ItemType.Seed => seed,
                ItemType.Commodity => item,
                ItemType.CollectTool => tool,
                ItemType.BreakTool => tool,
                ItemType.HoeTool => tool,
                ItemType.ChopTool => tool,
                ItemType.ReapTool => tool,
                ItemType.WaterTool => tool,
                _ => normal
            };
            cursorEnable= true;
        }

    }

    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);

        var playerGridPos = currentGrid.WorldToCell(PlayerTransform.position);

        //判断在使用范围内
        if(Mathf.Abs(mouseGridPos.x - playerGridPos.x) > currentItem.itemUseRadius || Mathf.Abs(mouseGridPos.y - playerGridPos.y) > currentItem.itemUseRadius)
        {
            SetCursorInvalid();
            return;
        }

        TileDetails currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPos);

        if (currentTile != null)
        {
            CropDetails currentCrop = CropManager.Instance.GetCropDetails(currentTile.seedItemID);

            switch (currentItem.itemType)
            {
                case ItemType.Seed:
                    if (currentTile.daysSinceDug > -1 && currentTile.seedItemID == -1) SetCursorValid(); else SetCursorInvalid();
                    break;
                case ItemType.Commodity:
                    if (currentTile.canDropItem && currentItem.canDropped) SetCursorValid(); else SetCursorInvalid();
                    break;
                case ItemType.HoeTool:
                    if (currentTile.canDig) SetCursorValid(); else SetCursorInvalid();
                    break;
                case ItemType.WaterTool:
                    if (currentTile.daysSinceDug >-1 && currentTile.daysSinceWatered == -1) SetCursorValid(); else SetCursorInvalid();
                    break;
                case ItemType.CollectTool:
                    if(currentCrop != null)
                    {
                        if(currentTile.growthDays>= currentCrop.TotalGrowthDays) SetCursorValid(); else SetCursorInvalid();
                    }
                    else
                    {
                        SetCursorInvalid();
                    }
                    break;
            }
        }
        else
        {
            SetCursorInvalid();
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
