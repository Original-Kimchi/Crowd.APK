using Photon;
using UnityEngine;

public class PhotonLauncher: PunBehaviour
{
    #region Inspector

    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject progressText;

    #endregion

#if UNITY_EDITOR || UNITY_STANDALONE
    private readonly int maxPlayers = 2;
#else
    private readonly int maxPlayers = 4;
#endif

    private void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;
    }

    public void Connect()
    {
        if (PhotonNetwork.connected)
            PhotonNetwork.JoinRandomRoom();
        else
            PhotonNetwork.ConnectUsingSettings("1");
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
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 }, new TypedLobby());
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if(PhotonNetwork.room.PlayerCount == maxPlayers)
            PhotonNetwork.LoadLevel("MainScene");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
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
