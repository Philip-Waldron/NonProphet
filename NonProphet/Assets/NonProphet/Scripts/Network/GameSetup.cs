using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public static GameSetup Instance;

    public Transform[] SpawnPoints;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
