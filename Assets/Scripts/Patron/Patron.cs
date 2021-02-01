using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;


public class Patron : MonoBehaviour
{
    #region Variables
    public Transform patronHands;
    public NeedsAttributes Needs;
    bool isItemHeld = false;
    GameObject patronHolding = null;

    //till we can actually check item properties keep true
    bool correctItem = true;
    public bool isSatisfied = false;
    Player player;
    public AudioSource playPoints;

    public int curPoints = 500;

    #endregion

    #region Unity Scripts
    void Start()
    {
        player = FindObjectOfType<Player>();
        //select an item/needs
    }

    public void AssignRandomNeeds()
    {
        if (patronHolding?.gameObject != null)
        {
            Destroy(patronHolding.gameObject);
        }

        var lostItem = Main.Instance.GetRandomSpawnedEntity()?.GetComponent<LostItemEntity>();
        Needs = lostItem?.Attributes;

        patronHands.GetComponent<ItemHinting>().Init(lostItem);
	}

    // TODO for debugging, remove this
	/*private void OnMouseDown()
	{
        AssignRandomNeeds();
	}*/
    /*IEnumerator GiveHints()
    {
        if(isSatisfied == true){
            hintMessage += Needs.Model.ToString();
            yield return new WaitForSeconds(10);
            hintMessage += "that is " + Needs.Color.ToString();
            curPoints -= 100;
            yield return new WaitForSeconds(10);
            hintMessage += " and has the " + Needs.Texture.ToString() + " pattern";
            curPoints -= 100;
        }
        yield return null;
    }*/

    void HandlePoints(int score)
    {
        player.points += score;  
        if(player.points == 10000)
        {
            playPoints.Play(0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Pick up item
        if (other.gameObject.tag == "Item")
        {         
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<Rigidbody>().angularDrag = 0f;
            other.gameObject.transform.rotation = Quaternion.identity;
            other.gameObject.transform.position = patronHands.position;
            other.gameObject.transform.parent = patronHands;

            isItemHeld = true;
            patronHolding = other.gameObject;

            var lostItemComponent = patronHolding.GetComponent<LostItemEntity>();
            correctItem = lostItemComponent.Attributes.Equals(Needs);
           
            if (player.heldItem != null)
            {
                player.forceRelease = true;               
            }
            
        }

        if (player.heldItem != null)
        {
            patronHolding = null;
            isItemHeld = false;
        }
        if (isItemHeld)
        {
            if (correctItem)
            {                
                HandlePoints(curPoints);
                patronHands.GetComponent<ItemHinting>().DisableHinting();

                if (patronHolding?.gameObject != null)
                {
                    Destroy(patronHolding.gameObject);
                }
                isSatisfied = true;
                curPoints = 500;
                correctItem = false;
            }
            else
            {
                HandlePoints(-200);
                patronHolding.GetComponent<Rigidbody>().isKinematic = false;
                patronHolding.transform.position = player.hands.position;
                player.heldItem = patronHolding.gameObject;
                patronHolding.transform.parent = player.hands;
            }

            //Debug.Log("isItemHeld is set to false");
            isItemHeld = false;
        }
    }
    #endregion
}
