using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine; 
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GeradorDeQuebraCabeca : MonoBehaviour
{
    public Transform Fundo;
    public GameObject ImageFundo;
    public Sprite Borda;
    public Sprite EfeitoPeca;
    public List<Sprite> PecasSuperiores;
    public List<Sprite> PecasMeio1;
    public List<Sprite> PecasMeio2;
    public List<Sprite> PecasInferiores;
    public List<Sprite> PecasSuperioresEfeitoBorda;
    public List<Sprite> PecasMeio1EfeitoBorda;
    public List<Sprite> PecasMeio2EfeitoBorda;
    public List<Sprite> PecasInferioresEfeitoBorda;
    public List<GameObject> FundoMarcacao;
    public static int TamanhoX;
    public static int TamanhoXaux;
    private int ContagemX;
    public static int TamanhoY;
    public static int TamanhoYaux;
    private int AuxiliarLinhaEscolha;
    private List<Sprite> Pecas;
    private List<Sprite> PecasEfeito;
    public Sprite[] FotosParaFundo;
    private GameObject BordaAux;
    public System.Random rd = new System.Random();
    Vector3 pos;
    private int ImagemEscolhida;
    public string[] Images;
    public void Awake()
    {
#if UNITY_STANDALONE_WIN
        BuscarFotosPasta();
#endif
        gameObject.name = "Gerador";
    }
    void BuscarFotosPasta()
    {
#if UNITY_STANDALONE_WIN
        string supportedExtensions = "*.jpg,*.png,*.jpeg";
        string _assetsDirectory = "./Assets/Resources/";
        if (Directory.Exists(_assetsDirectory))
        {
            IEnumerable<string> filePaths =
                Directory.GetFiles(_assetsDirectory, "*.*", SearchOption.AllDirectories).Where(s => supportedExtensions.Contains(Path.GetExtension(s).ToLower()));
            Images = filePaths.ToArray();

            if (Images.Length > 0) FotosParaFundo = new Sprite[Images.Length];

            for (int i = 0; i < Images.Length; i++)
            {
                Texture2D texture = new Texture2D(500, 475, TextureFormat.RGB24, false);
                texture.filterMode = FilterMode.Point;
                TextAsset text = Resources.Load<TextAsset>(Images[i]);
                byte[] FileData;
                FileData = File.ReadAllBytes(Images[i]);
                texture.LoadImage(FileData);
                FotosParaFundo[i] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f, 1, SpriteMeshType.FullRect);
            }
            if (PlayerPrefs.HasKey("FotoSet") && PlayerPrefs.GetInt("FotoSet") < FotosParaFundo.Length)
             {
                   ImagemEscolhida = PlayerPrefs.GetInt("FotoSet");
             }else{
                ImagemEscolhida=0;
             }
        }
        else
        {
            Debug.Log("Criando o diretório: "+ _assetsDirectory);
            Directory.CreateDirectory(_assetsDirectory);
        }
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        if (TamanhoX <= 0 || TamanhoY <= 0)
        {
            TamanhoX = 6;
            TamanhoY = 4;
        }
        //Verifica Se é Par, segue o calculo de posição, e a linha da matriz :]
        if (TamanhoY >= 1 && TamanhoX >= 1 && TamanhoY % 2 == 0 && TamanhoX % 2 == 0)
        {
            transform.position = Vector3.zero;
#if UNITY_WEBGL
            TamanhoX = 8;
            TamanhoY = 6;
            Resultado.RecaregarCena = SceneManager.GetActiveScene().name;
#endif

#if UNITY_ANDROID
            TamanhoX = 8;
            TamanhoY = 6;
#endif
            TamanhoXaux = TamanhoX;
            TamanhoYaux = TamanhoY;
            pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            AuxiliarLinhaEscolha = 3;
            Gerar();
        }
        //Verifica Se For Impar, segue com uma mensagem de erro, sendo uma matriz Inválida :[
        else
        {
            Debug.ClearDeveloperConsole();
            Debug.LogError("Formato Inválido: " + TamanhoX + "X" + TamanhoY);
        }
    }

    void Gerar()
    {
        for (int y = 0; y < TamanhoY; y++)
        {

            ContagemX = 2;
            if (y <= 0)
            {
                Pecas = PecasSuperiores;
                PecasEfeito = PecasSuperioresEfeitoBorda;
            }
            else if (y > 0 && y < TamanhoY - 1)
            {
                AuxiliarLinhaEscolha++;
                if (AuxiliarLinhaEscolha >= 3)
                {
                    AuxiliarLinhaEscolha = 1;
                }
                if (AuxiliarLinhaEscolha == 1)
                {
                    if (y == 1)
                    {
                        pos = new Vector3(transform.position.x, pos.y - 1.47f, transform.position.z);
                    }
                    else
                    {
                        pos = new Vector3(transform.position.x, pos.y - 1.71f, transform.position.z);
                    }
                    Pecas = PecasMeio1;
                    PecasEfeito = PecasMeio1EfeitoBorda;
                }
                if (AuxiliarLinhaEscolha == 2)
                {
                    pos = new Vector3(transform.position.x, pos.y - 1.71f, transform.position.z);
                    Pecas = PecasMeio2;
                    PecasEfeito = PecasMeio2EfeitoBorda;
                }
            }
            else if (y > 0 && y >= TamanhoY - 1)
            {
                if (y >= 2)
                {
                    pos = new Vector3(transform.position.x, pos.y - 1.47f, transform.position.z);
                }
                else
                {
                    pos = new Vector3(transform.position.x, pos.y - 1.21f, transform.position.z);
                }
                Pecas = PecasInferiores;
                PecasEfeito = PecasInferioresEfeitoBorda;
            }
            for (int x = 0; x < TamanhoX; x++)
            {
                GameObject pecaaux = new GameObject("Peca");
                pecaaux.AddComponent<SpriteRenderer>();
                pecaaux.AddComponent<SpriteMask>();
                pecaaux.GetComponent<SpriteMask>().alphaCutoff = 0.01f;
                pecaaux.AddComponent<SortingGroup>();
                GameObject EfeitoBorda = Instantiate(pecaaux, pecaaux.transform.position, pecaaux.transform.rotation);
                EfeitoBorda.name = "Efeito Borda";
                pecaaux.AddComponent<PosicaoPeca>();
                pecaaux.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                if (x <= 0)
                {
                    
                    pecaaux.name = Pecas[x].name + " (" + x.ToString() + ")";
                    pecaaux.GetComponent<SpriteRenderer>().sprite = Pecas[x];
                    pecaaux.GetComponent<SpriteMask>().sprite = Pecas[x];
                    EfeitoBorda.GetComponent<SpriteRenderer>().sprite = PecasEfeito[x];
                    EfeitoBorda.GetComponent<SpriteMask>().sprite = PecasEfeito[x];
                }
                if (x > 0 && x < TamanhoX - 1)
                {
                    pecaaux.name = Pecas[ContagemX].name + " (" + x.ToString() + ")";
                    pos = new Vector3(pos.x + 1.72f, pos.y, pos.z);
                    EfeitoBorda.GetComponent<SpriteRenderer>().sprite = PecasEfeito[ContagemX];
                    EfeitoBorda.GetComponent<SpriteMask>().sprite = PecasEfeito[ContagemX];
                    pecaaux.GetComponent<SpriteRenderer>().sprite = Pecas[ContagemX];
                    pecaaux.GetComponent<SpriteMask>().sprite = Pecas[ContagemX];
                }
                else if (x >= TamanhoX - 1)
                {
                    pecaaux.name = Pecas[Pecas.Count - 1] + " (" + x.ToString() +")";
                    pos = new Vector3(pos.x + 1.45f, pos.y, pos.z);
                    EfeitoBorda.GetComponent<SpriteRenderer>().sprite = PecasEfeito[Pecas.Count - 1];
                    EfeitoBorda.GetComponent<SpriteMask>().sprite = PecasEfeito[Pecas.Count - 1];
                    pecaaux.GetComponent<SpriteRenderer>().sprite = Pecas[Pecas.Count - 1];
                    pecaaux.GetComponent<SpriteMask>().sprite = Pecas[Pecas.Count - 1];

                }
                pecaaux.transform.position = pos;
                EfeitoBorda.GetComponent<SortingGroup>().sortingOrder = 1;
                EfeitoBorda.transform.position = pos;
                GameObject MarcaFundo = Instantiate(EfeitoBorda, EfeitoBorda.transform.position, EfeitoBorda.transform.rotation);
                MarcaFundo.name = "Fundo Efeito-Borda";
                MarcaFundo.GetComponent<SortingGroup>().sortingOrder = -1;
                FundoMarcacao.Add(MarcaFundo);
                pecaaux.AddComponent<PolygonCollider2D>();
                Camera.main.GetComponent<Embaralhador>().Pecas.Add(pecaaux);
                pecaaux.GetComponent<PosicaoPeca>().Calcular(pecaaux.transform);
                EfeitoBorda.transform.SetParent(pecaaux.transform);
                ContagemX++;
                if (ContagemX >= PecasSuperiores.Count - 1)
                {
                    ContagemX = 1;
                }
            }
        }
        GerarFundo();
    }
    void GerarFundo()
    {
        Fundo.transform.position = new Vector3(Fundo.transform.position.x, Fundo.transform.position.y, 3);
        ImageFundo.GetComponent<SpriteRenderer>().sprite = FotosParaFundo[ImagemEscolhida];
        ImageFundo.name = "Fundo";
        int totalPecas = Camera.main.GetComponent<Embaralhador>().Pecas.Count - 1;
        ImageFundo.layer = 2;
        ImageFundo.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        ImageFundo.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        float posx = (Camera.main.GetComponent<Embaralhador>().Pecas[0].transform.position.x - Camera.main.GetComponent<Embaralhador>().Pecas[TamanhoX - 1].transform.position.x);
        float posy = (Camera.main.GetComponent<Embaralhador>().Pecas[TamanhoX - 1].transform.position.y - Camera.main.GetComponent<Embaralhador>().Pecas[totalPecas].transform.position.y);
        float Alt = Camera.main.GetComponent<Embaralhador>().Pecas[0].GetComponent<SpriteRenderer>().sprite.texture.height;
        float Larg = Camera.main.GetComponent<Embaralhador>().Pecas[0].GetComponent<SpriteRenderer>().sprite.texture.width;
        ImageFundo.GetComponent<SpriteRenderer>().size = new Vector2(posx + posx / Larg, posy + posy / Alt);
        ImageFundo.GetComponent<SpriteRenderer>().size = new Vector2(ImageFundo.GetComponent<SpriteRenderer>().size.x - 1.97f, ImageFundo.GetComponent<SpriteRenderer>().size.y + 2.21f);
        totalPecas = Camera.main.GetComponent<Embaralhador>().Pecas.Count - 1;
        float PosXinicial = getPositionXPeca(Camera.main.GetComponent<Embaralhador>().Pecas[0], true);
        float PosYinicial = getPositionYPeca(Camera.main.GetComponent<Embaralhador>().Pecas[0], true);
        float PosXFinal = getPositionXPeca(Camera.main.GetComponent<Embaralhador>().Pecas[totalPecas], false);
        float PosYFinal = getPositionYPeca(Camera.main.GetComponent<Embaralhador>().Pecas[totalPecas], false);
        ImageFundo.GetComponent<SpriteRenderer>().sortingOrder = 1;
        ImageFundo.transform.position = new Vector3((PosXinicial + PosXFinal) / 2 + 0.13f, (PosYinicial + PosYFinal) / 2, ImageFundo.transform.position.z);
        for (int i = 0; i <= totalPecas; i++)
        {
            GameObject Image = Instantiate(ImageFundo, ImageFundo.transform.position, ImageFundo.transform.rotation);
            GameObject EfeitoPapelPeca = Instantiate(ImageFundo, ImageFundo.transform.position, ImageFundo.transform.rotation);
            EfeitoPapelPeca.GetComponent<SpriteRenderer>().sprite = EfeitoPeca;
            Image.transform.SetParent(Camera.main.GetComponent<Embaralhador>().Pecas[i].transform);
            Image.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            EfeitoPapelPeca.transform.SetParent(Camera.main.GetComponent<Embaralhador>().Pecas[i].transform);
            EfeitoPapelPeca.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            EfeitoPapelPeca.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
        }
        Camera.main.transform.position = new Vector3(ImageFundo.transform.position.x, ImageFundo.transform.position.y, Camera.main.transform.position.z);
        Camera.main.orthographicSize = DistanciaXCal() + 1;
        if (Camera.main.orthographicSize < 3)
        {
            Camera.main.orthographicSize = 2.5f;
        }
        if (Camera.main.orthographicSize < 0)
        {
            Camera.main.orthographicSize = -(Camera.main.orthographicSize) + 5;
        }
        float dist = Vector3.Distance(Camera.main.GetComponent<Embaralhador>().Pecas[0].transform.position, Camera.main.GetComponent<Embaralhador>().Pecas[Camera.main.GetComponent<Embaralhador>().Pecas.Count - 1].transform.position);
        Camera.main.orthographicSize = dist;
        if(Camera.main.orthographicSize < 3)
        {
            Camera.main.orthographicSize = 3;
        }
        for (int i=0; i < Camera.main.GetComponent<Embaralhador>().Pecas.Count; i++)
        {
            Camera.main.GetComponent<Embaralhador>().Pecas[i].GetComponent<PosicaoPeca>().CalcularTamanho();
        }
        ImageFundo.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.4f);
        ImageFundo.GetComponent<SpriteRenderer>().sortingOrder = -1;
        ImagemEscolhida++;
#if UNITY_STANDALONE_WIN
        PlayerPrefs.SetInt("FotoSet", ImagemEscolhida);
#endif
        if (ImagemEscolhida > FotosParaFundo.Length-1)
        {
            ImagemEscolhida = 0;
        }
        BordaAux = Instantiate(ImageFundo, ImageFundo.transform.position, ImageFundo.transform.rotation) as GameObject;
        BordaAux.GetComponent<SpriteRenderer>().sprite = Borda;
        BordaAux.GetComponent<SpriteRenderer>().color = Color.white;
        BordaAux.transform.localScale = new Vector3(BordaAux.transform.lossyScale.x+0.015f, BordaAux.transform.lossyScale.y+0.033f, BordaAux.transform.lossyScale.z);
    }

    public void PosicionarCamera()
    {
        if (Screen.height > Screen.width)
        {
            float dist = Vector3.Distance(Camera.main.GetComponent<Embaralhador>().Pecas[0].transform.position, Camera.main.GetComponent<Embaralhador>().Pecas[Camera.main.GetComponent<Embaralhador>().Pecas.Count - 1].transform.position);
            Camera.main.farClipPlane = dist+6;
            Fundo.transform.position = new Vector3(Fundo.transform.position.x, Fundo.transform.position.y, dist -1);
        }
        
        else
        {
            Fundo.transform.position = new Vector3(Fundo.transform.position.x, Fundo.transform.position.y, TamanhoX * TamanhoY);
            
            Camera.main.farClipPlane = Vector3.Distance(Camera.main.transform.position, Fundo.transform.position) * 2;
        }
    }

    float DistanciaXCal()
    {
        float DistanciaX = Camera.main.GetComponent<Embaralhador>().Pecas[TamanhoX - 1].transform.position.x/2;
        if(DistanciaX < TamanhoX)
        {
            DistanciaX = Camera.main.GetComponent<Embaralhador>().Pecas[TamanhoX - 1].transform.position.x / 2 + TamanhoY;
        }
        if (DistanciaX < 0)
        {
            DistanciaX = -(DistanciaX);
        }
        if (DistanciaX == TamanhoY)
        {
            DistanciaX *= DistanciaX;
        }
        if (TamanhoX > TamanhoY)
        {
            DistanciaX -= (DistanciaX-TamanhoY) /(TamanhoX/TamanhoY);
        }
        return DistanciaX;
    }
    float getPositionXPeca(GameObject Peca, bool initial = true)
    {
        float position = Peca.transform.position.x;
        float width = Peca.GetComponent<SpriteRenderer>().sprite.texture.width;
        return (initial ? position - (width / 2) : position + (width / 2));
    }

    float getPositionYPeca(GameObject Peca, bool initial = true)
    {
        float position = Peca.transform.position.y;
        float height = Peca.GetComponent<SpriteRenderer>().sprite.texture.height;
        return (initial ? position - (height / 2) : position + (height / 2));
    }
    public void GerarNovo()
    {
        Destroy(BordaAux);
        Embaralhador.Embaralhar = false;
        ImageFundo.SetActive(true);
        for (int i = 0; i < Camera.main.GetComponent<Embaralhador>().Pecas.Count; i++)
        {
            Destroy(FundoMarcacao[i]);
            Destroy(Camera.main.GetComponent<Embaralhador>().Pecas[i]);
        }
        FundoMarcacao = new List<GameObject>();
        Camera.main.GetComponent<Embaralhador>().Pecas = new List<GameObject>();
        Camera.main.GetComponent<Embaralhador>().Start();
        Awake();
        Start();
    }
}
