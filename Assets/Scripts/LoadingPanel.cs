using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : Panel
{
    public LoadingBar loadingBar;
    public Image fadePanel;
    public GameObject loadingPanel;
    private void OnEnable()
    {
        StartCoroutine(LoadBarAndPassStartScene());
    }

    private IEnumerator LoadBarAndPassStartScene()
    {
        
        fadePanel.gameObject.SetActive(false);
        for (int i = 0; i < 120; i++)
        {
            loadingBar.FillTheBar(i / 120f);
            yield return new WaitForEndOfFrame();
        }
        fadePanel.gameObject.SetActive(true);
        for (int i = 0; i < 120; i++)
        {
            Color c = fadePanel.color;
            c.a = i / 120f;
            fadePanel.color = c;
            yield return new WaitForEndOfFrame();
        }
        loadingPanel.SetActive(false);
        for (int i = 0; i < 60; i++)
        {
            Color c = fadePanel.color;
            c.a =1-(i / 60f);
            fadePanel.color = c;
            yield return new WaitForEndOfFrame();
        }
        fadePanel.gameObject.SetActive(false);

        gameManager.uiManager.PassPanel();
    }
}
