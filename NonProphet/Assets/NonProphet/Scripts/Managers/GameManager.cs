using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool UseVR = true;
    public Camera DefaultCamera;
    public Camera VRCamera;

    void Awake()
    {
        DontDestroyOnLoad(this);
        Debug.Log("GameManager UseVR = " + UseVR);
        XRSettings.enabled = UseVR;
        DefaultCamera.gameObject.SetActive(!IsUsingVR());
        VRCamera.gameObject.SetActive(IsUsingVR());
    }

    public bool IsUsingVR()
    {
        return UseVR && IsXRDevicePresent();
    }

    public static bool IsXRDevicePresent()
    {
        var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances(xrDisplaySubsystems);
        return xrDisplaySubsystems.Any(xrDisplay => xrDisplay.running);
    }

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
