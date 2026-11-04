using UnityEngine;

public class WeaponPresenter : MonoBehaviour
{
    public RectTransform firepoint;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        // determine the animation to play based on input and the current selected weapon
        animator.Play("fingergun_fire");
    }
}
