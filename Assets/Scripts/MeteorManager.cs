using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MeteorManager : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabList = new List<GameObject>();
    public const float MeteorSpawnZPosition = 200;
    const float meteorSpawnBorderPercentage = 0.1f;
    Ship _ship;
    public ObjectPool<GameObject> Pool { get; private set; }
    private void Start()
    {
        Ship foundShip = FindObjectOfType<Ship>();
        if (foundShip != null)
        {
            _ship = foundShip;
        }
        Pool = new ObjectPool<GameObject>(CreatePooledMeteor, OnTakeFromPool, OnReturnToPool, OnDestroyObject, true);
        StartCoroutine(SpawnMeteorAsync());
    }
    private GameObject CreatePooledMeteor()
    {
        int prefabIndex = Random.Range(0, prefabList.Count);
        GameObject instance = Instantiate(prefabList[prefabIndex], prefabList[prefabIndex].transform.position, prefabList[prefabIndex].transform.rotation);
        instance.gameObject.SetActive(false);
        instance.GetComponent<Meteor>()._manager = this;
        return instance;
    }
    private void OnTakeFromPool(GameObject instance)
    {
        instance.gameObject.SetActive(true);
    }
    private void OnReturnToPool(GameObject instance)
    {
        instance.SetActive(false);
        instance.GetComponent<Meteor>().Reset();
    }
    private void OnDestroyObject(GameObject Instance)
    {
        Destroy(Instance);
    }
    IEnumerator SpawnMeteorAsync()
    {
        float timeBetweenSpawns = Random.Range(2, 7);
        SpawnMeteor();
        yield return new WaitForSeconds(timeBetweenSpawns);
        StartCoroutine(SpawnMeteorAsync());
    }
    GameObject SpawnMeteor()
    {
        GameObject newMeteor = Pool.Get();
        newMeteor.GetComponent<Meteor>().Setup(DetermineEnemySpawnPosition(), DetermineMeteorRotation());
        return newMeteor;
    }
    Vector3 DetermineEnemySpawnPosition()
    {
        Camera cam = Camera.main;
        float topSpawnMax = cam.ScreenToWorldPoint(new Vector3(0.5f, (0 + (1 * meteorSpawnBorderPercentage))*Screen.height, cam.nearClipPlane + Vector3.Distance(cam.transform.position, _ship.StartingPosition))).y;
        float bottomSpawnMin = cam.ScreenToWorldPoint(new Vector3(0.5f, (1 - (1 * meteorSpawnBorderPercentage)) * Screen.height, cam.nearClipPlane + Vector3.Distance(cam.transform.position, _ship.StartingPosition))).y;
        float leftSpawnMin = cam.ScreenToWorldPoint(new Vector3((0 + (1 * meteorSpawnBorderPercentage)) * Screen.width, 0.5f, cam.nearClipPlane + Vector3.Distance(cam.transform.position, _ship.StartingPosition))).x;
        float RightSpawnMax = cam.ScreenToWorldPoint(new Vector3((1 - (1 * meteorSpawnBorderPercentage))*Screen.width, 0.5f, cam.nearClipPlane + Vector3.Distance(cam.transform.position, _ship.StartingPosition))).x;
        Vector3 determinedPosition = new Vector3
            (
            Random.Range(leftSpawnMin, RightSpawnMax),
            Random.Range(bottomSpawnMin, topSpawnMax),
            MeteorSpawnZPosition
            );
        Debug.Log(determinedPosition);
        return determinedPosition;
    }
    Vector3 DetermineMeteorRotation()
    {
        const int meteorRotationScalar = 500;
        return Random.rotation.eulerAngles / meteorRotationScalar;
    }
}
