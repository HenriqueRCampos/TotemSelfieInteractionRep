using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectBars : MonoBehaviour
{
    [SerializeField] private GameObject scrollbar;
    [SerializeField] private List<GameObject> ShaderSliders, ThemesPormade;
    float scroll_pos = 0;
    float[] pos;
    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scroll_pos = scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                if (gameObject.name == "ContentTheme")
                {
                    transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1.2f, 1.2f), 0.05f);
                }else if (gameObject.name == "ContentShader")
                {
                    transform.GetChild(i).GetChild(1).localScale = Vector2.Lerp(transform.GetChild(i).GetChild(1).localScale, new Vector2(1.5f, 1.5f), 0.1f);
                }
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        if (gameObject.name == "ContentTheme")
                        {
                            transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.05f);
                        }
                        else if (gameObject.name == "ContentShader")
                        {
                            transform.GetChild(a).GetChild(1).localScale = Vector2.Lerp(transform.GetChild(a).GetChild(1).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        }
                    }
                }
                if (gameObject.name == "ContentShader")
                {
                    for (int t = 0; t < ShaderSliders.Count; t++)
                    {
                        ShaderSliders[t].SetActive(false);
                        switch (transform.GetChild(i).name)
                        {
                            case "Contrast":
                                ShaderSliders[0].SetActive(true);
                                break;
                            case "Saturation":
                                ShaderSliders[1].SetActive(true);
                                break;
                            case "Temperature":
                                ShaderSliders[2].SetActive(true);
                                break;
                            case "Brightness":
                                ShaderSliders[3].SetActive(true);
                                break;
                        }
                    }
                }
                else if (gameObject.name == "ContentTheme")
                {
                    for (int p = 0; p < ThemesPormade.Count; p++)
                    {
                        ThemesPormade[p].SetActive(false);
                        switch (transform.GetChild(i).name)
                        {
                            case "theme_0":
                                ThemesPormade[0].SetActive(true);
                                break;
                            case "theme_1":
                                ThemesPormade[1].SetActive(true);
                                break;
                            case "theme_2":
                                ThemesPormade[2].SetActive(true);
                                break;
                            case "theme_3":
                                ThemesPormade[3].SetActive(true);
                                break;
                            case "theme_4":
                                ThemesPormade[4].SetActive(true);
                                break;
                        }
                    }
                }
            }
        }
    }
}