using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressText: MonoBehaviour
{
    #region Inspector

    [SerializeField] private float repeatTime = 1f;

    #endregion

    private Text progressText;
    private readonly string[] strPool = { "Waitng.", "Waiting..", "Waiting..." };

    #region Unity Event Functions

    private void Awake()
    {
        progressText = gameObject.GetComponent<Text>();
    }

    private void OnEnable()
    {
        StartCoroutine(TextAnimation());
    }

    #endregion

    private IEnumerator TextAnimation()
    {
        int i = 0;
        while (PhotonNetwork.room.PlayerCount != PhotonNetwork.room.MaxPlayers)
        {
            yield return new WaitForSecondsRealtime(repeatTime);
            progressText.text = strPool[i++ % strPool.Length];
        }
    }
}
