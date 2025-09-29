using System;
using UnityEngine;
using UnityEngine.Events;

public class HeartSystem : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<int, int> onValueChangedHeart;
    [SerializeField]
    private UnityEvent<string> onValueChangedTimer;

    private int maxHeart;
    private int currentHeart;
    private float timer;
    private float refillTime;

    public float Timer
    {
        private set
        {
            timer = value;
            onValueChangedTimer.Invoke((CurrentHeart < maxHeart) ? $"{TimeSpan.FromSeconds(timer):mm\\:ss}" : "FULL");
        }
        get => timer;
    }

    public int CurrentHeart
    {
        private set
        {
            currentHeart = value;
            onValueChangedHeart.Invoke(currentHeart, maxHeart);
        }
        get => currentHeart;
    }

    private void Awake()
    {
        maxHeart = Database.DBItem.goods.maxHeart;
        refillTime = Database.DBItem.goods.heartRefillTime;

        LoadData();
    }

    private void Update()
    {
        if(CurrentHeart < maxHeart)
        {
            Timer -= Time.deltaTime;

            if(Timer <= 0f)
            {
                CurrentHeart++;

                if (currentHeart < maxHeart) Timer = refillTime;

                SaveData();
            }
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveData();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus) LoadData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public bool UseHeart(int count)
    {
        if(CurrentHeart >= count)
        {
            CurrentHeart -= count;

            if (CurrentHeart == maxHeart - count) Timer = refillTime;

            SaveData();

            return true;
        }

        return false;
    }

    private void SaveData()
    {
        Database.DBItem.goods.heart = CurrentHeart;
        Database.DBItem.goods.heartTimer = Timer;
        Database.DBItem.goods.heartLastTime = DateTime.UtcNow.ToBinary().ToString();
        Database.Write();
    }

    private void LoadData()
    {
        CurrentHeart = Database.DBItem.goods.heart;
        string lastTimeString = Database.DBItem.goods.heartLastTime;

        if (!string.IsNullOrEmpty(lastTimeString))
        {
            DateTime lastTime = DateTime.FromBinary(Convert.ToInt64(lastTimeString));

            float savedTimer = Database.DBItem.goods.heartTimer;
            float elapsed = (float)(DateTime.UtcNow - lastTime).TotalSeconds;
            float totalElapsed = elapsed + (refillTime - savedTimer);

            int heartToRecover = Mathf.FloorToInt((float)totalElapsed / refillTime);
            CurrentHeart = Mathf.Min(CurrentHeart + heartToRecover, maxHeart);

            if (CurrentHeart >= maxHeart) Timer = refillTime;
            else Timer = refillTime - (totalElapsed % refillTime);
        }
        else Timer = refillTime;
    }
}
