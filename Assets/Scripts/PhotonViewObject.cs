using UnityEngine;

public abstract class PhotonViewObject: MonoBehaviour
{
    public PhotonView PhotonView { get; private set; }

    protected void Awake()
    {
        PhotonView = gameObject.GetComponent<PhotonView>();
    }
}
