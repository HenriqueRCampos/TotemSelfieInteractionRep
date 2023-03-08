using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUIManager : MonoBehaviour
{
    public List<GameObject> uiButtons;
    [SerializeField] private GameObject ThemeScrollView;
    private SelfieManager selfieManager;

    private void Awake()
    {
        selfieManager = GetComponent<SelfieManager>();
    }
    private void Update()
    {
        if (selfieManager.webCam.isPlaying)
        {
            
        }
        else
        {

        }
    }
    private IEnumerator ShowUIElements()
    {

        yield return new WaitForSeconds(1f);
        
    }

    /// <summary>
    /// Active/Desactive all ButtonObjects
    /// </summary>
    public void SetActiveUiButtons(bool active)
    {
        for (int i = 0; i < uiButtons.Count; i++)
        {
            uiButtons[i].SetActive(active);
        }
    }
    /// <summary>
    /// Active/Desactive all ButtonObjects, else the objects index
    /// </summary>
    public void SetActiveUiButtons(bool active, int elseObjIndex)
    {
        for (int i = 0; i < uiButtons.Count; i++)
        {
            if (i == elseObjIndex)
            {
                continue;
            }
            uiButtons[i].SetActive(active);
        }
    }
    /// <summary>
    /// Active/Desactive all ButtonObjects, else the objects at list
    /// </summary>
    public void SetActiveUiButtons(bool active, List<int> elseObjIndex)
    {
        for (int i = 0; i < uiButtons.Count; i++)
        {
            if (elseObjIndex.Contains(i))
            {
                continue;
            }
            uiButtons[i].SetActive(active);
        }
    }
}