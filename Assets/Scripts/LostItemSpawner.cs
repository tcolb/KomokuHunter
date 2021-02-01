using System.Collections.Generic;
using UnityEngine;

public partial class LostItemSpawner : EntitySpawner
{
	[SerializeField] NeedsAttributes.AttributeMesh[] AllowedMeshes;

	public override void ForceSpawn()
	{
		var attributes = new NeedsAttributes();
		attributes.Randomize();

		attributes.Model = AllowedMeshes[Main.Instance.Rand.Next(AllowedMeshes.Length)];
		var model = attributes.GetModel();

		var entity = Instantiate(model, GetNextSpawnLocation(), Quaternion.Euler(ForcedRotation));

		var lostItem = entity.GetComponent<LostItemEntity>();
		lostItem.Attributes = attributes;
		lostItem.ApplyAttributesToMaterial();

		lostItem.DestroyCallback += OnSpawnedDestroyed;
		lostItem.tag = "Item";

		SpawnedEntities.Add(entity);
	}
}
