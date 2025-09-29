using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private ChapterData[] chapters;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private TextMeshPro textStageNumber;
    [SerializeField]
    private UIRewardResult uiRewardResult;
    [SerializeField]
    private PlayerBase player;
    [SerializeField]
    private float enemyCountScale = 0.15f;

    private int currentChapter;
    private int maxStage;
    private int currentStage = 0;
    private int baseEnemyCount = 10;

    private void Start()
    {
        currentChapter = PlayerPrefs.GetInt(Constants.ChapterIndex);
        maxStage = chapters[currentChapter].StageDataTable.maxStage;

        EnemySpawner.exitEvent.AddListener(SetupStage);

        SetupStage();
    }

    public void SetupStage()
    {
        currentStage++;

        if(currentStage > maxStage)
        {
            GameClear();
            return;
        }

        textStageNumber.text = $"STAGE {currentStage:D2}";

        enemySpawner.SpawnEnemys((int)(baseEnemyCount + currentStage * enemyCountScale));
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void GameClear()
    {
        SetTimeScale(0);

        long baseExp = (long)(currentStage * (5 + (currentChapter + 1) * 1.2f));
        long bonusExp = (long)Mathf.Pow(2, (currentChapter + 1)) * 100;
        long bonusGem = (currentChapter + 1) * 5000;
        bool isNewRecord = Database.DBItem.chapters[currentChapter].bestStage != maxStage;

        Database.DBItem.player.experience += (baseExp + bonusExp);
        Database.DBItem.goods.gem += (player.GEM + bonusGem);
        Database.DBItem.chapters[currentChapter].bestStage = maxStage;

        if (currentChapter + 1 < Database.DBItem.chapters.Length)
            Database.DBItem.chapters[currentChapter + 1].isUnlock = true;

        Database.Write();

        uiRewardResult.OnRewardResult(isNewRecord, true, currentChapter, maxStage,
            new (RewardType, long)[] { (RewardType.GEM, player.GEM), (RewardType.EXP, baseExp),
                                        (RewardType.GEM, bonusGem), (RewardType.EXP, bonusExp)});
    }

    public void GameOver()
    {
        SetTimeScale(0);

        long exp = (long)(currentStage * (5 + (currentChapter + 1) * 1.2f));
        bool isNewRecord = Database.DBItem.chapters[currentChapter].bestStage < currentStage;

        Database.DBItem.player.experience += exp;
        Database.DBItem.goods.gem += player.GEM;
        if (isNewRecord) Database.DBItem.chapters[currentChapter].bestStage = currentStage;

        Database.Write();

        uiRewardResult.OnRewardResult(isNewRecord, false, currentChapter, currentStage,
            new (RewardType, long)[] { (RewardType.GEM, player.GEM), (RewardType.EXP, exp) });
    }
}
