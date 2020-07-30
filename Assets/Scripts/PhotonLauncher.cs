using Photon;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLauncher: PunBehaviour
{
    #region Inspector

    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject progressText;
    [SerializeField] private InputField nameInputField;

    #endregion

#if UNITY_EDITOR || UNITY_STANDALONE
    private readonly int maxPlayers = 2;
#else
    private readonly int maxPlayers = 4;
#endif

    private event Action JoinRoomEvent;

    private void Awake()
    {
#if UNITY_STANDALONE
        Screen.SetResolution(720, 1280, false);
#endif
        PhotonNetwork.automaticallySyncScene = true;
        JoinRoomEvent += () =>
        {
            controlPanel.SetActive(false);
            progressText.SetActive(true);
        };
        PhotonNetwork.ConnectUsingSettings("1");

        nameInputField.text = PlayerPrefs.HasKey("Name") ? PlayerPrefs.GetString("Name") : "Player";
    }

    public void Connect()
    {
        if (string.IsNullOrEmpty(nameInputField.text))
            return;
        PhotonNetwork.player.NickName = nameInputField.text;
        StartCoroutine(CoConnect());
    }

    private IEnumerator CoConnect()
    {
        yield return new WaitUntil(() => PhotonNetwork.connectedAndReady);
        PhotonNetwork.JoinRandomRoom();
    }

#region PUN Callbacks

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
    }

    public override void OnDisconnectedFromPhoton()
    {
        base.OnDisconnectedFromPhoton();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        base.OnPhotonRandomJoinFailed(codeAndMsg);
        Debug.Log($"Launcher/OnPhotonRandomJoinFailed() called by PUN. Now we create room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = (byte)maxPlayers }, new TypedLobby());
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        JoinRoomEvent();
        if(PhotonNetwork.room.PlayerCount == maxPlayers)
            PhotonNetwork.LoadLevel("MainScene");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        JoinRoomEvent();
        Debug.Log($"Launcher/OnCreatedRoom() called by PUN.");
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        base.OnPhotonPlayerConnected(newPlayer);
        if (PhotonNetwork.room.PlayerCount == maxPlayers)
        {
            PhotonNetwork.LoadLevel("MainScene");
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        base.OnPhotonPlayerDisconnected(otherPlayer);
    }
#endregion
}
