using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class playerMovement : MonoBehaviour
{
    float moveSpeed;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float minMoveSpeed;
    float speedDenominator;
    public Vector2 inputDirection,lookDirection;
    Animator anim;

    [SerializeField] float raycastUpLength;
    [SerializeField] float raycastRightLength;
    [SerializeField] float raycastUpOffset;
    [SerializeField] float raycastRightOffset;

    [Header("Dpad Variables")]
    [SerializeField] GameObject dpad;
    [SerializeField] GameObject innerDpad;
    [SerializeField] float radius;

    public bool useDpad = true;
    Vector3 startPos, endPos;

    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;

    [SerializeField] float boxCastHeight;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        //makes the character look down by default
        lookDirection = new Vector2(0, -1);

        speedDenominator = radius / maxMoveSpeed; // using to scale Input vector to moveSpeed

        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //getting input from keyboard controls
        calculateDesktopInputs();

        if (useDpad)
        {
            UseDpad();
        }
        //sets up the animator
        animationSetup();

        CheckForCollisions();
        transform.Translate(inputDirection * moveSpeed * Time.deltaTime);
    }

    void CheckForCollisions()
    {
        Debug.DrawRay(transform.position, Vector3.right * 3, Color.red);
        Debug.DrawRay((Vector3)spriteRenderer.bounds.center, Vector3.right * 3, Color.yellow);
        Debug.DrawRay(transform.position + Vector3.up * boxCollider.size.y * transform.lossyScale.y * 2, Vector3.right * 3, Color.blue);
        Debug.DrawRay(transform.position + Vector3.up * boxCollider.size.y / 2, Vector3.right * 3, Color.green);

        float boxCastWidth = boxCollider.size.x;



    }
    //void CheckForCollisions()
    //{
    //    float x = inputDirection.x < 0 ? -1 : 1;
    //    float y = inputDirection.y < 0 ? -1 : 1;

    //    RaycastHit2D upHit;
    //    RaycastHit2D rightHit;

    //    bool raycastUp1 = Physics2D.Raycast(transform.position + Vector3.up * raycastUpOffset + Vector3.right * raycastRightOffset, Vector3.up, y * raycastUpLength, 1 << 6);
    //    bool raycastUp2 = Physics2D.Raycast(transform.position + Vector3.up * raycastUpOffset - Vector3.right * raycastRightOffset, Vector3.up, y * raycastUpLength, 1 << 6);

    //    Debug.DrawRay(transform.position + Vector3.up * raycastUpOffset + Vector3.right * raycastRightOffset, Vector3.up * y * raycastUpLength, Color.red);
    //    Debug.DrawRay(transform.position + Vector3.up * raycastUpOffset - Vector3.right * raycastRightOffset, Vector3.up * y * raycastUpLength, Color.red);


    //    bool raycastRight1 = Physics2D.Raycast(transform.position, Vector3.right, x * raycastRightLength, 1 << 6);
    //    bool raycastRight2 = Physics2D.Raycast(transform.position + Vector3.up * raycastUpOffset * 2, Vector3.right, x * raycastRightLength, 1 << 6);

    //    Debug.DrawRay(transform.position, Vector3.right * x * raycastRightLength, Color.green);
    //    Debug.DrawRay(transform.position + Vector3.up * raycastUpOffset * 2, Vector2.right * x * raycastRightLength, Color.green);

    //    if (raycastUp1 || raycastUp2)
    //    {
    //        Debug.Log($"raycastUp hit");
    //        inputDirection = new Vector2(inputDirection.x, 0);
    //    }
    //    if (raycastRight1 || raycastRight2)
    //    {
    //        Debug.Log($"raycastRIght hit");
    //        inputDirection = new Vector2(0, inputDirection.y);
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
            
    }

    void calculateDesktopInputs()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        inputDirection = new Vector2(x, y).normalized;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            attack();
        }

    }

    void UseDpad()
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
                dpad.transform.position = startPos;
                dpad.SetActive(true);
            }

            endPos = Input.mousePosition;

            Vector3 direction = endPos - startPos;

            inputDirection = direction.normalized;

            float directionMagnitude = direction.magnitude;
            moveSpeed = Mathf.Clamp(directionMagnitude / speedDenominator, minMoveSpeed, maxMoveSpeed);

            if (directionMagnitude > radius)
            {
                direction = direction.normalized * radius;
            }
            innerDpad.transform.position = startPos + direction;
        }
        else
        {
            dpad.SetActive(false);
        }
    }


    void animationSetup()
    {
        //checking if the player wants to move the character or not
        if (inputDirection.magnitude > 0.1f)
        {
            //changes look direction only when the player is moving, so that we remember the last direction the player was moving in
            lookDirection = inputDirection;

            //sets "isWalking" true. this triggers the walking blend tree
            anim.SetBool("isWalking", true);
        }
        else
        {
            // sets "isWalking" false. this triggers the idle blend tree
            anim.SetBool("isWalking", false);

        }

        //sets the values for input and lookdirection. this determines what animation to play in a blend tree
        anim.SetFloat("inputX", lookDirection.x);
        anim.SetFloat("inputY", lookDirection.y);
        anim.SetFloat("lookX", lookDirection.x);
        anim.SetFloat("lookY", lookDirection.y);
    }

    public void attack()
    {
        anim.SetTrigger("Attack");
    }

    void calculateMobileInput()
    {

    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    print($"collisionStay");
    //    moveSpeed = 0f;
    //    anim.SetBool("isWalking", false);
    //}

}
