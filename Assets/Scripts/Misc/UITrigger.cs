/*
 * 
 * 
 * 
 * 
 * Script no longer in use
 * 
 * 
 * 
 * 
 */











using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITrigger : MonoBehaviour
{
    #region Variables
    Transform target;
    Player player;
    public float rotation_speed = 1.0f;
    GameObject speech;
    Text textbox;
    public Patron patron;
    #endregion

    private void Start()
    {
        player = FindObjectOfType<Player>();
        target = player.transform;
        speech = transform.GetChild(0).gameObject;
        textbox = speech.GetComponent<Text>();
        patron = gameObject.GetComponentInParent<Patron>();
        StartCoroutine(Checkforneeds());
        StartCoroutine(Trackplayer());
    }

    private void Update()
    {
    }

/*    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // print("The Chungoid has entered the Amoongus field");
            speech.SetActive(true);
            StartCoroutine("trackplayer");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // print("The Chungoid has exited the Amoongus field");
            speech.SetActive(false);
            StopCoroutine("trackplayer");
        }
    }*/

    IEnumerator Trackplayer()
    {
        while (true)
        {
            Vector3 targetDirection = target.position - transform.position;
            float step = rotation_speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection,
                step, 0.0f);

            // Draw a ray
            //Debug.DrawRay(transform.position, newDirection, Color.red);

            transform.rotation = Quaternion.LookRotation(newDirection);
            yield return null;
        }
    }

    // the patron script should probably notify 
    // when the needs are changed (but maybe that will never happen in the game)
    // I didnt want to edit patron for now... - Vincent
    IEnumerator Checkforneeds()
    {
        NeedsAttributes needs;
        string message = "Give me the chungus burger, no onions";
        string model;
        string color;
        string pattern;
        int counter = 0;
        while (true)
        {
            if (counter > 3)
            {
                message = "Ok buddy, did you just blow in from stupid town, or what?";
                counter = 0;
            }
            else
            {
                needs = patron.Needs;
                if (needs != null)
                {
                    model = needs.Model.ToString();
                    color = needs.Color.ToString();
                    pattern = needs.Texture.ToString();
                    message = "Give me the " + color + " " + model + " with " + pattern;
                    counter++;
                }
            }
            textbox.text = message;
            yield return new WaitForSeconds(5);
        }
    }
}
