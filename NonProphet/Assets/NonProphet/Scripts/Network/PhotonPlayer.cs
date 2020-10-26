using Photon.Pun;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject VRPlayerPrfab;
    public GameObject PlayerModel;

    void Start()
    {
        if (GameManager.Instance.IsUsingVR())
        {
            PlayerModel = PhotonNetwork.Instantiate(VRPlayerPrfab.name,
                LevelManager.Instance.SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].position,
                LevelManager.Instance.SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].rotation);
        }
        else
        {
            PlayerModel = PhotonNetwork.Instantiate(PlayerPrefab.name,
                LevelManager.Instance.SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].position,
                LevelManager.Instance.SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].rotation);
        }
    }
}
