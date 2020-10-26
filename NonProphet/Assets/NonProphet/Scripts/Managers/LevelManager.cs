using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public Transform[] SpawnPoints;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
