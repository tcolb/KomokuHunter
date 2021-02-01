using System.Collections.Generic;
using UnityEngine;

public abstract class EntitySpawner : MonoBehaviour
{
	[SerializeField] protected int EntityLimit;
    [SerializeField] protected float SpawnIntervalInSeconds;
	[SerializeField] protected Vector3 SpawnAreaDimensions;
	[SerializeField] protected Vector3 ForcedRotation;
	[SerializeField] protected bool SpawnAtStart;
	[SerializeField] protected bool RandomDistribution;
	[SerializeField] protected bool OffsetX;
	[SerializeField] protected bool OffsetY;
	[SerializeField] protected bool OffsetZ;

    protected List<SpawnedEntity> SpawnedEntities;
    protected float TimeSinceLastSpawn = 0;

	private void Start()
	{
		SpawnedEntities = new List<SpawnedEntity>();

		LastPos = new Vector3
		(
			OffsetX ? transform.position.x - SpawnAreaDimensions.x * 0.5f : transform.position.x,
			OffsetY ? transform.position.y - SpawnAreaDimensions.y * 0.5f : transform.position.y,
			OffsetZ ? transform.position.z - SpawnAreaDimensions.z * 0.5f : transform.position.z
		);
	}

	bool AlreadySpawned = false;

	private void Update()
	{
		if (SpawnAtStart && !AlreadySpawned)
		{
			ForceSpawn();
			AlreadySpawned = true;
		}

		TimeSinceLastSpawn += Time.deltaTime;

		if (TimeSinceLastSpawn >= SpawnIntervalInSeconds)
		{
			if (SpawnedEntities.Count < EntityLimit || EntityLimit == 0)
			{
				ForceSpawn();
			}

			TimeSinceLastSpawn = 0;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, SpawnAreaDimensions);
		Gizmos.DrawSphere(LastPos, 0.2f);
	}

	protected void OnSpawnedDestroyed(SpawnedEntity entity)
	{
		SpawnedEntities.Remove(entity);
	}

	public abstract void ForceSpawn();

	public SpawnedEntity GetRandomSpawnedEntity()
	{
		if (SpawnedEntities.Count == 0)
		{
			return null;
		}

		return SpawnedEntities[Main.Instance.Rand.Next(SpawnedEntities.Count)];
	}

	Vector3 LastPos;
	protected Vector3 GetNextSpawnLocation()
	{
		if (RandomDistribution)
		{
			return GetRandomPointInSpawnArea();
		}
		else
		{
			return LastPos += new Vector3
			(
				OffsetX ? SpawnAreaDimensions.x / EntityLimit : 0,
				OffsetY ? SpawnAreaDimensions.y / EntityLimit : 0,
				OffsetZ ? SpawnAreaDimensions.z / EntityLimit : 0
			);
		}
	}

	Vector3 GetRandomPointInSpawnArea()
	{
		return new Vector3
		(
			Random.Range(transform.position.x - SpawnAreaDimensions.x * 0.5f, transform.position.x + SpawnAreaDimensions.x * 0.5f),
			Random.Range(transform.position.y - SpawnAreaDimensions.y * 0.5f, transform.position.y + SpawnAreaDimensions.y * 0.5f),
			Random.Range(transform.position.z - SpawnAreaDimensions.z * 0.5f, transform.position.z + SpawnAreaDimensions.z * 0.5f)
		);
	}

}
