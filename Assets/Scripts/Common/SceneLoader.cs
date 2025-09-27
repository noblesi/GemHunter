using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneName { Intro = 0, Lobby, Game }

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance {  get; private set; }

    [SerializeField]
    private GameObject loadingScreen;
    [SerializeField]
    private Image loadingBackground;
    [SerializeField]
    private Sprite[] loadingSprites;
    [SerializeField]
    private Slider loadingProgress;
    [SerializeField]
    private TextMeshProUGUI textProgress;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(string name)
    {
        int index = Random.Range(0, loadingSprites.Length);
        loadingBackground.sprite = loadingSprites[index];
        loadingProgress.value = 0f;
        loadingScreen.SetActive(true);

        StartCoroutine(LoadSceneAsync(name));
    }

    public void LoadScene(SceneName name)
    {
        LoadScene(name.ToString());
    }

    private IEnumerator LoadSceneAsync(string name)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);

        while(asyncOperation.isDone == false)
        {
            loadingProgress.value = asyncOperation.progress;
            textProgress.text = $"{Mathf.RoundToInt(asyncOperation.progress * 100)}%";

            yield return null;
        }

        float changeDelay = 0.5f;
        yield return new WaitForSeconds(changeDelay);

        loadingScreen.SetActive(false);
    }
}
