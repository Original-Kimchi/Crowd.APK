using System.Linq;
using UnityEngine;

public class Game: MonoBehaviour
{
    #region Unity Event Functions

    private void OnEnable()
    {
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
        if(PhotonNetwork.isMasterClient)
        {
            foreach (var _ in Enumerable.Range(1, 50))
            {
                var go = PhotonNetwork.Instantiate("Food", Vector3.zero, Quaternion.identity, 0);
                go.AddComponent<Food>();
                go.AddComponent<FoodRotation>();
            }
        }
    }

    private void Update()
    {
        while (ObjectBox.ObjectExist)
            ObjectBox.Dequeue();
    }

    #endregion
}
