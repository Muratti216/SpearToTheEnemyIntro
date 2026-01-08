using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video; // Video kontrolleri için ekledik
using TMPro; // Bu kütüphane yazı rengini değiştirmek için şart!

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Objeleri")]
    // Burayı 'GameObject' yerine 'TextMeshProUGUI' yaptık ki rengine müdahale edebilelim
    public TextMeshProUGUI pressAnyButtonText; 
    
    public GameObject logoVideoObjesi;
    public VideoPlayer videoPlayer; // Video Player referansı    
    public GameObject menuPanel;          

    [Header("Ayarlar")]
    public string oyunSahnesiAdi = "GameScene";
    public float yanipSonmeHizi = 2f; // Yazının yanıp sönme hızı

    [Header("Ses Efektleri")]
    public AudioClip videoBittiSesi; // Video bittiğinde çalacak ses
    [Range(0f, 1f)] public float videoBittiSesiVolume = 1f;
    public AudioClip menuAcildiSesi; // Menü açıldığında çalacak ses
    [Range(0f, 1f)] public float menuAcildiSesiVolume = 1f;
    public float yaziFadeSuresi = 0.5f; // Yazı tıklanınca fade süresi
    public float hizliYanipSure = 0.25f; // Tıklandıktan sonra hızlı yanıp sönme süresi
    public float hizliYanipSonmeHizi = 8f; // Tıklandıktan sonra hızlı yanıp sönme hızı
    private AudioSource audioSource;

    private bool menuAcikMi = false;
    private bool videoOynuyor = true; // Video durumunu takip eder
    private bool yaziFadeCikiyor = false;

    void Start()
    {
        // Başlangıç ayarları
        if (logoVideoObjesi != null) logoVideoObjesi.SetActive(true);
        if (pressAnyButtonText != null) pressAnyButtonText.gameObject.SetActive(false); // Başta gizli
            if (pressAnyButtonText != null)
            {
                pressAnyButtonText.color = new Color(pressAnyButtonText.color.r, pressAnyButtonText.color.g, pressAnyButtonText.color.b, 0f);
            }
        if (menuPanel != null) menuPanel.SetActive(false);

        menuAcikMi = false;
        
        // AudioSource komponentini al veya ekle
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // VideoPlayer referansını otomatik bul (yoksa)
        if (videoPlayer == null && logoVideoObjesi != null)
        {
            videoPlayer = logoVideoObjesi.GetComponent<VideoPlayer>();
        }
        
        // Video bittiğinde çağrılacak eventi ekle
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += VideoBitti;
        }
    }

    void VideoBitti(VideoPlayer vp)
    {
        videoOynuyor = false;

        // Yazıyı hemen göster (sesle aynı anda)
        if (pressAnyButtonText != null)
        {
            pressAnyButtonText.gameObject.SetActive(true);
            pressAnyButtonText.color = new Color(pressAnyButtonText.color.r, pressAnyButtonText.color.g, pressAnyButtonText.color.b, 1f);
        }

        float gecikme = 0f;
        if (audioSource != null && videoBittiSesi != null)
        {
            audioSource.PlayOneShot(videoBittiSesi, videoBittiSesiVolume);
            gecikme = videoBittiSesi.length;
        }

        StartCoroutine(YaziyiSesSonrasiGoster(gecikme));
    }

    System.Collections.IEnumerator YaziyiSesSonrasiGoster(float gecikme)
    {
        if (pressAnyButtonText != null)
        {
            pressAnyButtonText.gameObject.SetActive(true);
            pressAnyButtonText.color = new Color(pressAnyButtonText.color.r, pressAnyButtonText.color.g, pressAnyButtonText.color.b, 1f);
        }

        // Ses sürerken yazı görünsün; gecikme sadece bekleme amaçlı
        if (gecikme > 0f)
            yield return new WaitForSeconds(gecikme);
    }

    void Update()
    {
        // Eğer menü henüz açılmadıysa ve video bittiyse
        if (!menuAcikMi && !videoOynuyor && pressAnyButtonText != null && pressAnyButtonText.gameObject.activeSelf && !yaziFadeCikiyor)
        {
            // 1. YAZIYI YANIP SÖNDÜR (PingPong Efekti)
            // Zamanla 0 ile 1 arasında gidip gelen bir değer üretir
            float alpha = Mathf.PingPong(Time.time * yanipSonmeHizi, 1f);
            // Yazının rengini güncelle (sadece saydamlığı/alpha'yı değiştiriyoruz)
            pressAnyButtonText.color = new Color(pressAnyButtonText.color.r, pressAnyButtonText.color.g, pressAnyButtonText.color.b, alpha);

            // 2. TUŞ KONTROLÜ
            if (Input.anyKeyDown)
            {
                MenuyuAc();
            }
        }
    }

    void MenuyuAc()
    {
        menuAcikMi = true;

        float gecikme = 0f;
        if (audioSource != null && menuAcildiSesi != null)
        {
            audioSource.PlayOneShot(menuAcildiSesi, menuAcildiSesiVolume);
            gecikme = menuAcildiSesi.length;
        }

        StartCoroutine(MenuyuSesSonrasiAc(gecikme));
    }

    System.Collections.IEnumerator MenuyuSesSonrasiAc(float gecikme)
    {
        // Yazıyı fade-out yaparken aynı anda sesi bekle
        float fadeSure = yaziFadeSuresi;
        if (pressAnyButtonText != null)
        {
            yaziFadeCikiyor = true;

            if (hizliYanipSure > 0f)
                yield return StartCoroutine(BlinkText(pressAnyButtonText, hizliYanipSure, hizliYanipSonmeHizi));

            yield return StartCoroutine(FadeOutText(pressAnyButtonText, fadeSure));

            yaziFadeCikiyor = false;
        }

        // Eğer ses süresi fade'den uzunsa kalan kısmı bekle
        float kalanSure = Mathf.Max(0f, gecikme - fadeSure);
        if (kalanSure > 0f)
            yield return new WaitForSeconds(kalanSure);

        // Yazıyı gizle
        if (pressAnyButtonText != null) pressAnyButtonText.gameObject.SetActive(false);
        
        // Videoyu kapat
        if (logoVideoObjesi != null) logoVideoObjesi.SetActive(false);

        // Menüyü aç
        if (menuPanel != null) menuPanel.SetActive(true); 
    }

    System.Collections.IEnumerator BlinkText(TextMeshProUGUI tmp, float sure, float hiz)
    {
        if (tmp == null)
            yield break;

        float zaman = 0f;
        while (zaman < sure)
        {
            zaman += Time.deltaTime;
            float alpha = Mathf.PingPong(zaman * hiz, 1f);
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alpha);
            yield return null;
        }

        // Fade öncesi tam görünür yap
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 1f);
    }

    System.Collections.IEnumerator FadeOutText(TextMeshProUGUI tmp, float sure)
    {
        yaziFadeCikiyor = true;

        if (tmp == null)
        {
            yaziFadeCikiyor = false;
            yield break;
        }

        float baslangicAlpha = tmp.color.a;
        float zaman = 0f;
        while (zaman < sure)
        {
            zaman += Time.deltaTime;
            float t = Mathf.Clamp01(zaman / sure);
            float alpha = Mathf.Lerp(baslangicAlpha, 0f, t);
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alpha);
            yield return null;
        }

        // Net sıfırla
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, 0f);
        yaziFadeCikiyor = false;
    }

    public void OyunuBaslat()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OyundanCik()
    {
        Application.Quit();
        Debug.Log("Çıkış yapıldı.");
    }

    void OnDestroy()
    {
        // Event'i temizle
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= VideoBitti;
        }
    }
}