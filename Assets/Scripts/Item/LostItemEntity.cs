using UnityEngine;

public class LostItemEntity : SpawnedEntity
{
	public NeedsAttributes Attributes;

	public void RandomInit()
	{
		Attributes = new NeedsAttributes();
		Attributes.Randomize();

		ApplyAttributesToMaterial();
	}

	public void ApplyAttributesToMaterial()
	{
		var material = GetComponent<MeshRenderer>().material;
		Attributes.ApplyToMaterial(material);
	}
}