using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOverride : MonoBehaviour
{
    private Animator[] animators;

    public SpriteRenderer holdItem;

    [Header("各部分動畫列表")]
    public List<AnimatorType> animatorTypes;

    private Dictionary<string, Animator> animatorsNameDict = new Dictionary<string, Animator>();
    private void Awake()
    {
        animators = GetComponentsInChildren<Animator>();

        foreach(var anim in animators)
        {
            animatorsNameDict.Add(anim.name, anim);
        }
    }

    private void OnEnable()
    {
        EventHandler.ItemSelectEvent += OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadedEvent;
    }

    private void OnBeforeSceneUnloadedEvent()
    {
        holdItem.enabled = false;
        SwitchAnimator(PartType.None);
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails,bool isSelected)
    {
        //WORKFLOW:不同的工具返回不同的動畫可以在這裏補全
        PartType currentType = itemDetails.itemType switch
        {
            ItemType.Seed       => PartType.Carry,
            ItemType.Commodity  => PartType.Carry,
            ItemType.HoeTool    => PartType.Hoe,
            ItemType.WaterTool  => PartType.Water,
            ItemType.CollectTool => PartType.Collect,
            _                   => PartType.None
        };

        if(isSelected==false)
        {
            currentType = PartType.None;
            holdItem.enabled = false;
        }
        else
        {
            if(currentType == PartType.Carry)
            {
                holdItem.sprite = itemDetails.itemOnWorldSprite;
                holdItem.enabled = true;
            }
            else
            {
                holdItem.enabled = false;
            }
        }

        SwitchAnimator(currentType);
    }

    private void SwitchAnimator(PartType partType)
    {
        foreach(var item in animatorTypes)
        {
            if(item.partType == partType)
            {
                animatorsNameDict[item.partName.ToString()].runtimeAnimatorController =item.overrideController;  
            }
        }
    }
}
