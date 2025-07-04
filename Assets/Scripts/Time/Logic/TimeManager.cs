﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;

    private Season gameSeason = Season.春天;

    private int monthInSeason = 3;


    public bool gameClockPause;

    public float tikTime;


    private void Awake()
    {
        NewGameTime();
    }

    private void Start()
    {
        EventHandler.CallGameDataEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
        EventHandler.CallGameMinuteEvent(gameMinute, gameHour);
    }

    private void Update()
    {
        if (!gameClockPause)
        {
            tikTime += Time.deltaTime;

            if(tikTime >= Settings.secondThreshold)
            {
                tikTime -= Settings.secondThreshold;
                UpdateGameTime();
            }
        }

        if (Input.GetKey(KeyCode.T))
        {
            for(int i = 0; i < 60; i++)
            {
                UpdateGameTime();
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            gameDay++;
            EventHandler.CallGameDayEvent(gameDay, gameSeason);
            EventHandler.CallGameDataEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
        }
    }

    private void NewGameTime()
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 7;
        gameDay = 1;
        gameMonth = 1;
        gameYear = 2025;
        gameSeason = Season.春天;
    }

    private void UpdateGameTime()
    {
        gameSecond++;
        if(gameSecond > Settings.secondHold)
        {
            gameMinute++;
            gameSecond = 0;

            if(gameMinute > Settings.minuteHold)
            {
                gameHour++;
                gameMinute = 0;

                if(gameHour > Settings.hourHold)
                {
                    gameDay++;
                    gameHour = 0;

                    if(gameDay > Settings.dayHold)
                    {
                        gameDay = 1;
                        gameMonth++;

                        if(gameMonth > 12)
                        {
                            gameMonth = 1;
                        }
                        monthInSeason--;

                        if(monthInSeason == 0)
                        {
                            monthInSeason = 3;

                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;

                            if(seasonNumber > Settings.seasonHold)
                            {
                                seasonNumber = 0;
                                gameYear++;
                            }

                            gameSeason = (Season)seasonNumber;

                            if(gameYear > 9999)
                            {
                                gameYear = 2025;
                            }
                        }
                        //用来刷新地图和农作物生长
                        EventHandler.CallGameDayEvent(gameDay, gameSeason);
                    }
                }
                EventHandler.CallGameDataEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            }
            EventHandler.CallGameMinuteEvent(gameMinute, gameHour);
        }
        //Debug.Log("Second: " + gameSecond + "Minute: " + gameMinute);
    }
}

