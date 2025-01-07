using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    [SerializeField] private Collider _platformCollider;
    [SerializeField] private int _poolDefaultCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;
    [SerializeField] private float _spawnRate = 1.0f;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_cube),
            actionOnGet: (cube) => SetParameters(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => DestroyCube(cube),
            collectionCheck: true,
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolMaxSize);
    }
    
    private void Start()
    {
        StartCoroutine(TwoSecondTimer());
    }

    private void SpawnCube()
    {
        _pool.Get();
    }

    private void SetParameters(Cube cube)
    {
        cube.gameObject.SetActive(true);
        cube.transform.position = GetRandomPosition();
        cube.ResetVelocity();
        cube.Touched += ReturnCubeToPool;
    }
    
    private void ReturnCubeToPool(Cube cube)
    {
        cube.Touched -= ReturnCubeToPool;
        _pool.Release(cube);
    }

    private void DestroyCube(Cube cube)
    {
        Destroy(cube.gameObject);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 position;
        Vector3 size = _platformCollider.bounds.size;
        float decreaser = 0.8f;
        float spawnHeight = 12f;
        float platformWidthApothem = size.x / 2;
        float platformLengthApothem = size.z / 2;

        position = _platformCollider.transform.position;
        position.x = Random.Range(-platformWidthApothem, platformWidthApothem) * decreaser;
        position.z = Random.Range(-platformLengthApothem, platformLengthApothem) * decreaser;
        position.y += spawnHeight;

        return position;
    }

    private IEnumerator TwoSecondTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnRate);
            SpawnCube();
        }
    }
}
