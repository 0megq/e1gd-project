using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float reloadTime = 1f;
    public float fireRate = 0.15f;
    public int magSize = 20;

    public GameObject bullet;
    public Vector3 bulletSpawnPoint;

    private int currentAmmo;
    private bool isReloading = false;
    private float nextTimeToFire = 0f;

    public UnityEvent shotEvent; 

    void Start()
    {
        currentAmmo = magSize;
    }

    public void Shoot()
    {
        // Don't shoot if reloading
        if (isReloading) return;
        if (Time.time < nextTimeToFire) return;

        // reload if needed
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Fire the gun
        shotEvent.Invoke();

        // restart firecount timer
        nextTimeToFire = Time.time + fireRate;

        // decrease ammo
        currentAmmo--;

        // spawn bullet
        Instantiate(bullet, bulletSpawnPoint, transform.rotation);
    }

    IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magSize;
        isReloading = false;
    }

    public void TryReload()
    {
        if (isReloading) return;
        if (currentAmmo == magSize) return;

        StartCoroutine(Reload());
    }

    public int getCurAmmo(){
        return currentAmmo;
    }
}
