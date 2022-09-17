using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadingBar : MonoBehaviour
{
    public Image fillingBar;

    public void FillTheBar(float value)
    {
        fillingBar.fillAmount = value;
    }
     
}
