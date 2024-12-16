
using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Mermi prefab'ý
    public float bulletSpeed = 10f; // Mermi hýzý
    public int maxAmmo = 30; // Maksimum mermi sayýsý
    private int currentAmmo; // Þu anki mermi sayýsý
    public float reloadTime = 1f; // Reload süresi
    private bool isReloading = false;
    public float bulletSpawnHeight = 3f;
    public float gunRange = 10f;
    public float fireRate = 0.2f; // Ateþ etme gecikmesi
    private float nextFireTime = 0f;

    void Start()
    {
        currentAmmo = maxAmmo; // Baþlangýçta 30 mermi
    }

    void Update()
    {
        // Eðer reloading yapýlmýyorsa ve mermi varsa sol týkla ateþ et
        if (!isReloading && currentAmmo > 0 && Input.GetMouseButton(0))
        {
            if (Time.time >= nextFireTime)
            {
                FireBullet();
                currentAmmo--;
                nextFireTime = Time.time + fireRate; // Bir sonraki ateþ etme zamaný
            }
        }

        // Eðer mermiler bitmiþse veya R tuþuna basýlmýþsa reload iþlemi
        if (Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0)
        {
            if (!isReloading)
                StartCoroutine(Reload());
        }
    }

    // Mouse yönüne ateþ etme
    void FireBullet()
    {
        // Player'ýn yönünü al (baktýðý yön)
        Vector3 direction = transform.forward;  // Oyuncunun baktýðý yön (3D oyun için)

        // Mermiyi instantiate et
        GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, bulletSpawnHeight, 0), Quaternion.identity);

        // Merminin Rigidbody'sini al
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Mermiyi oyuncunun baktýðý yönde hareket ettir
            rb.velocity = direction * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Rigidbody component not found on bullet prefab!");
        }
        Destroy(bullet, gunRange);
    }




    // Reload iþlemi
    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime); // Reload süresi
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
