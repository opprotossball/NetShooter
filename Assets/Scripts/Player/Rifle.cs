using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class Rifle : AWeapon { 
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float damage;
    [SerializeField] private GameObject impactEffect;

    private readonly float RAY_RANGE = 200f;


    private void Start() {
        lastFired = Time.time;
        loadedAmmo = magSize;
    }

    //protected override void Update() {
    //    base.Update();
    //}

    public override void Shoot(Vector3 dir)
    {
        StartCoroutine(IEShoot(dir));
    }

    IEnumerator IEShoot(Vector3 dir) {
        
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, dir, RAY_RANGE, ~(1 << 6));
        lineRenderer.SetPosition(0, firePoint.position);
        if (hit) {
            IDamagable target = hit.transform.GetComponent<IDamagable>();
            if (target != null) {
                target.Damage(damage);
            }
            lineRenderer.SetPosition(1, hit.point);
            Instantiate(impactEffect, hit.point, Quaternion.identity);
        } else {
            lineRenderer.SetPosition(1, firePoint.position + dir * RAY_RANGE);
        }
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.02f);
        lineRenderer.enabled = false;
    }


}
