using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CoinCollector : MonoBehaviour
{
    [Header("Score Settings")]
    [SerializeField] private int toplamPara = 0;
    [SerializeField] private int baslangicDegeri = 1;          // Her para başlangıçta kaç puan verir
    [SerializeField] private int herToplamadaArtis = 0;        // Her toplamada para değeri ne kadar artsın

    [Header("UI (Opsiyonel)")]
    [SerializeField] private TMP_Text paraMetni;               // Para sayısını göstermek için UI
    [SerializeField] private string yaziOnEki = "Para Miktarı: ";      // UI metni için önek

    [Header("Ses Efektleri")]
    [SerializeField] private AudioClip coinToplamaSesi;         // Para toplanırken çalınacak ses

    public UnityEvent<int> ParaDegisti;                        // UI veya diğer sistemlere haber ver

    private int guncelParaDegeri;

    private void Awake()
    {
        guncelParaDegeri = baslangicDegeri;
        GuncelleUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sadece "Coin" etiketli, trigger durumundaki colliderlara tepki ver
        if (!other.CompareTag("Coin"))
            return;

        ParaTopla(other.gameObject);
    }

    private void ParaTopla(GameObject coinObjesi)
    {
        toplamPara += guncelParaDegeri;

        // Sonraki toplama için değeri artır (0 ise sabit kalır)
        if (herToplamadaArtis > 0)
        {
            guncelParaDegeri += herToplamadaArtis;
        }

        ParaDegisti?.Invoke(toplamPara);
        GuncelleUI();

        // Ses efektini çal
        if (coinToplamaSesi != null)
        {
            AudioSource.PlayClipAtPoint(coinToplamaSesi, transform.position);
        }

        // Varsa animasyon tetikle, ardından nesneyi yok et
        var animator = coinObjesi.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Collect");
            Destroy(coinObjesi, 0.35f);
        }
        else
        {
            Destroy(coinObjesi);
        }
    }

    private void GuncelleUI()
    {
        if (paraMetni == null)
            return;

        paraMetni.text = yaziOnEki + toplamPara;
    }
}
