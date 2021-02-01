using UnityEngine;


public class ItemHinting : MonoBehaviour
{
    public float RotationSpeed = 50f;
    LostItemEntity LostItem;

    public void Init(LostItemEntity lostItem)
    {
        var entity = Instantiate(lostItem, transform.position, transform.rotation);
        entity.tag = "Untagged";
        entity.transform.SetParent(transform);

        LostItem = entity;

        // this'll probably throw an error
        var collider = LostItem.GetComponent<Collider>();
		collider.enabled = false;

        var rigidBody = LostItem.GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;
	}

    void Update()
    {
        var rot = Mathf.Sin(Time.time) * RotationSpeed * Time.deltaTime;
        LostItem?.transform.Rotate(new Vector3(rot, rot, rot));
	}

	public void DisableHinting()
	{
        if (LostItem?.gameObject != null)
        {
            Destroy(LostItem?.gameObject);
            LostItem = null;
        }

	}
}
