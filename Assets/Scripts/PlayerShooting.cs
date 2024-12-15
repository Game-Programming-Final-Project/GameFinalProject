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

    void Start()
    {
        currentAmmo = maxAmmo; // Baþlangýçta 30 mermi
    }

    void Update()
    {
        // Eðer reloading yapýlmýyorsa ve mermi varsa sol týkla ateþ et
        if (!isReloading && currentAmmo > 0 && Input.GetMouseButtonDown(0))
        {
            FireBullet();
            currentAmmo--;
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
        // Player'ýn yönünü al (yani karakterin baktýðý yön)
        Vector3 direction = transform.up;  // Player'ýn "forward" yönü (baktýðý yön)

        // Mermiyi instantiate et
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Rigidbody3D bileþenini al
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Mermiyi player'ýn baktýðý yönde hareket ettir
            rb.velocity = direction * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Rigidbody component not found on bullet prefab!");
        }
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
