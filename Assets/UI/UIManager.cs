using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI ammoCount;
    public TextMeshProUGUI health;

    // Update is called once per frame
    void Update()
    {
        
    }

    //updates ammo count text
    public void updateAmmo(int curAmmo, int maxAmmo){
        ammoCount.text = "Ammo: " + curAmmo + "/" + maxAmmo;
    }

    //updates player health text
    public void updateHealth(float curHealth, float maxHealth){
        health.text = "Health: " + curHealth + "/" + maxHealth;
    }
}
