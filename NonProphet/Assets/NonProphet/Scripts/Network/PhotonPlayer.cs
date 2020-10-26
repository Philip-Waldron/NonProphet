using Photon.Pun;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject PlayerModel;

    void Start()
    {
        PlayerModel = PhotonNetwork.Instantiate(PlayerPrefab.name,
            LevelManager.Instance.SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].position,
            LevelManager.Instance.SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].rotation);
    }
}
