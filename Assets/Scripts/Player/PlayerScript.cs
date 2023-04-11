using UnityEngine;
using TMPro;


public class PlayerScript : MonoBehaviour, IDamagable
{
    public string nick;
    public bool isRunning, isCrouchnig, isJumping;
    public AWeapon selectedWeapon;
    [SerializeField] public Rifle rifle;
    [SerializeField] public ThrowGrenade throwGrenade;
    [SerializeField] private float maxHp;
    [SerializeField] private Transform nickParent;
    [SerializeField] private Transform healthBarParent;
    [SerializeField] private TMP_Text nickDisplay;
    [SerializeField] private Transform bullets;
    
    private int _direction;
    private float _health;
    public bool isDead;
    public int Direction { 
        get => _direction;
        set {
            if ((_direction ^ value) < 0) {
                transform.Rotate(0f, 180f, 0f);
            }
            _direction = value;
        }    
    }
    [SerializeField] private Transform healthBar;
    private PlayerStateManager playerStateManager;

    public float Health {
        get => _health; 
        set {
            float newVal = Mathf.Max( Mathf.Min(value, maxHp), 0 );
            healthBar.localScale = new Vector3(newVal / maxHp, 1f);
            if (newVal == 0 && _health > 0) {
                animator.SetTrigger("Death");
                isDead = true;
            }
            _health = newVal;
        } 
    }

    public void SetNickDisplay() {
        nickDisplay.text = nick;
    }

    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        playerStateManager = GetComponent<PlayerStateManager>();
        selectedWeapon = rifle;
        Health = maxHp;
    }

    void Update() {
        if (isDead && !animator.GetCurrentAnimatorStateInfo(0).IsName("DeathBlue"))
        animator.SetBool("Running", isRunning);
        animator.SetBool("Crouching", isCrouchnig);
        animator.SetBool("Jumping", isJumping);
        nickParent.position = transform.position;
        healthBarParent.position = transform.position;
        bullets.position = transform.position;
    }

    void IDamagable.Damage(float damage) {
        if (!playerStateManager.IsOwner) {
            return;
        }
        Health -= damage;
    }

}
