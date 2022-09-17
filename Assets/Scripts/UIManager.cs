using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class UIManager : MonoBehaviour
{
    int activePanelIndex;
    public List<APanel> panels;

    private IGameManager gameManager;
    [Inject]
    public void Setup(IGameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    // Start is called before the first frame update
    void Awake()
    {
        gameManager.uiManager = this;

        foreach (var item in panels)
        {
            item.Initialize(gameManager);
            item.HidePanel();
        }
        activePanelIndex = 0;
        panels[activePanelIndex].ShowPanel();

    }

    internal void PassPanel()
    {
        foreach (var item in panels)
        {
            item.Initialize(gameManager);
            item.HidePanel();
        }
        activePanelIndex++;
        panels[activePanelIndex].ShowPanel();
    }

    public void ActivatePanel(Type pageType)
    { 
        foreach (var item in panels)
        {
            if (pageType == item.GetType())
            { 
                item.ShowPanel();
            }
            else
            {
                item.HidePanel();
            }
        }
    }
     
    private void OnEnable()
    {
        EventManager.Get<OnLevelLoaded>().AddListener(OnLevelLoaded);
        EventManager.Get<OnGameSuccessEvent>().AddListener(OnLevelSuccessEvent);
    }

    private void OnDisable()
    {
        EventManager.Get<OnLevelLoaded>().RemoveListener(OnLevelLoaded);
        EventManager.Get<OnGameSuccessEvent>().RemoveListener(OnLevelSuccessEvent);

    }

    private void OnLevelLoaded(int level)
    {
        ActivatePanel(typeof(StartPanel));
    }

    private void  OnLevelSuccessEvent() {
        ActivatePanel(typeof(EndPanel));
    }
}
