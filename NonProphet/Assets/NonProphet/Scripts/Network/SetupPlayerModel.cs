using Photon.Pun;
using UnityEngine;

public class SetupPlayerModel : MonoBehaviour
{
    public PhotonView PhotonView;

    [Header("Cull these from the local player's camera")]
    public string CullLayer;
    public GameObject[] ObjectsToCull;

    [Header("Remove these if the model isn't owned by the local player")]
    public Component[] ComponentsToRemove;

    void Awake()
    {
        if (PhotonView.IsMine)
        {
            foreach (var item in ObjectsToCull)
            {
                item.layer = LayerMask.NameToLayer(CullLayer);
            }
        }
        else
        {
            foreach (Component item in ComponentsToRemove)
            {
                Destroy(item);
            }
        }
    }
}
