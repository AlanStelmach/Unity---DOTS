using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class SetupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject personPrefab;
    [SerializeField] private int gridSize;
    [SerializeField] private int spread;
    private BlobAssetStore blob;
    [SerializeField] private Vector2 speedRange = new Vector2(4, 7);
    [SerializeField] private Vector2 lifetimeRange = new Vector2(10, 60);

    private void Start()
    {
        blob = new BlobAssetStore();
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob);
        var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(personPrefab, settings);
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        for(int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                var instance = entityManager.Instantiate(entity);
                float3 position = new float3(x * spread, 0, z * spread);
                entityManager.SetComponentData(instance, new Translation {Value = position});
                entityManager.SetComponentData(instance, new Destination {Value = position}); // Comment this line to make objects travel to the center of the scene
                float speed = UnityEngine.Random.Range(speedRange.x, speedRange.y);
                entityManager.SetComponentData(instance, new MovementSpeed {Speed = speed});
                float lifetime = UnityEngine.Random.Range(lifetimeRange.x, lifetimeRange.y);
                entityManager.SetComponentData(instance, new Lifetime {Value = lifetime});
            }
        }
    }

    private void OnDestroy()
    {
        blob.Dispose();
    }
}
