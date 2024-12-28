using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    public Animator animator;
    public GameObject bulletPrefab; // Mermi prefab'ı
    public float bulletSpeed = 10f; // Mermi hızı
    public int maxAmmo = 30; // Maksimum mermi sayısı
    private int currentAmmo; // Şu anki mermi sayısı
    public float reloadTime = 2.5f; // Reload süresi
    private bool isReloading = false;
    public float bulletSpawnHeight = 3f;
    public float gunRange = 10f;
    public float fireRate = 0.2f; // Ateş etme gecikmesi
    private float nextFireTime = 0f;
    private AudioSource audioSource;
    public TextMeshProUGUI ammoText;
    private FinanceManager financeManager;
    public int damage = 10;
    public AudioClip reloadSound; // Reload ses klibi
    public bool isGameActive = false; // Oyunun aktif olup olmadığını kontrol eden değişken

    void Start()
    {
        financeManager = FindObjectOfType<FinanceManager>();
        currentAmmo = maxAmmo; // Başlangıçta maksimum mermi
        audioSource = GetComponent<AudioSource>(); // AudioSource bileşenini al
        UpdateAmmoText();
    }

    void Update()
    {
       

        // Sol tık ile ateş
        if (!isReloading && currentAmmo > 0 && Input.GetMouseButton(0))
        {
            if (Time.time >= nextFireTime)
            {
                FireBullet();
                currentAmmo--;
                nextFireTime = Time.time + fireRate; // Bir sonraki ateş etme zamanı
                UpdateAmmoText();
            }
        }

        // Eğer mermiler bitmişse veya R tuşuna basılmışsa reload işlemi
        if ((Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0) && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    void FireBullet()
    {
        // Player'ın baktığı yön
        Vector3 direction = transform.forward;

        animator.SetTrigger("ShootTrigger");

        // Mermiyi instantiate et
        GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, bulletSpawnHeight, 0), Quaternion.identity);

        // Merminin Rigidbody'sini al
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Mermiyi hareket ettir
            rb.velocity = direction * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Rigidbody component not found on bullet prefab!");
        }

        // Mermiyi yok et
        Destroy(bullet, gunRange);

        // Mermi sesi çal
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        animator.SetTrigger("ReloadTrigger");
        ammoText.text = "RELOADING...";
        if (audioSource != null && reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        UpdateAmmoText();
        isReloading = false;
    }

    public void BuyIncreaseMaxBullet(int cost)
    {
        if (financeManager != null && financeManager.SpendSoul(cost))
        {
            maxAmmo += 5;
            UpdateAmmoText();
            Debug.Log("Max Bullet increased by +5!");
        }
        else
        {
            Debug.Log("Not enough souls to increase Max Health!");
        }
    }

    public void BuyIncreaseDamage(int cost)
    {
        if (financeManager != null && financeManager.SpendSoul(cost))
        {
            damage += 2;
            Debug.Log("Damage increased by +2!");
        }
        else
        {
            Debug.Log("Not enough souls to increase damage!");
        }
    }

    public void BuyIncreaseFireRate(int cost)
    {
        if (financeManager != null && financeManager.SpendSoul(cost))
        {
            fireRate -= ((fireRate * 2) / 10);
            Debug.Log("Fire rate increased by %20!");
        }
        else
        {
            Debug.Log("Not enough souls to increase fire rate!");
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    public void StartGame()
    {
        isGameActive = true;
    }

    public void StopGame()
    {
        isGameActive = false;
    }

    private void UpdateAmmoText()
    {
        ammoText.text = "BULLET= " + currentAmmo + "/" + maxAmmo;
    }
}
