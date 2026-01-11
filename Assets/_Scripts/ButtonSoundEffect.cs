using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundEffect : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Ses Klipleri")]
    public AudioClip hoverClip;
    public AudioClip clickClip;

    [Header("Ses Ayarları")]
    [Range(0f, 1f)] public float hoverVolume = 1f;
    [Range(0f, 1f)] public float clickVolume = 1f;

    [Tooltip("Varsayılan AudioSource kullanılacaksa boş bırakılabilir.")]
    public AudioSource customAudioSource;

    private AudioSource source;

    void Awake()
    {
        // Tercihen verilen kaynağı kullan, yoksa kendin oluştur
        source = customAudioSource != null ? customAudioSource : GetComponent<AudioSource>();
        if (source == null)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cal(hoverClip, hoverVolume);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Cal(clickClip, clickVolume);
    }

    private void Cal(AudioClip clip, float volume)
    {
        if (clip == null || source == null)
            return;

        source.PlayOneShot(clip, volume);
    }
}
