using UnityEngine;

public abstract class APanel : MonoBehaviour
{
    public abstract void Initialize(IGameManager gameManager);
    public abstract void ShowPanel();
    public abstract void HidePanel();
 
}