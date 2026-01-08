using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float hiz = 5f;          // Karakterin yürüme hızı
    public float ziplamaGucu = 10f; // Zıplama kuvveti
    
    private Rigidbody2D rb;
    private bool yerdeMi = false;   // Karakter yere değiyor mu?

    void Start()
    {
        // Karakterin üzerindeki fizik bileşenini alıyoruz
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // --- Hareket ---
        // Klavyeden sağ/sol ok tuşlarını veya A/D tuşlarını okur (-1 ile 1 arası değer)
        float yatayHareket = Input.GetAxis("Horizontal");

        // Hızı karaktere uygula (Y eksenini ellemiyoruz ki düşmeye devam etsin)
        rb.linearVelocity = new Vector2(yatayHareket * hiz, rb.linearVelocity.y);

        // --- Zıplama ---
        // Space tuşuna basıldıysa VE karakter yerdeyse
        if (Input.GetButtonDown("Jump") && yerdeMi)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, ziplamaGucu);
            yerdeMi = false; // Zıpladığı an artık yerde değildir
        }
    }

    // Karakter bir şeye çarptığında çalışır
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Eğer çarptığımız şeyin etiketi "Zemin" ise
        if (collision.gameObject.CompareTag("Zemin"))
        {
            yerdeMi = true; // Tekrar zıplayabilir
        }
    }
}