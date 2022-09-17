using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndPanel : Panel
{
    [SerializeField] private GameObject nextButton;

    private void Awake()
    {
        // startButton.onClick.AddListener(OnStartButtonPressed);
        EventTrigger myEventTrigger = nextButton.GetComponent<EventTrigger>(); //you need to have an EventTrigger component attached this gameObject
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener(OnNextButtonPressed);
        myEventTrigger.triggers.Add(entry);
    }

    
    private void OnNextButtonPressed(BaseEventData eventData)
    {
        HidePanel();
        gameManager.levelManager.GoNextLevel();
    }
}
