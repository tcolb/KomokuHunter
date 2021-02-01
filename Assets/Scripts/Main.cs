using UnityEngine;
using TMPro;
using Random = System.Random;


public class Main : MonoBehaviour
{
    public static Main Instance;
	public Random Rand;

    Texture2D[] _LostItemTextures;
    public Texture2D[] LostItemTextures => _LostItemTextures;

    AttributeColorWrapper[] _LostItemColors;
    public AttributeColorWrapper[] LostItemColors => _LostItemColors;

    SpawnedEntity[] _LostItemMeshes;
    public SpawnedEntity[] LostItemMeshes => _LostItemMeshes;

    // For getting random spawned entities
    EntitySpawner[] ItemSpawners;

    void Start()
    {
        // lazy
        Instance = this;

        // Initialize game wide rng 
        Rand = new Random();

       

        // Load Resources
        _LostItemTextures = LoadAllResourcesAtPathOfType<Texture2D>("ItemTextures");
        _LostItemColors = LoadAllResourcesAtPathOfType<AttributeColorWrapper>("ItemColors");
        _LostItemMeshes = LoadAllResourcesAtPathOfType<SpawnedEntity>("ItemMeshes");

        // Initialize asset enum maps
        // This must be done after resources are loaded
        NeedsAttributes.SetEnumAttributeMaps();
    }

	T[] LoadAllResourcesAtPathOfType<T>(string path) where T : Object
    {
        var results = Resources.LoadAll<T>(path);
        Debug.Log($"[MAIN] Loaded {results.Length} resources of type {typeof(T)}");
        return results;
	}

    public SpawnedEntity GetRandomSpawnedEntity()
    {
        // TODO: move this into main and sync
        ItemSpawners = FindObjectsOfType<EntitySpawner>();

        if (ItemSpawners.Length == 0)
        {
            return null;
		}

        var spawner = ItemSpawners[Rand.Next(ItemSpawners.Length)];
        return spawner.GetRandomSpawnedEntity();
	}

}