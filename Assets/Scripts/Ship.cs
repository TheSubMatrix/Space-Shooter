using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerProjectileSpawner), typeof(Rigidbody), typeof(Health))]
public class Ship : MonoBehaviour
{
    const float _moveSpeed = 2f;
    const float _invulnerabilityFlashDelay = 0.05f;
    [SerializeField] Transform _firePoint1;
    [SerializeField] Transform _firePoint2;

    Rigidbody _rb;
    Vector3 _pointToMoveTowards;
    Vector3 _desiredVelocity = Vector3.zero;
    Vector3 _startingPosition;
    Renderer[] _gameObjectRenderers;
    bool _shouldBeFlashing = false;

    public Vector3 StartingPosition
    {
        get
        {
            return _startingPosition;
        }
        private set 
        {
            if(_startingPosition == default)
            {
                _startingPosition = value;
            }
        }
    }
    public Transform FirePoint2
    {
        get
        {
            return _firePoint2;
        }
    }
    public Transform FirePoint1
    {
        get
        {
            return _firePoint1;
        }
    }

    PlayerProjectileSpawner _projectileSpawner;

    private void Start()
    {
        StartingPosition = transform.position;
        _projectileSpawner = GetComponent<PlayerProjectileSpawner>();
        _rb = GetComponent<Rigidbody>();
        _gameObjectRenderers = GetComponentsInChildren<Renderer>();
    }
    private void Update()
    {

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + Vector3.Distance(Camera.main.transform.position, StartingPosition));
        _pointToMoveTowards = new Vector3(Camera.main.ScreenToWorldPoint(mousePos).x, Camera.main.ScreenToWorldPoint(mousePos).y, StartingPosition.z);
        _desiredVelocity = (_pointToMoveTowards - transform.position) * _moveSpeed;
        _rb.velocity = (_desiredVelocity);
        _rb.MoveRotation(Quaternion.Euler(_desiredVelocity.y / 2, -_desiredVelocity.x / 2, _desiredVelocity.x * -1));

        if (Input.GetMouseButtonDown(0))
        {
            Projectile newProjectile = _projectileSpawner._pool.Get();
            newProjectile.Shoot(FirePoint1.position, transform.forward);
            newProjectile = _projectileSpawner._pool.Get();
            newProjectile.Shoot(FirePoint2.position, transform.forward);
        }
    }
    public void OnDeath()
    {
        Destroy(gameObject);
    }
    public void OnBecameInvulnerable()
    {
        StartCoroutine(FlashDuringInvulnerability(_invulnerabilityFlashDelay));
    }
    public void OnBecameVulnerable()
    {
        _shouldBeFlashing = false;
    }
    IEnumerator FlashDuringInvulnerability(float timeToWaitAfterFlash)
    {
        _shouldBeFlashing = true;
        bool isInvisible = false;
        while (_shouldBeFlashing) 
        {
            yield return Flash(!isInvisible, timeToWaitAfterFlash);
            isInvisible = !isInvisible;
        }
        foreach (Renderer renderer in _gameObjectRenderers)
        {
            renderer.enabled = true;
        }

    }
    IEnumerator Flash(bool desiredState,float timeToWaitAfterFlash)
    {
        foreach(Renderer renderer in _gameObjectRenderers) 
        {
            renderer.enabled = desiredState;
        }
        yield return new WaitForSeconds(timeToWaitAfterFlash);
    }
}
