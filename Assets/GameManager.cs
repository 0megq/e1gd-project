using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    PlayerController pController;
    PlayerShooting pShooting;
    [SerializeField] WeaponPresenter presenter;
    UIManager ui;
    SpawnManager spawner;
    int waveNum = 1;
    int enemyCount = 0;

    void Start(){
        pController = player.GetComponent<PlayerController>();
        pShooting = player.GetComponent<PlayerShooting>();
        pShooting.gun.shotEvent.AddListener(WeaponFired);
        ui = GetComponent<UIManager>();
        spawner = GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(spawner != null && enemyCount == 0)
        {
            spawner.spawn(waveNum);
        }

        if (ui != null) {
            ui.updateAmmo(pShooting.gun.getCurAmmo(), pShooting.gun.magSize);
            ui.updateHealth(pController.getCurHealth(), pController.getMaxHealth());
        }
        presenter.SetBullets(pShooting.gun.getCurAmmo() > 0);

        // update the bullet spawn point
        pShooting.gun.bulletSpawnPoint = pController.cam.ScreenToWorldPoint(presenter.firepoint.position + Vector3.forward * pShooting.gun.bullet.GetComponent<Bullet>().distanceFromPlayer);
    }

    void WeaponFired()
    {
        presenter.Fire();
    }
}
