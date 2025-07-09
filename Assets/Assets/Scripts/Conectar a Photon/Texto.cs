using TMPro;
using UnityEngine;
using System;

public class Texto : MonoBehaviour
{
    public TextMeshProUGUI _texto;
    public static Texto Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void RegistarTexto(string texto)
    {
        _texto.text = texto;
    }

}
