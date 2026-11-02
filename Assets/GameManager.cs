using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    PlayerController pController;
    PlayerShooting pShooting;
    UIManager ui;

    void Start(){
        pController = player.GetComponent<PlayerController>();
        pShooting = player.GetComponent<PlayerShooting>();
        ui = GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ui.updateAmmo(pShooting.gun.getCurAmmo(), pShooting.gun.magSize);
        ui.updateHealth(pController.getCurHealth(), pController.getMaxHealth());
    }
}
