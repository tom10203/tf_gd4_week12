using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float gravity;
    [SerializeField] float moveSpeed;
    [SerializeField] float skinWidth = 0.1f;
    [SerializeField] float verticalRaycastLength;
    [SerializeField] int verticalRayCount = 3;
    [SerializeField] int horizontalRayCount = 3;
    [SerializeField] float jumpAmount;

    BoxCollider2D boxCollider;
    BoxColliderBounds colliderBounds;
    public Vector2 velocity;
    float stepChange;
    float horizontalRaySpacing;
    float verticalRaySpacing;

    public bool isGrounded;
    public bool startCoroutine = true;

    PlayerAnimations animator;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        CalculateRaySpacing();

        animator = GetComponent<PlayerAnimations>();
    }
    void FixedUpdate()
    {
        UpdateBounds();

        velocity.y += gravity * Time.fixedDeltaTime;

        float horizontalInput = Input.GetAxisRaw($"Horizontal");

        if (horizontalInput != 0)
        {
            velocity.x = horizontalInput * moveSpeed * Time.fixedDeltaTime;
        }
        else
        {

            velocity.x = 0f;

        }


        if (velocity.x != 0f)
        {
            CheckHorizontalCollisions();
        }
        if (velocity.y != 0f)
        {
            CheckVerticalCollisions();
        }

        transform.Translate(velocity, Space.World);

    }

    private void Update()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpAmount;
        }

    }

    struct BoxColliderBounds
    {
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
        public Vector2 topLeft;
        public Vector2 topRight;
    }

    void UpdateBounds()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(-skinWidth * 2);

        colliderBounds.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        colliderBounds.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        colliderBounds.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        colliderBounds.topRight = new Vector2(bounds.max.x, bounds.max.y);

        Debug.DrawRay(colliderBounds.bottomLeft, (colliderBounds.bottomRight - colliderBounds.bottomLeft), Color.yellow);
    }



    void CheckVerticalCollisions()
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        bool isHit = false;
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? colliderBounds.bottomLeft : colliderBounds.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, 1 << 6);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                isHit = true;

                if (hit.collider.tag == "FallingPlatform")
                {
                    if (startCoroutine)
                    {
                        Debug.Log($"Fallingplatformhit");
                        FallingPlatform fallingPlatform = hit.collider.GetComponent<FallingPlatform>();
                        fallingPlatform.startCoroutine = true;
                        startCoroutine = false;
                    }
                }

                if (hit.collider.tag == "SpikePit")
                {
                    if (startCoroutine)
                    {
                        Debug.Log($"TakeDamage call");
                        animator.TakeDamage();
                        startCoroutine = false;
                    }
                }

                if (hit.collider.tag == "BumpPlatform")
                {
                    Debug.Log($"BumpPlatformhit");
                    BumpPlatform bp = hit.collider.GetComponent<BumpPlatform>();
                    bp.CallMove();
                }
            }
        }

        isGrounded = isHit ? true : false;
    }


    void CheckHorizontalCollisions()
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        //bool isHit = false;
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? colliderBounds.bottomLeft : colliderBounds.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i + velocity.y);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, 1 << 6);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
            }
        }
    }


    void CalculateRaySpacing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }


}
