using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyTopBar : MonoBehaviour
{
    [SerializeField]
    private LevelData levelData;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private Slider fillGuageEXP;
    [SerializeField]
    private TextMeshProUGUI textHeart;
    [SerializeField]
    private TextMeshProUGUI textHeartTimer;
    [SerializeField]
    private TextMeshProUGUI textGEMCount;

    private void Awake()
    {
        UpdateLevel();

        textGEMCount.text = NotateNumber.Transform((long)Database.DBItem.goods.gem);
    }

    private void UpdateLevel()
    {
        int level = Database.DBItem.player.level;

        while (Database.DBItem.player.experience >= levelData.MaxExperience[level - 1])
        {
            if (level > levelData.MaxExperience.Length) level = levelData.MaxExperience.Length;

            Database.DBItem.player.experience -= levelData.MaxExperience[level - 1];
            Database.DBItem.player.level++;
        }
        Database.Write();

        fillGuageEXP.value = Database.DBItem.player.experience / levelData.MaxExperience[level - 1];
        textLevel.text = Database.DBItem.player.level.ToString();
    }

    public void UpdateHeart(int current, int max)
    {
        textHeart.text = $"{current}/{max}";
    }

    public void UpdateHeartTimer(string text)
    {
        textHeartTimer.text = text;
    }
}
