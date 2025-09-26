using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private Transform parentTransform;
    [SerializeField]
    private GemCollector gemCollector;
    [SerializeField]
    private EntityBase target;
    [SerializeField]
    private int enemyCount = 10;

    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);
    private List<Vector3> possibleTiles = new List<Vector3>();

    public static List<EntityBase> Enemies { get; private set; } = new List<EntityBase>();

    private void Awake()
    {
        tilemap.CompressBounds();

        CalculatePossibleTiles();

        for(int i = 0; i< enemyCount; i++)
        {
            int type = Random.Range(0, enemyPrefabs.Length);
            int index = Random.Range(0, possibleTiles.Count);

            GameObject clone = Instantiate(enemyPrefabs[type], possibleTiles[index], Quaternion.identity, transform);
            clone.GetComponent<EnemyBase>().Initialize(this, parentTransform, gemCollector);
            clone.GetComponent<EnemyFSM>().SetUp(target);

            Enemies.Add(clone.GetComponent<EnemyBase>());
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
    }
}
