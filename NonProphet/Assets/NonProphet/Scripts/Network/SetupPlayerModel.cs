using Photon.Pun;
using UnityEngine;

public class SetupPlayerModel : MonoBehaviour
{
    public PhotonView PhotonView;

    public GameObject[] ObjectsToCull;
    public string CullLayer;

    public Behaviour[] BehavioursToDisable;
    public CharacterController CharacterControllerToDisable;

    void Start()
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
            foreach (var item in BehavioursToDisable)
            {
                item.enabled = false;
            }

            CharacterControllerToDisable.enabled = false;
        }
    }
}
