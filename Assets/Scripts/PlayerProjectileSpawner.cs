using UnityEngine;
using UnityEngine.Pool;

public class PlayerProjectileSpawner : MonoBehaviour
{
    [SerializeField] Projectile prefab;
    public ObjectPool<Projectile> _pool {  get; private set; }
    private void Awake()
    {
        _pool = new ObjectPool<Projectile>(CreatePooledProjectile, OnTakeFromPool, OnReturnToPool, OnDestroyObject, true);
    }
    private Projectile CreatePooledProjectile()
    {
        Projectile instance =  Instantiate(prefab, Vector3.zero, Quaternion.identity);
        instance.gameObject.SetActive(false);
        instance._projectileSpawner = this;
        return instance;
    }
    private void OnTakeFromPool(Projectile instance)
    {
        instance.gameObject.SetActive(true);
    }
    private void OnReturnToPool(Projectile instance)
    {
        instance.gameObject.SetActive(false);
    }
    private void OnDestroyObject(Projectile Instance)
    {
        Destroy(Instance);
    }
}
