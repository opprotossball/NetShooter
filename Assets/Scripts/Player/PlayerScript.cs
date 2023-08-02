using UnityEngine;
using TMPro;


public enum Weapon
{
    RIFLE,
    GRENADE
}

public class PlayerScript : MonoBehaviour, IDamagable
{
    public string nick;
    public bool isRunning, isCrouchnig, isJumping = false;
    public AWeapon selectedWeapon;
    [SerializeField] public Rifle rifle;
    [SerializeField] public ThrowGrenade throwGrenade;
    [SerializeField] private float maxHp;
    [SerializeField] private Transform nickParent;
    [SerializeField] private Transform healthBarParent;
    [SerializeField] private TMP_Text nickDisplay;
    [SerializeField] private Transform bullets;
    [SerializeField] private GameObject trajectoryPoint;
    [SerializeField] private int trajectoryPointsCount;
    [SerializeField] private float trajectoryPointsDistance;
    public GameObject[] trajectoryPoints;
    private int _direction;
    private float _health;
    public bool isDead;
    public int Direction { 
        get => _direction;
        set {
            if ((_direction ^ value) < 0) {
                transform.Rotate(0f, 180f, 0f);
                nickParent.Rotate(0f, 180f, 0f);
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
                Die();            
            }
            _health = newVal;
        } 
    }

    private void Die()
    {
        animator.SetTrigger("Death");
        isDead = true;
        GameManager.instance.RegisterDeath(this);
    }

    public void SetNickDisplay() {
        nickDisplay.text = nick;
    }

    private Animator animator;

    void Start() {
        GameManager.instance.RegisterPlayer(gameObject);
        animator = GetComponent<Animator>();
        playerStateManager = GetComponent<PlayerStateManager>();
        selectedWeapon = rifle;
        Health = maxHp;
        trajectoryPoints = new GameObject[trajectoryPointsCount];
        for (int i = 0; i < trajectoryPointsCount; i++)
        {
            trajectoryPoints[i] = Instantiate(trajectoryPoint, throwGrenade.firePoint.position, Quaternion.identity);
        }
        SetTrajectoryVisibility(false);
    }

    void Update() {
        if (!isDead)
        {
            animator.SetBool("Crouching", isCrouchnig);
            animator.SetBool("Jumping", isJumping);
            animator.SetBool("Running", isRunning);
        }
        nickParent.position = transform.position;
        healthBarParent.position = transform.position;
        bullets.position = transform.position;
        ShowTrajectoryPoints();
        selectedWeapon.Reload();
        MainSceneUI.instance.activeWeapon = selectedWeapon is ThrowGrenade ? Weapon.GRENADE : Weapon.RIFLE;
        MainSceneUI.instance.cooldown = selectedWeapon.TimeToReload();
        MainSceneUI.instance.ShowCooldowns();
    }

    void IDamagable.Damage(float damage) {
        if (!playerStateManager.IsOwner) {
            return;
        }
        Health -= damage;
    }

    private void ShowTrajectoryPoints()
    {
        if (selectedWeapon is not ThrowGrenade) {
            return;
        }
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (target - selectedWeapon.firePoint.transform.position).normalized;
        SetTrajectoryVisibility(selectedWeapon.CanShoot(dir));
        for (int i = 0; i < trajectoryPointsCount; i++)
        {
            trajectoryPoints[i].transform.position = TrajectoryPointPosition(i * trajectoryPointsDistance, dir);
        }
    }

    private Vector2 TrajectoryPointPosition(float t, Vector2 dir)
    {
        return (Vector2)selectedWeapon.firePoint.position + dir * throwGrenade.force * t + 0.5f * Physics2D.gravity * (t * t);
    }

    public void SetTrajectoryVisibility(bool active)
    {
        foreach (GameObject point in trajectoryPoints)
        {
            point.SetActive(active);
        }
    }

    public bool IsLocal()
    {
        return playerStateManager.IsOwner;
    }

}
