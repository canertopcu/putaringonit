using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : APanel
{
    internal IGameManager gameManager;
    public override void HidePanel()
    {
        gameObject.SetActive(false);
    }

    public override void Initialize(IGameManager gameManager)
    {
        this.gameManager = gameManager;
    }
 
    public override void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
