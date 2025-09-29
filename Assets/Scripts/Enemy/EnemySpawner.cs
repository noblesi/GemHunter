using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private GameObject enemySpawnTile;
    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private Transform parentTransform;
    [SerializeField]
    private GemCollector gemCollector;
    [SerializeField]
    private EntityBase target;

    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);
    private List<Vector3> possibleTiles = new List<Vector3>();
    private MemoryPool enemySpawnTilePool;
    private WaitForSeconds waitTime = new WaitForSeconds(2.0f);
    public static UnityEvent exitEvent = new UnityEvent();

    public static List<EntityBase> Enemies { get; private set; } = new List<EntityBase>();

    [System.Serializable]
    private struct WayPointData
    {
        public GameObject[] wayPoints;
    }
    [SerializeField]
    private WayPointData[] wayPointData;

    private void Awake()
    {
        enemySpawnTilePool = new MemoryPool(enemySpawnTile);

        tilemap.CompressBounds();

        CalculatePossibleTiles();
    }

    public void SpawnEnemys(int count)
    {
        Enemies.Clear();
        StartCoroutine(nameof(Process), count);
    }

    private IEnumerator Process(int count)
    {
        Vector3[] positions = new Vector3[count];
        for(int i = 0; i < count; ++i)
        {
            positions[i] = possibleTiles[Random.Range(0, possibleTiles.Count)];
            enemySpawnTilePool.ActivatePoolItem(positions[i]);
        }

        yield return waitTime;

        enemySpawnTilePool.DeactivateAllPoolItems();

        for(int i = 0; i < count; ++i)
        {
            int type = Random.Range(0, enemyPrefabs.Length);
            int wayIndex = Random.Range(0, wayPointData.Length);

            GameObject clone = Instantiate(enemyPrefabs[type], positions[i], Quaternion.identity, transform);
            clone.GetComponent<EnemyBase>().Initialize(this, parentTransform, gemCollector);
            clone.GetComponent<EnemyFSM>().SetUp(target, wayPointData[wayIndex].wayPoints);

            Enemies.Add(clone.GetComponent<EntityBase>());
        }
    }

    private void CalculatePossibleTiles()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        for(int y = 1; y < bounds.size.y - 1; ++y)
        {
            for(int x = 1; x < bounds.size.x - 1; ++x)
            {
                TileBase tile = allTiles[y * bounds.size.x + x];

                if(tile != null)
                {
                    Vector3Int localPosition = bounds.position + new Vector3Int(x, y);
                    Vector3 position = tilemap.CellToWorld(localPosition) + offset;
                    position.z = 0;

                    possibleTiles.Add(position);
                }
            }
        }
    }

    public void Deactivate(EntityBase enemy)
    {
        Enemies.Remove(enemy);
        Destroy(enemy.gameObject);

        if(Enemies.Count == 0)
        {
            exitEvent?.Invoke();
        }
    }
}
