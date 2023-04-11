using UnityEngine;

public class ThrowGrenade : AWeapon
{
    [SerializeField] private GameObject grenade;
    [SerializeField] private float force;
    public override void Shoot(Vector3 dir)
    {
        GameObject newGrenade = Instantiate(grenade, firePoint.position, Quaternion.identity);
        newGrenade.GetComponent<Rigidbody2D>().AddForce(dir * force, ForceMode2D.Impulse);
    }

    private void Start()
    {
        lastFired = Time.time;
        loadedAmmo = magSize;
    }

}
