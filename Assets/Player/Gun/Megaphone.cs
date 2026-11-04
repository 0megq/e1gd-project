using UnityEngine;

public class Megaphone : Gun
{
    public Vector3 spawnPos;

    public void shoot(){
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
        Instantiate(bullet, spawnPos, Camera.main.transform.localRotation);
    }

    void Update(){
        spawnPos = Camera.main.transform.forward * 2;
    }
}
