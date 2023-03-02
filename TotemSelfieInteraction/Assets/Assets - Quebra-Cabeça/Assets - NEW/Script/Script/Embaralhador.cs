using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Embaralhador : MonoBehaviour
{
    public GameObject TelaResultado;
    public bool Ganhou;
    private int PecasNaoEncaixada;
    public Text TempoText;
    public float TempoMaxEmbarralha=4;
    public static float TempoMaxMontarQuebra;
    public static bool Embaralhar;
    public static int etapa;
    private float Tempo;
    public GameObject Contador;
    public List<GameObject> Pecas;
    public Canvas CanvaUIs;
    public GameObject Gerador;

    // Start is called before the first frame update
    public void Start()
    {

        TelaResultado.SetActive(false);
        Ganhou = false;
        MudarTempo();
#if UNITY_STANDALONE_WIN
        if (TempoMaxMontarQuebra <= 0)
        {
            TempoMaxMontarQuebra = 90;
        }
#endif
#if UNITY_WEBGL
        TempoMaxMontarQuebra = 120;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (!Ganhou)
        {
            if (Mathf.CeilToInt(Tempo) > 0)
            {
                Tempo -= Time.deltaTime;
                if (etapa > 0)
                {
                    AplicarMascaraTempo();
                }
                else{
                    TempoText.text = "Prepare-se";
                }
            }
            else
            {
                if (etapa > 0)
                {
                    VerPecaPosicao();
                }
                else if (etapa <= 0)
                {
                    //Tamanho Da Tela Para Cima
                    //Tamanho Da Tela Para Baixo
                    if (Screen.height <= Screen.width)
                    {
                        float TelaAlturaMinY1 = TamanhoTela(Screen.height) - 2.2f;
                        float TelaAlturaMaxY1 = TamanhoTela(1) + 0.6f;

                        for (int i = 0; i < Pecas.Count / 2; i++)
                        {
                            Pecas[i].transform.localPosition = new Vector3(Random.Range(PosicaoEmbaralhar(Screen.width) - 0.8f, PosicaoEmbaralhar(Screen.width) - GeradorDeQuebraCabeca.TamanhoXaux / 2), Random.Range(TelaAlturaMinY1, TelaAlturaMaxY1), i);
                        }
                        for (int i = Pecas.Count / 2; i < Pecas.Count; i++)
                        {
                            Pecas[i].transform.localPosition = new Vector3(Random.Range(PosicaoEmbaralhar(+1.0f) + 0.6f, PosicaoEmbaralhar(+1.0f) + GeradorDeQuebraCabeca.TamanhoXaux / 2), Random.Range(TelaAlturaMinY1, TelaAlturaMaxY1), i);
                        }
                    }
                    else
                    {
                        float TelaAlturaMinY2 = TamanhoTela(Screen.height) - 3.5f;
                        float TelaAlturaMaxY2 = TamanhoTela(1) + 1.6f;
                        float TelaLarguraMinX2 = PosicaoEmbaralhar(Screen.width) - 1.6f;
                        float TelaLarguraMaxX2 = PosicaoEmbaralhar(+1.0f) + 0.6f;

                        for (int i = 0; i < Pecas.Count / 2; i++)
                        {
                            Pecas[i].transform.position = new Vector3(Random.Range(TelaLarguraMinX2 + 0.6f, TelaLarguraMaxX2 - 0.6f), Random.Range(TelaAlturaMinY2- 3.4f, TelaAlturaMinY2+0.6f), i);
                        }
                        for (int i = Pecas.Count / 2; i < Pecas.Count; i++)
                        {
                            Pecas[i].transform.position = new Vector3(Random.Range(TelaLarguraMinX2 + 0.6f, TelaLarguraMaxX2 - 0.6f), Random.Range(TelaAlturaMaxY2, TelaAlturaMaxY2 + 5.5f), i);
                        }
                    }
                    Embaralhar = true;
                    Gerador.GetComponent<GeradorDeQuebraCabeca>().PosicionarCamera();
                    MudarTempo();
                }
            }
        }
    }

    float PosicaoEmbaralhar(float valor)
    {
        float PosicaoAleatoriaX = Camera.main.ScreenToWorldPoint(new Vector3(valor, 0, 0)).x;
        return PosicaoAleatoriaX;
    }

    float TamanhoTela(float valor)
    {
        float TamanhoTelaY = Camera.main.ScreenToWorldPoint(new Vector2(0,valor)).y;
        return TamanhoTelaY;
    }

    void MudarTempo()
    {
        
        if (!Embaralhar)
        {
            CanvaUIs.planeDistance = GeradorDeQuebraCabeca.TamanhoXaux * GeradorDeQuebraCabeca.TamanhoYaux+5;
            Contador.SetActive(true);
            Contador.transform.GetChild(0).GetComponent<Animator>().Play("Counter");
            Tempo = TempoMaxEmbarralha;
            etapa = 0;
        }
        else
        {
            if (Contador != null)
            {
                Contador.SetActive(false);
            }

#if UNITY_ANDROID
            Tempo = 1800000;
#else
            Tempo = TempoMaxMontarQuebra;
#endif

            etapa = 1;
        }
        
    }

    public void VerPecaPosicao()
    {
        PecasNaoEncaixada = 0;
        if (Embaralhar)
        {
            for (int i = 0; i < Pecas.Count; i++)
            {
                if (!Pecas[i].GetComponent<PosicaoPeca>().Finalizou)
                {
                    PecasNaoEncaixada++;
                }
            }
            Ganhou = PecasNaoEncaixada <= 0;
            if (Ganhou)
            {
                TelaResultado.transform.GetChild(0).GetComponent<Text>().text = "VENCEU!";
                if (Tempo > TempoMaxMontarQuebra * 17 / 100)
                {
                    Embaralhar = false;
                }
                if (Tempo <= TempoMaxMontarQuebra * 17 / 100)
                {
                    Embaralhar = false;
                }
                TelaResultado.SetActive(true);
                SceneManager.LoadSceneAsync("SelfieScene");
#if UNITY_ANDROID
                TelaResultado.GetComponent<Animator>().SetInteger("Finalizacao", 2);
#else
                TelaResultado.GetComponent<Animator>().SetInteger("Finalizacao", 1);
#endif
            }
            else if (PecasNaoEncaixada > 0)
            {
                if (Tempo <= 0)
                {
                    TelaResultado.transform.GetChild(0).GetComponent<Text>().text = "TEMPO ESGOTADO!";
                    Embaralhar = false;
                    TelaResultado.SetActive(true);
                    SceneManager.LoadSceneAsync("SelfieScene");
#if UNITY_ANDROID
                    TelaResultado.GetComponent<Animator>().SetInteger("Finalizacao", 2);
#else
                    TelaResultado.GetComponent<Animator>().SetInteger("Finalizacao", 1);
#endif
                }
            }
        }
    }

    void AplicarMascaraTempo()
    {
        if (Tempo < 0) Tempo = 0f;
#if UNITY_ANDROID
        TempoText.text = "";
#else
        TempoText.text = CalculaTempo(Tempo);
#endif
    }
    private string CalculaTempo(float Valor)
    {
        float minutos = Mathf.FloorToInt(Valor / 60);
        float segundos = Mathf.FloorToInt(Valor % 60);

        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }
    public void TentarNovam()
    {
        SceneManager.LoadSceneAsync("SelfieScene");
    }
}