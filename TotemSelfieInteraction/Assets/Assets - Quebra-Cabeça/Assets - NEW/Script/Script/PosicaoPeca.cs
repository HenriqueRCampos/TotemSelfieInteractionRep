using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosicaoPeca : MonoBehaviour
{
    public Vector2 PosOri;
    public Vector2 TamanhoPecaOri;
    public Vector2 TamanhoPecaReduzido;
    public bool EstaPerto;
    public bool PosicaoVoltar = false;
    public bool Finalizou=false;

    // Start is called before the first frame update
    public void Calcular(Transform Posicao)
    {
        PosOri = Posicao.position;
        TamanhoPecaOri = Posicao.lossyScale;
        PosicaoVoltar = false;
    }
    public void CalcularTamanho() {
        float tamanhopeca = 0;
        tamanhopeca =  (Camera.main.orthographicSize/GeradorDeQuebraCabeca.TamanhoXaux);
        if (tamanhopeca <= 1.5f)
        {
            tamanhopeca = 0.5f;
        }
        TamanhoPecaReduzido = new Vector3(TamanhoPecaOri.x - tamanhopeca, TamanhoPecaOri.y - tamanhopeca);
        if(TamanhoPecaReduzido.x < 0)
        {
            TamanhoPecaReduzido = new Vector3(-(TamanhoPecaReduzido.x), TamanhoPecaReduzido.y);
        }
        if (TamanhoPecaReduzido.y < 0)
        {
            TamanhoPecaReduzido = new Vector3(TamanhoPecaReduzido.x, -(TamanhoPecaReduzido.y));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TamanhoPecaReduzido != Vector2.zero && !Finalizou)
        {
            EstaPerto = Vector2.Distance(transform.position, PosOri) < 1.0f;
            if (EstaPerto)
            {
                transform.localScale = Vector2.MoveTowards(transform.lossyScale, TamanhoPecaOri, 0.05f);
            }
            else
            {
                transform.localScale = Vector2.MoveTowards(transform.lossyScale, TamanhoPecaReduzido, 0.05f);
            }
            if (PosicaoVoltar)
            {
                if (Vector3.Distance(transform.position, PosOri) > 0.001f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, PosOri, 0.06f);
                }
                else if (Embaralhador.Embaralhar)
                {
                    if (Vector2.Distance(transform.lossyScale, TamanhoPecaOri) < 0.001f)
                    {
                        gameObject.layer = 2;
                        Finalizou = true;
                        Camera.main.GetComponent<Embaralhador>().VerPecaPosicao();
                    }
                }
            }
        }
    }

}