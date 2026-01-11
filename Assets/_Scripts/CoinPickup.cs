using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [Header("Görsel ve Ses Ayarları")]
    public GameObject explosionVFX; // Oluşturduğun Prefab'ı buraya atacağız
    public AudioClip collectSound;  // Para sesini buraya atacağız
    [Range(0, 1)] public float sesSiddeti = 0.5f; // Sesi kısmak istersen diye

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Çarpan şeyin Oyuncu (Player) olup olmadığını kontrol et
        // Not: Player objenin Tag'ini "Player" yapmayı unutma!
        if (other.CompareTag("Player"))
        {
            Topla();
        }
    }

    void Topla()
    {
        if (explosionVFX != null)
        {
            Debug.Log("Efekt Yaratıldı!"); // <--- Bunu ekle
            GameObject vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            Destroy(vfx, 2.0f); 
        }
        // 1. SESİ ÇAL (Özel Yöntem)
        // Normal AudioSource kullanırsak, obje yok olunca ses de kesilir.
        // PlayClipAtPoint, ses için geçici bir hoparlör yaratır ve işi bitince yok eder.
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position, sesSiddeti);
        }

        // 2. EFEKTİ YARAT
        if (explosionVFX != null)
        {
            // Efekti paranın olduğu pozisyonda oluştur
            GameObject vfx = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            
            // Efekt oynadıktan 2 saniye sonra sahne kirlenmesin diye silsin
            Destroy(vfx, 2.0f); 
        }

        // 3. PARAYI YOK ET
        Destroy(gameObject);
    }
}