using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public const float itemFadeDuration = 0.35f;


    public const float targetAlpha = 0.1f;

    //时间相关
    public const float secondThreshold = 0.1f;

    public const int secondHold = 59;

    public const int minuteHold = 59;

    public const int hourHold = 23;

    public const int dayHold = 30;

    public const int seasonHold = 3;

    //Transition
    public const float fadeDuration = 0.3f;     //视频原本代码的设置
    
    public const float fadeInOutDuration = 0.3f;   // 淡入 & 淡出动画时间
    
    public const float fadeHoldDuration = 1.5f;    // 黑屏停留时间（不变）
}
