using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameUIManager : MonoBehaviour
{
    [NonSerialized] public bool themeAplyed, hideTheme;
    public List<GameObject> uiButtons;
    public GameObject ThemeScrollView;
    [SerializeField] private Scrollbar themeScrollbar;
    private List<int> indexList;
    private Vector2 beginScale = new(0.1f, 0.1f);
    private Vector2 finalSacele = new(1f, 1f);

    private void Update()
    {
        for (int i = 0; i < uiButtons.Count; i++)
        {
            if (uiButtons[i].activeInHierarchy)
            {
                uiButtons[i].transform.localScale = Vector2.Lerp(uiButtons[i].transform.localScale, finalSacele, 0.01f);
            }
        }
    }
    public void ThemeView(bool active)
    {

        if (active)
        {
            themeScrollbar.value = 0f;
            ChooseButtonsToActive(new List<int>() { 2, 3 });
        }
        ThemeScrollView.SetActive(active);
    }
    public void AplyThemetoToTexture()
    {
        themeAplyed = true;
    }

    public void ChooseButtonsToActive()
    {
        for (int i = 0; i < uiButtons.Count; i++)
        {
            uiButtons[i].SetActive(false);
        }
    }
    public void ChooseButtonsToActive(int elseObjIndex)
    {
        List<int> list = new(){elseObjIndex};
        for (int i = 0; i < uiButtons.Count; i++)
        {
            uiButtons[i].SetActive(false);
            if (i == elseObjIndex)
            {
                indexList = list.GetRange(0, list.Count);
                StartCoroutine(DelayToActiveButtons());
            }
        }
    }
    public void ChooseButtonsToActive(List<int> elseObjIndex)
    {
        for (int i = 0; i < uiButtons.Count; i++)
        {
            uiButtons[i].SetActive(false);
            if (elseObjIndex.Contains(i))
            {
                indexList = elseObjIndex.GetRange(0, elseObjIndex.Count);
                StartCoroutine(DelayToActiveButtons());
            }
        }
    }
    private IEnumerator DelayToActiveButtons()
    {
        foreach (GameObject button in uiButtons)
        {
            button.transform.localScale = beginScale;
        }
        yield return new WaitForSeconds(1f);
        foreach (int index in indexList)
        {
            uiButtons[index].SetActive(true);
        }
    }
}