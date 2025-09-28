﻿using UnityEngine;

public class LobbySceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject chapterIconPrefab;
    [SerializeField]
    private Transform parentContent;
    [SerializeField]
    private ChapterData[] allChapter;
    [SerializeField]
    private SwipeUI swipeUI;
    [SerializeField]
    private HeartSystem heartSystem;

    private void Awake()
    {
        for(int i = 0; i < allChapter.Length; ++i)
        {
            GameObject icon = Instantiate(chapterIconPrefab, parentContent);
            icon.GetComponent<UIChapterIcon>().SetUp(i, allChapter[i]);
        }
    }

    public void ButtonEvent_GameStart()
    {
        int index = swipeUI.CurrentPage;

        if (Database.DBItem.chapters[index].isUnlock == false)
        {
            Logger.Log("잠겨있는 챕터입니다.");
            return;
        }

        if (!heartSystem.UseHeart(5))
        {
            Logger.Log("하트가 부족합니다.");
            return;
        }

        PlayerPrefs.SetInt(Constants.ChapterIndex, index);

        SceneLoader.Instance.LoadScene(SceneName.Game);
    }
}
