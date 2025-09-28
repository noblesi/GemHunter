using UnityEngine;

[CreateAssetMenu]
public class ChapterData : ScriptableObject
{
    [SerializeField]
    private ChapterDataTable chaterDataTable;
    [SerializeField]
    private StageDataTable stageDataTable;

    public ChapterDataTable ChapterDataTable => chaterDataTable;
    public StageDataTable StageDataTable => stageDataTable;
}

[System.Serializable]
public struct ChapterDataTable
{
    public Sprite spriteChapter;
    public Color colorChapter;
    public string chapterName;
}

[System.Serializable]
public struct StageDataTable
{
    public int maxStage;
    public int baseEnemyCount;
    public int baseEnemyLevel;
    public GameObject[] enemyPrefabs;
}
