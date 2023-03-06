using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BarValuesManager : MonoBehaviour
{
    [SerializeField]private GameObject scrollView;
    [SerializeField]private List<GameObject> activeEffects;
    void Update()
    {
        for (int i = 0; i < activeEffects.Count; i++)
        {
            if (!scrollView.activeInHierarchy)
            {
               activeEffects[i].SetActive(false);
            }
        }
    }
}