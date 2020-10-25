using System.Collections;
using System.Collections.Generic;
using NonProphet.Scripts.XR;
using UnityEngine;

public static class Reference
{
    public const string PlayerTag = "Player";
    public static XRInputController XRInputController()
    {
        return GameObject.FindWithTag(PlayerTag).GetComponent<XRInputController>();
    }
}
