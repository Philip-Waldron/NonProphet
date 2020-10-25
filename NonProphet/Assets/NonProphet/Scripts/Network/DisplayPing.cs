using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPing : MonoBehaviour
{
    public Text pingText;
    public bool AllowPingQuery = true;

    void Start()
    {
        StartCoroutine(nameof(UpdatePing));
    }

    private IEnumerator UpdatePing()
    {
        while (AllowPingQuery)
        {
            pingText.text = PhotonNetwork.GetPing() + "ms";
            yield return new WaitForSeconds(2);
        }
    }
}
