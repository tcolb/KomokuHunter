using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using TMPro;

public class Patrol : MonoBehaviour
{
    #region Variables
    public Transform mongus;
    public PointOfInterest[] poi;
    public PointOfInterest endPoint;
    private bool PleaseGodKillme = false;
    private bool hintFlag = true;
    private NavMeshAgent patronAgent;

    public TextMeshPro hintText;
    public string hintMessage = "I need a ";
    public string defaultHintMessage;

    public float HintWaitSeconds;

    Patron patron;
    Player player;
    PointOfInterest curPoint;
    NeedsAttributes needs;
    float rotationSpeed = 12f;
    #endregion


    #region Unity Scripts
    void Start(){
        player = FindObjectOfType<Player>();
        patron = gameObject.GetComponent<Patron>();
        patronAgent = patron.GetComponent<NavMeshAgent>();
        patronAgent.autoBraking = false;
        hintMessage = defaultHintMessage;
        MoveToPoint();
        
    }

    IEnumerator GiveHints()
    {

        if (patron.isSatisfied == false && patron.Needs != null)
        {
           // Debug.Log("Starting to update message");
            hintMessage = "I need a " + patron.Needs.Model.ToString();
            yield return new WaitForSeconds(HintWaitSeconds);
            hintMessage += " that is " + patron.Needs.Color.ToString();
            patron.curPoints -= 150;
            yield return new WaitForSeconds(HintWaitSeconds);
            hintMessage += " and has " + patron.Needs?.Texture.ToString();
            patron.curPoints -= 150;
        }
        yield return null;
    }

    void MoveToPoint()
    {
        if(poi.Length == 0)
        {            
            return;
        }
        foreach(PointOfInterest p in poi)
        {       
            if(p.hasPatron == false)
            {
                curPoint = p;
                patronAgent.destination = p.GetComponent<Transform>().position;
                patronAgent.isStopped = false;
                curPoint.hasPatron = true;
                break;
            }
        }
    }

    void FindEndPoint()
    {
        patronAgent.destination = endPoint.GetComponent<Transform>().position;
        PleaseGodKillme = true;
        curPoint.hasPatron = false;
    }


    void FixedUpdate(){
        hintText.text = hintMessage;
        if (PleaseGodKillme)
        {
            //Destroy(patron.GameObject);
        }
        //Patron is Satisfied and moving back to endpoint
        if (patron.isSatisfied && !PleaseGodKillme)
        {
            hintMessage = "Thanks bruh you're like totally epic";
            hintFlag = true;
            StopCoroutine(GiveHints());
            patronAgent.isStopped = false;
            FindEndPoint();
        }
        //Patron has reached their point
        if(!patronAgent.pathPending && patronAgent.remainingDistance < .05f)
        {
            patronAgent.isStopped = true;
            //Give needs to patron once the reach the counter
            if(patron.Needs == null)
            {
                patron.AssignRandomNeeds();
            }
            if(hintFlag == true)
            {
                hintFlag = false;
                StartCoroutine(GiveHints());
            }
           
        }
        //Patron reached endpoint
        if(patron.isSatisfied && (Vector3.Distance(patronAgent.transform.position, endPoint.transform.position)) <= .05f)
        {           
            PleaseGodKillme = false;
            patron.isSatisfied = false;
            patron.Needs = null;
            hintMessage = defaultHintMessage;
            hintFlag = true;
            MoveToPoint();
        }
    }
    #endregion
}
