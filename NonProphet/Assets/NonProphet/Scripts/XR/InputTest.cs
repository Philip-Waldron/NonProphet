using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InputTest : MonoBehaviour
{
    public XRController nonDominant, dominant;

    private void Update()
    {
        bool triggerValue;
        
        if (nonDominant.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue) && triggerValue)
        {
            Debug.Log("Trigger button is pressed.");
        }
    }
}
