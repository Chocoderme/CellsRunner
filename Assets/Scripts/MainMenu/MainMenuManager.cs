using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    [Header("Player Count Settings")]
    [SerializeField] private int minPlayers = 2;
    [SerializeField] private int maxPlayers = 4;
    private int PlayerCount = 2;

    [Header("Objects")]
    [SerializeField] private TMPro.TextMeshProUGUI PlayerCountText;
    [SerializeField] private TMPro.TextMeshProUGUI ActualLevelText;
    [SerializeField] private UnityEngine.UI.Image ActualLevelImage;

    [Header("Panels")]
    [SerializeField] private GameObject playerSelectPanel;
    [SerializeField] private GameObject levelSelectPanel;

    [Header("Levels")]
    [SerializeField] private Level[] levels;
    private int levelIndex = 0;

    public void Start()
    {
        if (PlayerPrefs.HasKey("PlayerCount"))
        {
            PlayerCount = PlayerPrefs.GetInt("PlayerCount", minPlayers);
        }
        else
        {
            PlayerPrefs.SetInt("PlayerCount", PlayerCount);
            PlayerPrefs.Save();
        }

        ShowPlayerSelect();
    }

    public void AddPlayer()
    {
        if (PlayerCount < maxPlayers)
        {
            PlayerCount++;
            PlayerPrefs.SetInt("PlayerCount", PlayerCount);
            PlayerPrefs.Save();
        }
    }

    public void RemovePlayer()
    {
        if (PlayerCount > minPlayers)
        {
            PlayerCount--;
            PlayerPrefs.SetInt("PlayerCount", PlayerCount);
            PlayerPrefs.Save();
        }
    }

    public void NextLevel()
    {
        levelIndex++;
        if (levelIndex > levels.Length - 1)
            levelIndex = 0;
    }

    public void PreviousLevel()
    {
        levelIndex--;
        if (levelIndex < 0)
            levelIndex = levels.Length - 1;
    }

    public void ShowLevelSelect()
    {
        levelSelectPanel.SetActive(true);
        playerSelectPanel.SetActive(false);
    }

    public void ShowPlayerSelect()
    {
        playerSelectPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
    }

    public void LaunchGame()
    {
        Level levelToLoad = null;
        if (levels[levelIndex].index == -1)
        {
            var id = UnityEngine.Random.Range(0, levels.Length - 1);
            levelToLoad = levels[id];
        }
        else
        {
            levelToLoad = levels[levelIndex];
        }
        if (levelToLoad != null)
            StartCoroutine(DelayLoad(levelToLoad));
    }

    private IEnumerator DelayLoad(Level level)
    {
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(level.index, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (PlayerCountText != null)
            PlayerCountText.SetText(PlayerCount.ToString() + " Players");
        if (ActualLevelText != null && levels.Length > 0)
            ActualLevelText.SetText(levels[levelIndex].name);
        if (ActualLevelImage != null && levels.Length > 0)
            ActualLevelImage.sprite = levels[levelIndex].Image;
    }

    [System.Serializable]
    public class Level
    {
        public string name;
        public int index;
        public Sprite Image;
    }
}
