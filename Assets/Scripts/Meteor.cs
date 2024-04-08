
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Health), typeof(Collider), typeof(Rigidbody))]
public class Meteor : MonoBehaviour
{
    public MeteorManager _manager;
    const float moveSpeed = 10;
    const uint damageToDealOnCollision = 20;
    public Health Health { get; private set; }
    public GameManager GameManager { get; private set; }
    Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Awake()
    {
        Health = GetComponent<Health>();
        GameManager = FindObjectOfType<GameManager>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {

        _rigidbody.velocity = Vector3.forward * -1 * moveSpeed;
        
        if(!GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(Camera.main), gameObject.GetComponent<Collider>().bounds))
        {
            _manager.Pool.Release(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.HasComponent<IDamagable>()) 
        {
            collision.gameObject.GetComponent<IDamagable>().Damage(damageToDealOnCollision);
        }
    }
    public void OnDeath()
    {
        _manager.Pool.Release(gameObject);
        if(GameManager != null)
        {
            GameManager.Score++;
        }
    }
    public void Reset()
    {
        Health.Reset();
    }
    public void Setup(Vector3 position, Vector3 rotation)
    {
        transform.position = position;
        _rigidbody.AddRelativeTorque(rotation, ForceMode.VelocityChange);
    }

}
