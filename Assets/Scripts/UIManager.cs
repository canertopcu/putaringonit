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

    // Update is called once per frame
    void Update()
    {
        
    }
}
