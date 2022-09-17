using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelManager : MonoBehaviour
{
    private IGameManager gameManager;
    [Inject]
    public void Setup(IGameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public List<GameObject> levels;
    public GameObject lastLoadedLevel = null;
    public int activeLevelId = -1;

    private void Awake()
    {
        gameManager.levelManager = this;
    }
    private void Start()
    {
        activeLevelId = PlayerPrefs.GetInt("LastLoadedLevelID");
        LoadLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            GoNextLevel();
        }
    }

    public void GoNextLevel()
    {
        activeLevelId++;
        SaveLevel();
        LoadLevel();
    }

    public void LoadLevel()
    { 
        Destroy(lastLoadedLevel);
        lastLoadedLevel = Instantiate(levels[activeLevelId % levels.Count], Vector3.zero, Quaternion.identity); 
        gameManager.player.SetNewSpline(lastLoadedLevel.GetComponent<ILevel>().splineComputer);
    }

    void SaveLevel()
    {
        PlayerPrefs.SetInt("LastLoadedLevelID", activeLevelId);
    }

}
