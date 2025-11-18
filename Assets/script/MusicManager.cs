using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio")]
    public AudioSource musicSource;  // AudioSource con la música de fondo

    private void Awake()
    {
        // Singleton: solo puede haber uno
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null)
            musicSource = GetComponent<AudioSource>();

        // Configuración básica
        if (musicSource != null)
        {
            musicSource.loop = true;

            if (!musicSource.isPlaying && musicSource.clip != null)
                musicSource.Play();

            // Volumen inicial
            if (musicSource.volume <= 0f)
                musicSource.volume = 0.5f;
        }
    }

    // Esta función la llamará el Slider
    public void OnSliderValueChanged(float value)
    {
        if (musicSource != null)
        {
            musicSource.volume = value;
            // 👇 nunca paramos la música aquí
            if (!musicSource.isPlaying && musicSource.clip != null && value > 0f)
            {
                musicSource.Play();
            }
        }
    }
}
