using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class StartPanel : Panel
{ 
    [SerializeField] private GameObject startButton;
    public TextMeshProUGUI levelText;
    // Start is called before the first frame update
    private void Awake()
    {
        // startButton.onClick.AddListener(OnStartButtonPressed);
        EventTrigger myEventTrigger = startButton.GetComponent<EventTrigger>(); //you need to have an EventTrigger component attached this gameObject
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener(OnStartButtonPressed);
        myEventTrigger.triggers.Add(entry);
    }

    public void SetLevelText(int levelId) {
        levelText.text = "Level " + levelId;
    }
    private void OnStartButtonPressed(BaseEventData eventData)
    {
        HidePanel();
        gameManager.State = GameState.Play;
    }

    private void OnEnable()
    {
        SetLevelText(gameManager.levelManager.activeLevelId);
    }

}
