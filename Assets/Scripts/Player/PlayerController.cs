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
    private PlayerScript playerScript;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        playerScript = GetComponent<PlayerScript>();
        whatIsGround = ((1 << 6) | (1 << 3));
    }

    private void Update() {
        if (playerScript.isDead || GameManager.instance.GetGameState() != GameState.ACTIVE) {
            return;
        }
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        ChangeWeapon();
        Fire();
        Move();
    }

    private void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            playerScript.selectedWeapon = playerScript.rifle;
            playerScript.SetTrajectoryVisibility(false);
            MainSceneUI.instance.SetActiveWeapon(Weapon.RIFLE);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            playerScript.selectedWeapon = playerScript.throwGrenade;
            MainSceneUI.instance.SetActiveWeapon(Weapon.GRENADE);
        }
    }

    private void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = (target - playerScript.selectedWeapon.firePoint.transform.position).normalized;
            dir.z = 0f;
            playerScript.selectedWeapon.OwnerFire(dir);
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.A))
        {
            runningDirection = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            runningDirection = 1;
        }
        else
        {
            runningDirection = 0;
        }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = Vector2.up * jump;
        }

        if (runningDirection != 0)
        {
            playerScript.Direction = runningDirection;
        }
        playerScript.isRunning = runningDirection != 0;
        playerScript.isJumping = !isGrounded;
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(speed * runningDirection, rb.velocity.y);
    }
  
}
