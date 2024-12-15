using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Mermi prefab'�
    public float bulletSpeed = 10f; // Mermi h�z�
    public int maxAmmo = 30; // Maksimum mermi say�s�
    private int currentAmmo; // �u anki mermi say�s�
    public float reloadTime = 1f; // Reload s�resi
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo; // Ba�lang��ta 30 mermi
    }

    void Update()
    {
        // E�er reloading yap�lm�yorsa ve mermi varsa sol t�kla ate� et
        if (!isReloading && currentAmmo > 0 && Input.GetMouseButtonDown(0))
        {
            FireBullet();
            currentAmmo--;
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
        // Player'�n y�n�n� al (yani karakterin bakt��� y�n)
        Vector3 direction = transform.up;  // Player'�n "forward" y�n� (bakt��� y�n)

        // Mermiyi instantiate et
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Rigidbody3D bile�enini al
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Mermiyi player'�n bakt��� y�nde hareket ettir
            rb.velocity = direction * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Rigidbody component not found on bullet prefab!");
        }
    }



    // Reload i�lemi
    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime); // Reload s�resi
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
