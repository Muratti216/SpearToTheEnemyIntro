using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float hiz = 5f;          // Karakterin yürüme hızı
    public float ziplamaGucu = 10f; // Zıplama kuvveti
    public GameObject dustEffect;   // Karakter yürürken görünen toz efekti
    
    private Rigidbody2D rb;
    private SpriteRenderer benimSpriteim;
    private bool yerdeMi = false;   // Karakter yere değiyor mu?
    private float yatayHareketInput;
    private bool jumpPressed;

    void Start()
    {
        
        benimSpriteim = GetComponent<SpriteRenderer>();
        // Karakterin üzerindeki fizik bileşenini alıyoruz
        rb = GetComponent<Rigidbody2D>();

        // Kamera takip ve görsel takılmaları azaltmak için fizik interpolasyonu aç
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Update()
    {
        // --- Hareket ---
        // Yatay hareket girdisi (+1 sağ, -1 sol)
        yatayHareketInput = Input.GetAxisRaw("Horizontal");

        // Zıplama isteğini sadece input okuma fazında al
        if (Input.GetButtonDown("Jump") && yerdeMi)
        {
            jumpPressed = true;
        }

        // --- Yönü Döndürme ---
        // Tüm child objelerle birlikte karakteri Y ekseninde çevir
        if (yatayHareketInput > 0)
        {
            // Karakteri orijinal yönüne (0 derece) döndür
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (yatayHareketInput < 0)
        {
            // Karakteri Y ekseninde 180 derece çevir (Arkasına dönsün)
            // Bu işlem karaktere bağlı olan toz efektini, silahı vs. her şeyi döndürür.
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    void FixedUpdate()
    {
        // Hızı karaktere uygula (Y eksenini ellemiyoruz ki düşmeye devam etsin)
        rb.velocity = new Vector2(yatayHareketInput * hiz, rb.velocity.y);

        // --- Zıplama ---
        if (jumpPressed && yerdeMi)
        {
            rb.velocity = new Vector2(rb.velocity.x, ziplamaGucu);
            jumpPressed = false;
            yerdeMi = false;

            // Havaya çıktığında toz efektini kapat
            if (dustEffect != null)
                dustEffect.SetActive(false);
        }
        else
        {
            jumpPressed = false;
        }
    }

    // Karakter bir şeye çarptığında çalışır
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Eğer çarptığımız şeyin etiketi "Zemin" ise
        if (collision.gameObject.CompareTag("Zemin"))
        {
            yerdeMi = true; // Tekrar zıplayabilir

            // Yere indiğinde toz efektini tekrar aç
            if (dustEffect != null)
                dustEffect.SetActive(true);
        }
    }
}