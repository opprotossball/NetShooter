using UnityEngine;

public class Barrel : Explosive, IDamagable
{
    [SerializeField] private float maxHp;
    [SerializeField] private Transform healthBar;
    private float _health;

    private void Start()
    {
        _health = maxHp;
    }

    public void Damage(float damage)
    {
        Health -= damage;
    }

    public float Health
    {
        get => _health; 
        set {
            float newVal = Mathf.Max( Mathf.Min(value, maxHp), 0 );
            healthBar.localScale = new Vector3(newVal/maxHp, 1f);
            if (newVal == 0 && _health > 0) {
                _health = 0;
                Explode();
            }
            _health = newVal;
        } 
    }

}
