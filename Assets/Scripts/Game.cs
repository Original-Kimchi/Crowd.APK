using UnityEngine;

public class Game: MonoBehaviour
{
    #region Unity Event Functions

    private void OnEnable()
    {
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
    }

    
    #endregion
}