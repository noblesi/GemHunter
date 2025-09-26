using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{
    private class PoolItem
    {
        public GameObject gameObject;
        private bool isActive;

        public bool IsActive
        {
            set
            {
                isActive = value;
                gameObject.SetActive(isActive);
            }
            get => isActive;
        }
    }

    private GameObject poolObject;
    private List<PoolItem> poolItemList;

    private readonly int increaseCount = 5;

    public int MaxCount { private set; get; }
    public int ActiveCount { private set; get; }

    private Vector3 tempPosition = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

    public MemoryPool(GameObject poolObject)
    {
        MaxCount = 0;
        ActiveCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }

    public void InstantiateObjects()
    {
        MaxCount += increaseCount;

        for(int i = 0; i < increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.gameObject = GameObject.Instantiate(poolObject);
            poolItem.gameObject.transform.position = tempPosition;
            poolItem.IsActive = false;

            poolItemList.Add(poolItem);
        }
    }

    public void DestroyObjects()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for(int  i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }

        poolItemList.Clear();
    }

    public GameObject ActivatePoolItem(Vector3 position)
    {
        if (poolItemList == null) return null;

        if(MaxCount == ActiveCount)
        {
            InstantiateObjects();
        }

        int count = poolItemList.Count;
        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.IsActive == false)
            {
                ActiveCount++;

                poolItem.gameObject.transform.position = position;
                poolItem.IsActive = true;

                return poolItem.gameObject;
            }
        }

        return null;
    }

    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null) return;

        int count = poolItemList.Count;
        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject == removeObject)
            {
                ActiveCount--;

                poolItem.IsActive = false;
                poolItem.gameObject.transform.position = tempPosition;

                return;
            }
        }
    }
    
    public void DeactivateAllPollItems()
    {
        if (poolItemList == null) return;

        int count = poolItemList.Count;
        for(int i = 0; i < count; ++i)
        {
            PoolItem poolItem= poolItemList[i];

            if(poolItem.gameObject != null && poolItem.IsActive == true)
            {
                poolItem.IsActive = false;
                poolItem.gameObject.transform.position = tempPosition;
            }
        }

        ActiveCount = 0;
    }
}
