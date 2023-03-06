using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public List<GameObject> uiElements;
    /// <summary>
    /// Active/Desactive all ButtonObjects
    /// </summary>
    public void SetActiveUiButtons(bool active)
    {
        for (int i = 0; i < uiElements.Count; i++)
        {
            uiElements[i].SetActive(active);
        }
    }
    /// <summary>
    /// Active/Desactive all ButtonObjects, else the objects index
    /// </summary>
    public void SetActiveUiButtons(bool active, int elseObjIndex)
    {
        for (int i = 0; i < uiElements.Count; i++)
        {
            if (i == elseObjIndex)
            {
                continue;
            }
            uiElements[i].SetActive(active);
        }
    }
    /// <summary>
    /// Active/Desactive all ButtonObjects, else the objects at list
    /// </summary>
    public void SetActiveUiButtons(bool active, List<int> elseObjIndex)
    {
        for (int i = 0; i < uiElements.Count; i++)
        {
            if (elseObjIndex.Contains(i))
            {
                continue;
            }
            uiElements[i].SetActive(active);
        }
    }
}