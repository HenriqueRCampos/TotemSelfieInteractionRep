using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProximaCena : MonoBehaviour
{

    public bool ChamarCena;
    public GameObject Gerador;

    private void Start()
    {
        Gerador = GameObject.Find("Gerador");
        ChamarCena = false;
    }

    private void Update()
    {

        if (ChamarCena)
        {
#if UNITY_ANDROID || UNITY_IPHONE
            Gerador.GetComponent<GeradorDeQuebraCabeca>().GerarNovo();
#else
          Cena();
#endif
        }
    }

    void Cena()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings -1);
    }
}
