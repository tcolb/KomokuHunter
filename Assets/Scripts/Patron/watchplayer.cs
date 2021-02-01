using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class watchplayer : MonoBehaviour
{
    Transform player;
    public float rotationSpeed;
    Quaternion rotation;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = player.position - transform.position;
        float step = rotationSpeed * Time.deltaTime;

        rotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, step);
    }
}
