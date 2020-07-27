using System.Linq;
using UnityEngine;

public class Game: MonoBehaviour
{
    private readonly int mapSize = 500;
    public GameObject PlayerObject { get; private set; }

    #region Unity Event Functions

    private void OnEnable()
    {
        PlayerObject = PhotonNetwork.Instantiate("Player", GetEmptyLocation(), Quaternion.identity, 0);

        if(PhotonNetwork.isMasterClient)
        {
            foreach (var _ in Enumerable.Range(1, 50))
            {
                var go = PhotonNetwork.Instantiate("Food", GetEmptyLocation(), Quaternion.identity, 0);
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

    public Vector3 GetEmptyLocation()
    {
        while (true)
        {
            int x = Random.Range(-mapSize / 2, mapSize / 2);
            int z = Random.Range(-mapSize / 2, mapSize / 2);

            Physics.Raycast(new Vector3(x, 1 << 5, z), Vector3.down, out RaycastHit hit);
            if (hit.collider.CompareTag("Floor"))
                return new Vector3(x, 1, z);
        }
    }
}
