using UnityEngine;

public class WeaponPresenter : MonoBehaviour
{
    public RectTransform firepoint;
    Animator animator;
    public PresenterType type;

    public enum PresenterType {
        Megaphone,
        Fingergun
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        if (type == PresenterType.Megaphone) animator.Play("megaphone_idle");
    }

    public void Fire()
    {
        // determine the animation to play based on input and the current selected weapon
        animator.SetTrigger("fire");
    }

    public void Reload()
    {
        animator.SetTrigger("beginReload");
    }
    
    public void SetBullets(bool hasBullets)
    {
        animator.SetBool("hasBullets", hasBullets);
    }
}
