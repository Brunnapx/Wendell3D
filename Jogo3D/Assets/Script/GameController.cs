using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text CoraçaoText;
    public float Quantidade;
    public Text QuantidadeText;

    public static GameController instance;

// Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

// Update is called once per frame
    void Update()
    {

    }

    public void UpdateQuantidade(float value)
    {
        Quantidade += value;
        QuantidadeText.text = Quantidade.ToString();
    }

    public void Coracao(float value)
    {
        CoraçaoText.text = "x " + value.ToString();
    }
}
