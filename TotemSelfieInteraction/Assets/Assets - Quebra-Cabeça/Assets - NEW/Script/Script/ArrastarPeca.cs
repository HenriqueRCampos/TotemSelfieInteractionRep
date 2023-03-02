using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class ArrastarPeca : MonoBehaviour
{

    RaycastHit2D hit;
    public List<GameObject> OcorrenciaToque;
    private int toques;
    private int index;

    private void Start()
    {
        toques = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (Embaralhador.Embaralhar)
        {
            if (toques != Input.touchCount && OcorrenciaToque.Count > 0)
            {
                PosicionarPeca();
            }
            if (toques != Input.touchCount)
            {
                CapturarPosicaoPecas();
            }
            if (toques == Input.touchCount)
            {
                if (Input.touchCount > 1)
                {
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        if (i < OcorrenciaToque.Count)
                        {
                            {
                                Vector3 PosNova = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x - OcorrenciaToque[i].transform.position.x, Input.GetTouch(i).position.y - OcorrenciaToque[i].transform.position.y));
                                OcorrenciaToque[i].transform.localPosition = new Vector3(PosNova.x, PosNova.y, OcorrenciaToque[i].transform.localPosition.z);
                            }
                        }
                    }
                }
                else
                {
                    if (OcorrenciaToque.Count > 0 && Input.touchCount > 0)
                    {
                        int i = 0;
                        Vector3 PosNova = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x - OcorrenciaToque[i].transform.position.x, Input.GetTouch(i).position.y - OcorrenciaToque[i].transform.position.y));
                        OcorrenciaToque[i].transform.localPosition = new Vector3(PosNova.x, PosNova.y, OcorrenciaToque[i].transform.localPosition.z);
                    }
                }
            }
            if (Input.touchCount <= 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)), (Vector2)Camera.main.transform.forward);
                    if (hit.collider != null && hit.collider.gameObject != null && !hit.collider.GetComponent<PosicaoPeca>().PosicaoVoltar)
                    {
                        OcorrenciaToque.Add(hit.collider.gameObject);
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    if (OcorrenciaToque.Count > 0)
                    {
                        Vector3 PosNova = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - OcorrenciaToque[0].transform.position.x, Input.mousePosition.y - OcorrenciaToque[0].transform.position.y));
                        OcorrenciaToque[0].transform.localPosition = new Vector3(PosNova.x, PosNova.y, OcorrenciaToque[0].transform.localPosition.z);
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    PosicionarPecaMouse();
                }
            }
        }
    }
    void CapturarPosicaoPecas()
    {
        OcorrenciaToque.Clear();
        for (int i = 0; i < Input.touchCount; i++)
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector2(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y)), (Vector2)Camera.main.transform.forward);
            if (hit.collider != null && hit.collider.gameObject != null && !hit.collider.GetComponent<PosicaoPeca>().PosicaoVoltar)
            {
                OcorrenciaToque.Add(hit.collider.gameObject);
            }
        }
        index = OcorrenciaToque.Count;
        toques = Input.touchCount;
    }
    void PosicionarPeca()
    {
        if (Input.touchCount < toques)
        {
            for (int i = 0; i < OcorrenciaToque.Count; i++)
            {
                OcorrenciaToque[i].GetComponent<PosicaoPeca>().PosicaoVoltar = OcorrenciaToque[i].GetComponent<PosicaoPeca>().EstaPerto;
                if (OcorrenciaToque[i].GetComponent<PosicaoPeca>().PosicaoVoltar)
                {
                    OcorrenciaToque[i].GetComponent<SortingGroup>().sortingOrder = -1;
                    OcorrenciaToque[i].GetComponent<SpriteMask>().sortingOrder = -1;
                }
                else
                {
                    OcorrenciaToque[i].GetComponent<SortingGroup>().sortingOrder = 1;
                    OcorrenciaToque[i].GetComponent<SpriteMask>().sortingOrder = 1;
                }
            }
        }
    }
    void PosicionarPecaMouse()
    {
        for (int i = 0; i < OcorrenciaToque.Count; i++)
        {
            OcorrenciaToque[i].GetComponent<PosicaoPeca>().PosicaoVoltar = OcorrenciaToque[i].GetComponent<PosicaoPeca>().EstaPerto;
            if (OcorrenciaToque[i].GetComponent<PosicaoPeca>().PosicaoVoltar)
            {
                OcorrenciaToque[i].GetComponent<SortingGroup>().sortingOrder = -1;
                OcorrenciaToque[i].GetComponent<SpriteMask>().sortingOrder = -1;
            }
            else
            {
                OcorrenciaToque[i].GetComponent<SortingGroup>().sortingOrder = 1;
                OcorrenciaToque[i].GetComponent<SpriteMask>().sortingOrder = 1;
            }
        }
        OcorrenciaToque.Clear();
    }
}