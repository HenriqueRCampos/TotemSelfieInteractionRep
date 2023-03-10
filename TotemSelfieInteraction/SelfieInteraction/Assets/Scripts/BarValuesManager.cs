using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BarValuesManager : MonoBehaviour
{
    [SerializeField]private GameObject scrollView, themeView;
    [SerializeField]private List<GameObject> activeEffects, activeThemes;
    private GameUIManager gameUIManager;
    private void Awake()
    {
        gameUIManager = GameObject.Find("Canvas").GetComponent<GameUIManager>();
    }
    void Update()
    {
        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (!scrollView.activeInHierarchy)
            {
               activeEffects[i].SetActive(false);
            }
        }
        for (int i = 0; i < activeThemes.Count; i++)
        {
            if (!themeView.activeInHierarchy && gameUIManager.hideTheme)
            {
                activeThemes[i].SetActive(false);
            }
        }
    }
}