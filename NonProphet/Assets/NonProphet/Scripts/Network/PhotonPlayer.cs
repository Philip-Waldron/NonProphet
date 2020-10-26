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
                new Vector3(5, 5, 5),
                Quaternion.identity);
        }
        else
        {
            PlayerModel = PhotonNetwork.Instantiate(PlayerPrefab.name,
                new Vector3(5, 5, 5),
                Quaternion.identity);
        }
    }
}

//LevelManager.Instance.SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].position,
//LevelManager.Instance.SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].rotation);