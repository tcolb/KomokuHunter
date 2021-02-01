using UnityEngine;
using TMPro;


public class PatronSpawner : EntitySpawner
{
	[SerializeField] SpawnedEntity PatronEntity;

	[SerializeField] string DefaultHintText;
	[SerializeField] PointOfInterest[] POIS;
	[SerializeField] PointOfInterest EndPOI;
	[SerializeField] float HintWaitSeconds;

	public override void ForceSpawn()
	{
		var entity = Instantiate(PatronEntity, GetNextSpawnLocation(), Quaternion.Euler(ForcedRotation));
		entity.transform.position = transform.position;
		entity.enabled = true;

		var spawnedEntity = entity.GetComponent<SpawnedEntity>();
		spawnedEntity.DestroyCallback += OnSpawnedDestroyed;

		var patrol = spawnedEntity.gameObject.AddComponent<Patrol>();
		patrol.poi = POIS;
		patrol.endPoint = EndPOI;
		patrol.hintText = spawnedEntity.GetComponentInChildren<TextMeshPro>();
		patrol.mongus = spawnedEntity.GetComponentInChildren<MeshCollider>().transform;
		patrol.defaultHintMessage = DefaultHintText;
		patrol.HintWaitSeconds = HintWaitSeconds;

		SpawnedEntities.Add(entity);
	}
}
