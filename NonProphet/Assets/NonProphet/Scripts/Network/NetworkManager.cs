using Photon.Pun;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject StartButton;
    public AnimateTransform ButtonMaskAnimateTransform;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the " + PhotonNetwork.CloudRegion + " server!");
        StartButton.SetActive(true);
        ButtonMaskAnimateTransform.PlayAnimation();
    }
}
