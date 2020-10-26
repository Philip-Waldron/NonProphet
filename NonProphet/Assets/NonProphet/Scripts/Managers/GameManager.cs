using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private bool _useVR = true;

    void Awake()
    {
        DontDestroyOnLoad(this);
        Debug.Log("GameManager UseVR = " + _useVR);
        XRSettings.enabled = _useVR;
    }

    public bool IsUsingVR()
    {
        return _useVR && IsXRDevicePresent();
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
