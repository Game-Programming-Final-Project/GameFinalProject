
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    public Animator animator;
    public GameObject bulletPrefab; // Mermi prefab'�
    public float bulletSpeed = 10f; // Mermi h�z�
    public int maxAmmo = 30; // Maksimum mermi say�s�
    private int currentAmmo; // �u anki mermi say�s�
    public float reloadTime = 1f; // Reload s�resi
    private bool isReloading = false;
    public float bulletSpawnHeight = 3f;
    public float gunRange = 10f;
    public float fireRate = 0.2f; // Ate� etme gecikmesi
    private float nextFireTime = 0f;
    private AudioSource audioSource;
    public TextMeshProUGUI ammoText;
    private FinanceManager financeManager;
    public int damage = 10;


    void Start()
    {
        financeManager = FindObjectOfType<FinanceManager>();
        currentAmmo = maxAmmo; // Ba�lang��ta 30 mermi
        audioSource = GetComponent<AudioSource>(); // AudioSource b

    }

    void Update()
    {
        
        // E�er reloading yap�lm�yorsa ve mermi varsa sol t�kla ate� et
        if (!isReloading && currentAmmo > 0 && Input.GetMouseButton(0))
        {
            ammoText.text = "BULLET= " + currentAmmo + "/" + maxAmmo;
            if (Time.time >= nextFireTime)
            {
                FireBullet();
                currentAmmo--;
                nextFireTime = Time.time + fireRate; // Bir sonraki ate� etme zaman�
            }
        }

        // E�er mermiler bitmi�se veya R tu�una bas�lm��sa reload i�lemi
        if (Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0)
        {
            if (!isReloading)
                StartCoroutine(Reload());
        }
    }

    // Mouse y�n�ne ate� etme
    void FireBullet()
    {
        // Player'�n y�n�n� al (bakt��� y�n)
        Vector3 direction = transform.forward;  // Oyuncunun bakt��� y�n (3D oyun i�in)
        animator.SetTrigger("ShootTrigger");
        // Mermiyi instantiate et
        GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, bulletSpawnHeight, 0), Quaternion.identity);

        // Merminin Rigidbody'sini al
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Mermiyi oyuncunun bakt��� y�nde hareket ettir
            rb.velocity = direction * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Rigidbody component not found on bullet prefab!");
        }
        Destroy(bullet, gunRange);
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }




    // Reload i�lemi
    IEnumerator Reload()
    {
        isReloading = true;
        ammoText.text = "RELOADING...";
        animator.SetTrigger("ReloadTrigger");
        yield return new WaitForSeconds(reloadTime); // Reload s�resi
        currentAmmo = maxAmmo;
        ammoText.text = "BULLET= " + currentAmmo + "/30";
        isReloading = false;
    }


    public void BuyIncreaseMaxBullet(int cost)
    {
        if (financeManager != null && financeManager.SpendSoul(cost))
        {
            maxAmmo += 5;
            ammoText.text = "BULLET= " + currentAmmo + "/" + maxAmmo;
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

            Debug.Log("Max damage increased by +5!");
        }
        else
        {
            Debug.Log("Not enough souls to increase Max Health!");
        }
    }
    public void BuyIncreaseFireRate(int cost)
    {
        if (financeManager != null && financeManager.SpendSoul(cost))
        {
            fireRate -= ((fireRate*2)/10);

            Debug.Log("Firerate increased by %30!");
        }
        else
        {
            Debug.Log("Not enough souls to increase Max Health!");
        }
    }

    public int GetDamage()
    {
        return damage;
    }

}

