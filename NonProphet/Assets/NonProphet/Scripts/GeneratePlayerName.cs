using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GeneratePlayerName : MonoBehaviour
{
    public string[] FirstNames;
    public string[] LastNames;

    public InputField NameInputField;
    public Text PlaceholderNameText;
    private string _placeholderNickname;

    private void Start()
    {
        _placeholderNickname = FirstNames[Random.Range(0, FirstNames.Length)] + " " + LastNames[Random.Range(0, LastNames.Length)];

        if (PlaceholderNameText != null)
        {
            PlaceholderNameText.text = _placeholderNickname;
        }

        PhotonNetwork.LocalPlayer.NickName = _placeholderNickname;
    }

    public void RandomiseName()
    {
        _placeholderNickname = FirstNames[Random.Range(0, FirstNames.Length)] + " " + LastNames[Random.Range(0, LastNames.Length)];

        if (PlaceholderNameText != null)
        {
            PlaceholderNameText.text = _placeholderNickname;
        }

        NameInputField.text = "";

        PhotonNetwork.LocalPlayer.NickName = _placeholderNickname;
    }

    public void SetPlayerNickname()
    {
        if (NameInputField.text != "")
        {
            PhotonNetwork.LocalPlayer.NickName = NameInputField.text;
        }
    }
}
