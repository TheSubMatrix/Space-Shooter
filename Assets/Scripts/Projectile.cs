using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TrailRenderer))]
public class Projectile : MonoBehaviour
{
    public PlayerProjectileSpawner _projectileSpawner;
    [SerializeField] float _speed = 20f;
    [SerializeField] uint _damage = 10;
    [SerializeField] float lifetime = 5;
    public IEnumerator DestroyRoutine { get; private set; }
    private void Update()
    {
            transform.position += transform.forward * (_speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.HasComponent<IDamagable>())
        {
            collider.gameObject.GetComponent<IDamagable>().Damage(_damage);
        }
        _projectileSpawner._pool.Release(this);
    }
    public void Shoot(Vector3 spawnPosition, Vector3 Direction)
    {
        transform.position = spawnPosition;
        transform.forward = Direction;
        GetComponent<TrailRenderer>().Clear();
        GetComponent<TrailRenderer>().emitting = true;
        DestroyRoutine = DestroyAfterLifetime();
        StartCoroutine(DestroyRoutine);
    }
    IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        _projectileSpawner._pool.Release(this);
        DestroyRoutine = null;
    }
    private void OnDisable()
    {
        GetComponent<TrailRenderer>().emitting = false;
        if (DestroyRoutine != null)
        {
            StopCoroutine(DestroyRoutine);
        }
        DestroyRoutine = null;
    }
}
