using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SelectShaderBar : MonoBehaviour
{
    [SerializeField] private GameObject scrollbar;
    [SerializeField] private List<GameObject> ShaderSliders;
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
                transform.GetChild(i).GetChild(0).localScale = Vector2.Lerp(transform.GetChild(i).GetChild(0).localScale, new Vector2(1.5f, 1.5f), 0.1f);
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).GetChild(0).localScale = Vector2.Lerp(transform.GetChild(a).GetChild(0).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
        for (int i = 0; i < ShaderSliders.Count; i++)
        {
            ShaderSliders[i].SetActive(false);
            switch (transform.GetChild(0).name)
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
}
