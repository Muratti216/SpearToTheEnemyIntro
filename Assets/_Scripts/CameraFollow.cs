using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Hedef")]
    public Transform target; // Takip edilecek oyuncu

    [Header("Ayarlar")]
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Kamera uzaklığı
    public float smoothTime = 0.15f; // Takip yumuşatma süresi
    public bool lockY = false; // Y eksenini kilitlemek istersen

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null)
            return;

        var desiredPosition = target.position + offset;
        if (lockY)
            desiredPosition.y = transform.position.y;

        // Pozisyonu yumuşak geçişle güncelle
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}
