using UnityEngine;
using UnityEngine.UI;

public class MexerFundo : MonoBehaviour
{
    public float speed = 1f;
    Vector2 offset;
    void Start()
    {
        gameObject.GetComponent<Image>().material.mainTextureOffset = new Vector2(0, 0);
        offset = new Vector2(0, speed);
    }

    // Update is called once per frame
    void Update()
    {

        gameObject.GetComponent<Image>().material.mainTextureOffset += offset * Time.deltaTime;
        if(gameObject.GetComponent<Image>().material.mainTextureOffset.y >= 2) {
            gameObject.GetComponent<Image>().material.mainTextureOffset = new Vector2(0,0);
        }
    }
}
