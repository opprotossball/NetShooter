using UnityEngine;

public class ThrowGrenade : AWeapon
{
    [SerializeField] private GameObject grenade;
    public float force;

    public override void Shoot(Vector3 dir)
    {
        GameObject newGrenade = Instantiate(grenade, firePoint.position, Quaternion.identity);
        newGrenade.GetComponent<Rigidbody2D>().AddForce(dir * force, ForceMode2D.Impulse);
    }

 }
