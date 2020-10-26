using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
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
    public Canvas MainMenuCanvas;
    public Canvas LobbyCanvas;

    [Header("Dropdowns")]
    public Dropdown inputsDropdown;
    public Dropdown outputsDropdown;

    private bool settingsOpen;
    private bool mainMenuOpen;
    private bool lobbyOpen;

    private List<MicRef> micOptions;

    [SerializeField]
    private Recorder recorder;

    void Awake()
    {
        this.RefreshMicrophones();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //Track state of enabled canvases when user hits escape
            if (!settingsOpen)
            {
                mainMenuOpen = MainMenuCanvas.enabled;
                lobbyOpen = LobbyCanvas.enabled;
            }

            settingsOpen = !settingsOpen;

            if (settingsOpen)
            {
                SettingsCanvas.enabled = true;
                MainMenuCanvas.enabled = false;
                LobbyCanvas.enabled = false;
            }

            else
            {
                SettingsCanvas.enabled = false;

                //Revert other canvases to their previous before the user hit escape
                MainMenuCanvas.enabled = mainMenuOpen;
                LobbyCanvas.enabled = lobbyOpen;
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

        if (Recorder.PhotonMicrophoneEnumerator.IsSupported)
        {
            for (int i = 0; i < Recorder.PhotonMicrophoneEnumerator.Count; i++)
            {
                string n = Recorder.PhotonMicrophoneEnumerator.NameAtIndex(i);
                this.micOptions.Add(new MicRef(n, Recorder.PhotonMicrophoneEnumerator.IDAtIndex(i)));
                micOptionsStrings.Add(n);
            }
        }

        this.inputsDropdown.AddOptions(micOptionsStrings);
        this.inputsDropdown.onValueChanged.RemoveAllListeners();
        this.inputsDropdown.onValueChanged.AddListener(delegate { this.MicDropdownValueChanged(this.micOptions[this.inputsDropdown.value]); });
    }

    private void MicDropdownValueChanged(MicRef mic)
    {
        this.recorder.MicrophoneType = mic.MicType;
        this.recorder.PhotonMicrophoneDeviceId = mic.PhotonId;

        if (this.recorder.RequiresRestart)
        {
            this.recorder.RestartRecording();
        }
    }

    private void SetCurrentValue()
    {
        if (this.micOptions == null)
        {
            Debug.LogWarning("micOptions list is null");
            return;
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
