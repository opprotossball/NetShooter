using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public abstract class AWeapon : NetworkBehaviour
{
    [SerializeField] public Transform firePoint;
    [SerializeField] protected float cooldownTime;
    [SerializeField] protected float maxAngle;
    [SerializeField] protected int magSize;
    [SerializeField] protected float magReloadTime;
    [SerializeField] protected int ammo;
    [SerializeField] protected bool infinityAmmo;
    protected int loadedAmmo;
    protected float startedReloading;
    protected float lastFired;

    public abstract void Shoot(Vector3 dir);
    private void ManageAmmo()
    {
        loadedAmmo--;
        if (loadedAmmo < 1)
        {
            startedReloading = Time.time;
        }
        lastFired = Time.time;
    }


    [ServerRpc]
    public void RequestFireServerRpc(Vector3 dir)
    {
        if (!CanShoot(dir))
        {
            return;
        }
        FireClientRpc(dir);
    }

    [ClientRpc]
    public void FireClientRpc(Vector3 dir)
    {
        if (!IsOwner)
        {
            Fire(dir);
        }
    }

    public void Fire(Vector3 dir)
    {
        if (!CanShoot(dir))
        {
            return;
        }
        ManageAmmo();
        Shoot(dir);
    }

    public bool CanShoot(Vector3 dir)
    {
        return (loadedAmmo > 0 && lastFired + cooldownTime <= Time.time && Vector2.Angle(firePoint.right, dir) <= maxAngle);
    }

    protected virtual void Update()
    {
        if (loadedAmmo < 1 && Time.time > startedReloading + magReloadTime)
        {
            if (infinityAmmo)
            {
                loadedAmmo = magSize;
            }
            else
            {
                loadedAmmo = Mathf.Min(magSize, ammo);
                ammo -= loadedAmmo;
            }
        }
    }

    public int GetLoadedAmmo()
    {
        return loadedAmmo;
    }

}
