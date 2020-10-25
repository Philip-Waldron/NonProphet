using UnityEngine;
using UnityEngine.UI;

public class GenerateRandomStartButtonText : MonoBehaviour
{
    public string[] StartPhrases;
    public Text ButtonText;

    void Start()
    {
        ButtonText.text = StartPhrases[Random.Range(0, StartPhrases.Length)];
    }
}
