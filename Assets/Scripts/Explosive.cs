using UnityEngine;

public abstract class Explosive : MonoBehaviour
{
    protected readonly int nRays = 1000;
    [SerializeField] protected float damage;
    [SerializeField] protected Transform explosionPoint;
    [SerializeField] protected float range;
    [SerializeField] protected float destroyDelay;
    [SerializeField] protected GameObject explosionEffect;
    protected void Explode()
    {
        float step = 360 / (float)nRays;
        float angle = 0;
        float damagePerRay = damage / (float)nRays;
        while (angle < 360)
        {
            Vector3 dir = transform.TransformDirection(Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.one);
            RaycastHit2D hit = Physics2D.Raycast(explosionPoint.position, dir, range, ~(1 << 6));
            if (hit)
            {
                IDamagable target = hit.transform.GetComponent<IDamagable>();
                if (target != null)
                {
                    target.Damage(damagePerRay);
                }
            }
            angle += step;
        }
        Instantiate(explosionEffect, explosionPoint.position, Quaternion.identity);
        Destroy(gameObject, destroyDelay);
    }
}
