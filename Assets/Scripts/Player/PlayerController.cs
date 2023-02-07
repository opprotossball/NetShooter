using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform feetPos;
    [SerializeField] private float checkRadius;
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    private LayerMask whatIsGround;
    public bool isGrounded;
    private int runningDirection = 0;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PlayerScript playerScript;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerScript = GetComponent<PlayerScript>();
        whatIsGround = ((1 << 6) | (1 << 3));
    }

    private void Update() {
        if (playerScript.isDead) {
            return;
        }
        
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (Input.GetKey(KeyCode.A)) {
            runningDirection = -1;
        } else if (Input.GetKey(KeyCode.D)) {
            runningDirection = 1;
        } else {
            runningDirection = 0;
        }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded) {
            rb.velocity = Vector2.up * jump;
        }

        if (runningDirection != 0) {
            playerScript.Direction = runningDirection;
        }    
        playerScript.isRunning = runningDirection != 0;
        playerScript.isJumping = !isGrounded;
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(speed * runningDirection, rb.velocity.y);
    }
  
}
