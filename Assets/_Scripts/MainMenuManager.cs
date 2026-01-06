using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Bu kütüphane yazı rengini değiştirmek için şart!

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Objeleri")]
    // Burayı 'GameObject' yerine 'TextMeshProUGUI' yaptık ki rengine müdahale edebilelim
    public TextMeshProUGUI pressAnyButtonText; 
    
    public GameObject logoVideoObjesi;    
    public GameObject menuPanel;          

    [Header("Ayarlar")]
    public string oyunSahnesiAdi = "GameScene";
    public float yanipSonmeHizi = 2f; // Yazının yanıp sönme hızı

    private bool menuAcikMi = false;

    void Start()
    {
        // Başlangıç ayarları
        if (logoVideoObjesi != null) logoVideoObjesi.SetActive(true);
        if (pressAnyButtonText != null) pressAnyButtonText.gameObject.SetActive(true);
        if (menuPanel != null) menuPanel.SetActive(false);

        menuAcikMi = false;
    }

    void Update()
    {
        // Eğer menü henüz açılmadıysa (yani Press Any Button ekranındaysak)
        if (!menuAcikMi)
        {
            // 1. YAZIYI YANIP SÖNDÜR (PingPong Efekti)
            if (pressAnyButtonText != null)
            {
                // Zamanla 0 ile 1 arasında gidip gelen bir değer üretir
                float alpha = Mathf.PingPong(Time.time * yanipSonmeHizi, 1f);
                // Yazının rengini güncelle (sadece saydamlığı/alpha'yı değiştiriyoruz)
                pressAnyButtonText.color = new Color(pressAnyButtonText.color.r, pressAnyButtonText.color.g, pressAnyButtonText.color.b, alpha);
            }

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

        // Yazıyı gizle
        if (pressAnyButtonText != null) pressAnyButtonText.gameObject.SetActive(false);
        
        // Videoyu kapat
        if (logoVideoObjesi != null) logoVideoObjesi.SetActive(false);

        // Menüyü aç
        if (menuPanel != null) menuPanel.SetActive(true); 
    }

    public void OyunuBaslat()
    {
        SceneManager.LoadScene(oyunSahnesiAdi);
    }

    public void OyundanCik()
    {
        Application.Quit();
        Debug.Log("Çıkış yapıldı.");
    }
}