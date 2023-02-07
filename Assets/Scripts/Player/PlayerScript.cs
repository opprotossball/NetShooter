using UnityEngine;

public class PlayerScript : MonoBehaviour, IDamagable
{
    public bool isRunning, isCrouchnig, isJumping;
    [SerializeField] private float maxHp;
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
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        Health = maxHp;
    }

    void Update() {
        animator.SetBool("Running", isRunning);
        animator.SetBool("Crouching", isCrouchnig);
        animator.SetBool("Jumping", isJumping);
    }

    void IDamagable.Damage(float damage) {
        Health -= damage;
        Debug.Log("I have " + Health + " hp now");
    }

}
