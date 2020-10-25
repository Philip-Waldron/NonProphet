using UnityEngine;

public class PlayRandomMusic : MonoBehaviour
{
    public AudioSource MusicPlayer;
    public AudioClip[] Songs;
    private int _songIndex;

    private void Start()
    {
        _songIndex = Random.Range(0, Songs.Length);
        MusicPlayer.clip = Songs[_songIndex];
        MusicPlayer.Play();
    }

    private void Update()
    {
        if (!MusicPlayer.isPlaying)
        {
            _songIndex++;
            if (_songIndex >= Songs.Length)
            {
                _songIndex = 0;
            }

            MusicPlayer.clip = Songs[_songIndex];
            MusicPlayer.Play();
        }
    }
}
