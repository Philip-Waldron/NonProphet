﻿using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Canvases")]
    public Canvas MainMenu;
    public Canvas LobbyMenu;

    [Header("Lobby Elements")]
    public Button JoinButton;
    public Button RefreshNameButton;
    public Button LeaveButton;

    [Space(20)]
    public GameObject SelfPlayerListingPrefab;
    public GameObject OtherPlayerListingPrefab;
    public Transform PlayerListLayout;

    private Dictionary<string, GameObject> _playerListings = new Dictionary<string, GameObject>();

    private void InstantiatePlayerListing(GameObject playerListingPrefab, Player player)
    {
        GameObject playerListing = Instantiate(playerListingPrefab, PlayerListLayout);
        playerListing.name = "PlayerListing: " + player.NickName;
        playerListing.GetComponentInChildren<Text>().text = player.NickName;
        _playerListings.Add(player.UserId, playerListing);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void JoinQuickStartLobby()
    {
        JoinButton.interactable = false;
        RefreshNameButton.interactable = false;
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoomLobby()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No existing lobby found, creating lobby...");
        CreateRoom();
    }

    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, PublishUserId = true};
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        JoinButton.interactable = true;
        RefreshNameButton.interactable = true;
        Debug.LogWarning(message);
    }

    public override void OnJoinedRoom()
    {
        LeaveButton.interactable = true;

        MainMenu.enabled = false;
        LobbyMenu.enabled = true;

        foreach (Player player in PhotonNetwork.PlayerListOthers)
        {
            InstantiatePlayerListing(OtherPlayerListingPrefab, player);
        }

        InstantiatePlayerListing(SelfPlayerListingPrefab, PhotonNetwork.LocalPlayer);
    }

    public override void OnLeftRoom()
    {
        if (JoinButton != null)
        {
            JoinButton.interactable = true;
        }

        if (RefreshNameButton != null)
        {
            RefreshNameButton.interactable = true;
        }

        if (LeaveButton != null)
        {
            LeaveButton.interactable = false;
        }

        if (MainMenu != null)
        {
            MainMenu.enabled = true;
        }

        if (LobbyMenu != null)
        {
            LobbyMenu.enabled = false;
        }

        foreach (KeyValuePair<string, GameObject> player in _playerListings)
        {
            Destroy(player.Value);
        }

        _playerListings.Clear();
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        InstantiatePlayerListing(OtherPlayerListingPrefab, player);
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        Destroy(_playerListings[player.UserId]);
        _playerListings.Remove(player.UserId);
    }
}
