using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public struct MicRef
{
    public Recorder.MicType MicType;
    public string Name;
    public int PhotonId;

    public MicRef(string name, int id)
    {
        this.MicType = Recorder.MicType.Photon;
        this.Name = name;
        this.PhotonId = id;
    }

    public MicRef(string name)
    {
        this.MicType = Recorder.MicType.Unity;
        this.Name = name;
        this.PhotonId = -1;
    }

    public override string ToString()
    {
        return string.Format("Mic reference: {0}", this.Name);
    }
}

public class SettingsManager : MonoBehaviour
{
    [Header("Canvases")]
    public Canvas SettingsCanvas;
    public List<Canvas> OtherCanvases;
    private List<bool> OtherCanvasesEnabledList = new List<bool>();
    private CursorLockMode previousLockMode;

    [Header("Dropdowns")]
    public Dropdown inputsDropdown;

    private bool settingsOpen;

    private List<MicRef> micOptions;
    private Recorder recorder;

    void Awake()
    {
        this.recorder = PhotonVoiceNetwork.Instance.PrimaryRecorder;

        for (int i = 0; i < OtherCanvases.Count; i++)
        {
            OtherCanvasesEnabledList.Add(false);
        }

        this.RefreshMicrophones();
        this.SetSavedMicIfAvailable();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //Track state of enabled canvases when user hits escape
            if (!settingsOpen)
            {
                for (int i = 0; i < OtherCanvases.Count; i++)
                {
                    var canvas = OtherCanvases[i];
                    OtherCanvasesEnabledList[i] = canvas.enabled;
                }

                previousLockMode = Cursor.lockState;
            }

            settingsOpen = !settingsOpen;

            if (settingsOpen)
            {
                SettingsCanvas.enabled = true;
                inputsDropdown.enabled = true;

                foreach (Canvas canvas in OtherCanvases)
                {
                    canvas.enabled = false;
                }

                Cursor.lockState = CursorLockMode.None;
            }

            else
            {
                SettingsCanvas.enabled = false;
                inputsDropdown.enabled = false;

                //Revert other canvases to their previous before the user hit escape
                for (int i = 0; i < OtherCanvases.Count; i++)
                {
                    var canvas = OtherCanvases[i];
                    canvas.enabled = OtherCanvasesEnabledList[i];
                }

                Cursor.lockState = previousLockMode;
            }
        }
    }

    private void SetSavedMicIfAvailable()
    {
        var micPreference = PlayerPrefs.GetString("MicPreference");

        if (micPreference != null && this.recorder != null)
        {
            recorder.UnityMicrophoneDevice = micPreference;

            if (recorder.RequiresRestart)
            {
                recorder.RestartRecording();
            }
        }
    }

    public void RefreshMicrophones()
    {
        Recorder.PhotonMicrophoneEnumerator.Refresh();
        this.SetupMicDropdown();
        this.SetCurrentValue();
    }

    private void SetupMicDropdown()
    {
        this.inputsDropdown.ClearOptions();

        this.micOptions = new List<MicRef>();
        List<string> micOptionsStrings = new List<string>();

        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            string micName = Microphone.devices[i];
            this.micOptions.Add(new MicRef(micName));
            micOptionsStrings.Add(micName);
        }

        this.inputsDropdown.AddOptions(micOptionsStrings);
        this.inputsDropdown.onValueChanged.RemoveAllListeners();
        this.inputsDropdown.onValueChanged.AddListener(delegate { this.MicDropdownValueChanged(this.micOptions[this.inputsDropdown.value]); });
    }

    private void MicDropdownValueChanged(MicRef mic)
    {
        if (this.recorder == null)
        {
            return;
        }

        this.recorder.MicrophoneType = mic.MicType;
        this.recorder.UnityMicrophoneDevice = mic.Name;

        PlayerPrefs.SetString("MicPreference", mic.Name);

        if (this.recorder.RequiresRestart)
        {
            this.recorder.RestartRecording();
        }
    }

    private void SetCurrentValue()
    {
        if (this.recorder == null)
        {
            Debug.LogWarning("Recorder is null");
            return;
        }

        if (this.micOptions == null)
        {
            Debug.LogWarning("micOptions list is null");
            return;
        }

        for (int valueIndex = 0; valueIndex < this.micOptions.Count; valueIndex++)
        {
            MicRef val = this.micOptions[valueIndex];
            if (val.Name == PlayerPrefs.GetString("MicPreference"))
            {
                    this.inputsDropdown.value = valueIndex;
                    return;
            }
        }

        for (int valueIndex = 0; valueIndex < this.micOptions.Count; valueIndex++)
        {
            MicRef val = this.micOptions[valueIndex];
            if (this.recorder.MicrophoneType == val.MicType)
            {
                if (val.PhotonId == this.recorder.PhotonMicrophoneDeviceId)
                {
                    this.inputsDropdown.value = valueIndex;
                    return;
                }
            }
        }
        for (int valueIndex = 0; valueIndex < this.micOptions.Count; valueIndex++)
        {
            MicRef val = this.micOptions[valueIndex];
            if (this.recorder.MicrophoneType == val.MicType)
            {
                this.inputsDropdown.value = valueIndex;
                this.recorder.PhotonMicrophoneDeviceId = val.PhotonId;
            }
        }
        if (this.recorder.RequiresRestart)
        {
            this.recorder.RestartRecording();
        }
    }
}
