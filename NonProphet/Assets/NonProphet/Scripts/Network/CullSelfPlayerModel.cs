using Photon.Pun;
using UnityEngine;

public class CullSelfPlayerModel : MonoBehaviour
{
    public PhotonView PhotonView;

    public GameObject[] ObjectsToCull;
    public string CullLayer;

    void Start()
    {
        if (PhotonView.IsMine)
        {
            foreach (var item in ObjectsToCull)
            {
                item.layer = LayerMask.NameToLayer(CullLayer);
            }
        }
    }
}
