using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    PlayerController pController;
    PlayerShooting pShooting;
    [SerializeField] WeaponPresenter presenter;
    UIManager ui;

    void Start(){
        pController = player.GetComponent<PlayerController>();
        pShooting = player.GetComponent<PlayerShooting>();
        pShooting.gun.shotEvent.AddListener(WeaponFired);
        ui = GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ui.updateAmmo(pShooting.gun.getCurAmmo(), pShooting.gun.magSize);
        ui.updateHealth(pController.getCurHealth(), pController.getMaxHealth());
        // update the bullet spawn point
        pShooting.gun.bulletSpawnPoint = pController.cam.ScreenToWorldPoint(presenter.firepoint.position + Vector3.forward * 0.5f);
    }

    void WeaponFired()
    {
        presenter.PlayAnimation();
    }
}
