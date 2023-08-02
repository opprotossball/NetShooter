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
    private float reloadingTime = 0;

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

    public void OwnerFire(Vector3 dir)
    {
        if (!CanShoot(dir)) { return; }
        Fire(dir);
        RequestFireServerRpc(dir);
    }

    public void Fire(Vector3 dir)
    {
        ManageAmmo();
        Shoot(dir);
    }

    public bool CanShoot(Vector3 dir)
    {
        return (loadedAmmo > 0 && lastFired + cooldownTime <= Time.time && Vector2.Angle(firePoint.right, dir) <= maxAngle);
    }

    private void Start()
    {
        lastFired = Time.time;
        loadedAmmo = magSize;
    }

    public bool OutOfAmmo()
    {
        return (!infinityAmmo && ammo < 1);
    }

    public void Reload()
    {   
        if (OutOfAmmo()) { return; }
        if (loadedAmmo < 1) { reloadingTime += Time.deltaTime; }
        if (loadedAmmo < 1 && reloadingTime > magReloadTime)
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
            reloadingTime = 0;
        }
        
    }

    public float TimeToReload()
    {
        if (loadedAmmo < 1 && OutOfAmmo()) { return 1; }
        return reloadingTime / magReloadTime;
    }

    public int GetLoadedAmmo()
    {
        return loadedAmmo;
    }

}
