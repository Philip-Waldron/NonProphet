using Photon.Pun;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    public PhotonView _photonView;
    public GameObject PlayerPrefab;

    void Start()
    {
        if (_photonView.IsMine)
        {
            PhotonNetwork.Instantiate(PlayerPrefab.name,
                GameSetup.Instance.SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].position,
                GameSetup.Instance.SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].rotation);
        }
    }
}
