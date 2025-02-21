using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerController))]
public class PlayerAnimations : MonoBehaviour
{
    Animator animator;
    PlayerController playerController;
    Sprite originalSprite;
    [SerializeField] Sprite jumpUp;
    [SerializeField] Sprite jumpDown;

    float lookDirection = 0;
    private void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw($"Horizontal");

        if (horizontalInput < 0)
        {
            lookDirection = -1f;
        }
        else if (horizontalInput > 0) 
        {
            lookDirection = 0f;
        }

        transform.eulerAngles = new Vector3(0, 180 * lookDirection, 0);


        if (playerController.isGrounded)
        {
            animator.SetBool("JumpUp", false);
            animator.SetBool("JumpDown", false);

            if (horizontalInput != 0f)
            {
                animator.SetBool("Walk", true);
            }
            else
            {
                animator.SetBool("Walk", false);
               
            }
        }
        else
        {
            if (playerController.velocity.y < 0)
            {
                animator.SetBool("JumpDown", true);
            }
            else if (playerController.velocity.y > 0)
            {
                animator.SetBool("JumpUp", true);  
            }
        }
        
    }
    public void TakeDamage()
    {
        animator.SetTrigger("TakeDamage");
    }
}
