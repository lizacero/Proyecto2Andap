using TMPro;
using UnityEngine;

public class TextoConsola : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI _textMeshPro;
    public static TextoConsola instance;

    public void Awake()
    {
        if (instance != null) 
        {
            instance = this;
        }
        
       
    }

    public void RegisterText(string text)
    {
        Debug.Log("Console: " + text);
        _textMeshPro.text = text;

    }
}
