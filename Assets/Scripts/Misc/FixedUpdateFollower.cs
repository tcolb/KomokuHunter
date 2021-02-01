using UnityEngine;

public class FixedUpdateFollower : MonoBehaviour
{
	[SerializeField] Transform Target;

	[Range(0, 1)]
	[SerializeField] float LerpFactor;

	private void FixedUpdate()
	{
		transform.position = Vector3.Lerp(transform.position, Target.transform.position, LerpFactor);
		transform.rotation = Quaternion.Slerp(transform.rotation, Target.transform.rotation, LerpFactor);
	}
}
