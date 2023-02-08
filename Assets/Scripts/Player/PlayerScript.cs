using UnityEngine;
using TMPro;

public class PlayerScript : MonoBehaviour, IDamagable
{
    public string nick;
    public bool isRunning, isCrouchnig, isJumping;
    [SerializeField] private float maxHp;
    [SerializeField] private Transform nickParent;
    [SerializeField] private Transform healthBarParent;
    [SerializeField] private TMP_Text nickDisplay;
    private readonly Vector3 nickOffset = new Vector3(0, 2, 0);
    
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
            healthBar.localScale = new Vector3(newVal/maxHp, 1f);
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
        Health = maxHp;
    }

    void Update() {
        animator.SetBool("Running", isRunning);
        animator.SetBool("Crouching", isCrouchnig);
        animator.SetBool("Jumping", isJumping);
        nickParent.position = transform.position;
        healthBarParent.position = transform.position;
    }

    void IDamagable.Damage(float damage) {
        if (!playerStateManager.IsOwner) {
            return;
        }
        Health -= damage;
    }

}
