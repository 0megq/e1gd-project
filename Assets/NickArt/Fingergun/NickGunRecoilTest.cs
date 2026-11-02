using UnityEngine;

public class NickGunRecoilTest : MonoBehaviour
{

    [SerializeField] GameObject rest;
    [SerializeField] GameObject recoil;

    float timeSinceShoot = 0.0f;

    void OnShoot() {
        if (timeSinceShoot > 0.0f) {
            return;
        }

        Shake.shake = true;
        timeSinceShoot = 0.2f;
    }

    void Update() {
        if (timeSinceShoot > 0.0f) {
            rest.SetActive(false);
            recoil.SetActive(true);
        } else {
            rest.SetActive(true);
            recoil.SetActive(false);
        }
        timeSinceShoot -= Time.deltaTime;
    }
}
