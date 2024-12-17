
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

    void Start()
    {
        currentAmmo = maxAmmo; // Ba�lang��ta 30 mermi
        audioSource = GetComponent<AudioSource>(); // AudioSource b
    }

    void Update()
    {
        ammoText.text="BULLET= "+ currentAmmo +"/30";
        // E�er reloading yap�lm�yorsa ve mermi varsa sol t�kla ate� et
        if (!isReloading && currentAmmo > 0 && Input.GetMouseButton(0))
        {
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
        animator.SetTrigger("ReloadTrigger");
        yield return new WaitForSeconds(reloadTime); // Reload s�resi
        currentAmmo = maxAmmo;
        ammoText.text="BULLET= "+ currentAmmo +"/30";
        isReloading = false;
    }
}
