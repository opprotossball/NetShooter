using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class Weapon : NetworkBehaviour { 
    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float damage;
    [SerializeField] private float reloadTime;
    [SerializeField] private GameObject impactEffect;
    private PlayerScript playerScript;
    private readonly float RAY_RANGE = 200f;

    private float lastFired = float.MinValue;

    private void Start() {
        playerScript = GetComponent<PlayerScript>();
    }

    private void Update() {
        if (!IsOwner || playerScript.isDead) {
            return;
        }
        // if (Input.GetKeyDown(KeyCode.Space) && lastFired + reloadTime < Time.time) {
        //     lastFired = Time.time;
        //     RequestFireServerRpc(firePoint.right);
        //     StartCoroutine(Shoot(firePoint.right));
        // }
        if (Input.GetMouseButtonDown(0)) {
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = (target - firePoint.transform.position).normalized;
            Debug.Log(dir.ToString());
            RequestFireServerRpc(dir);
            StartCoroutine(Shoot(dir));
        }
    }

    [ServerRpc]
    private void RequestFireServerRpc (Vector3 dir) {
        FireClientRpc(dir);
    }

    [ClientRpc]
    private void FireClientRpc(Vector3 dir) {
        if (!IsOwner) {
            StartCoroutine(Shoot(dir));
        }
    }

    IEnumerator Shoot(Vector3 dir) {
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
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 100);
        }

        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.02f);
        lineRenderer.enabled = false;
    }

}
