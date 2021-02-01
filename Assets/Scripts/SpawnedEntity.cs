using System;
using UnityEngine;

public class SpawnedEntity : MonoBehaviour
{
	public Action<SpawnedEntity> DestroyCallback;

	private void OnDestroy()
	{
		if (DestroyCallback == null)
		{
			Debug.LogWarning("[SpawnedEntity] Destroyed with null DestroyCallback");
			return;
		}

		DestroyCallback?.Invoke(this);
	}
}
