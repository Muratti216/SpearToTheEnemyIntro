using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    [Header("Bileşenler")]
    public VideoPlayer videoPlayer;     // Video Player bileşeni
    public GameObject introPanel;       // IntroVideoPlayer objesi
    public GameObject titleScreen;      // TitleScreen objesi

    void Start()
    {
        // Başlangıçta Title Screen kapalı, Intro açık olsun
        introPanel.SetActive(true);
        titleScreen.SetActive(false);

        // Videonun sonuna gelindiğinde VideoBitti fonksiyonunu çalıştır
        videoPlayer.loopPointReached += VideoBitti;
    }

    void Update()
    {
        // İsteğe bağlı: Oyuncu bir tuşa basarsa intro geçilsin
        if (Input.anyKeyDown)
        {
            VideoBitti(videoPlayer);
        }
    }

    void VideoBitti(VideoPlayer vp)
    {
        // 1. Intro objesini kapat
        introPanel.SetActive(false);
        
        // 2. Title Screen grubunu aç (Bu açılınca içindeki Loop video ve Yazı otomatik görünür)
        titleScreen.SetActive(true);
    }
}