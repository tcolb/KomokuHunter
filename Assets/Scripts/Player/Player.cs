using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    #region Variables
    public float mouseSense = 150f;
    public float throwForce = 100f;
    public bool forceRelease = false;
    public Transform hands;

    public Camera playerCamera;

    public Transform upperCameraPosition;
    public Transform lowerCameraPosition;
    float yRotation = 0f;

    public int points = 0;
    public TextMeshPro pointText;

    #endregion


    #region Unity Scripts
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    Vector3 lastHandsPosition;
    float mouseX, mouseY;
	

    void Update()
    {
        if (Time.time < 1)
        {
            yRotation = 0f;
		}

        mouseX = Input.GetAxis("Mouse X") * mouseSense * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSense * Time.deltaTime;

        //Send Points to TextMesh
        if (pointText != null && points != 0)
        {
            if (points < 0) points = 0;
            pointText.text = "Score: " + points.ToString();
        }

        // Rotate the held item or rotation the camera?
        if (Input.GetKey(KeyCode.Mouse2) && heldItem != null)
        {
            playerAppliedItemRotation = new Vector3(mouseX, 0, mouseY);
        }
        else
        {
            playerAppliedItemRotation = Vector3.zero;

            yRotation -= mouseY;
            yRotation = Mathf.Clamp(yRotation, -80f, 90f);
            playerCamera.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        // Crouch down
        if (Input.GetKey(KeyCode.LeftControl))
        {
            CrouchTowards(lowerCameraPosition.position);
        }
        else
        {
            CrouchTowards(upperCameraPosition.position);
        }

        // Handle quit
        if (Input.GetKey(KeyCode.Escape))
        {
/*#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif*/
		}

        //Allow Patron to forcibly remove item from player.
        if(forceRelease && (heldItem != null))
        {
            ReleaseItem();         
        }

        // The order of these three if statements matters
        // Also these *should* all probably be in FixedUpdate but it looks glitchy that way

        if (heldItem != null)
        {
            updateHeldItemForces();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            handlePlayerRightClick();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            handlePlayerLeftClick();
        }

        // this has to be set last
        lastHandsPosition = hands.position;
    }

    const float crouchLerpSpeed = 0.25f;
    const float doneCrouchingTowardsDist = 0.005f;
    void CrouchTowards(Vector3 position)
    {
        var newPos = Vector3.Lerp(playerCamera.transform.position, position, crouchLerpSpeed);
        var distToPos = Vector3.Distance(newPos, playerCamera.transform.position);
        if (distToPos < doneCrouchingTowardsDist)
        {
            return;
		}

        playerCamera.transform.position = newPos;
    }

    Vector3 playerAppliedItemRotation;
    bool hasCloseHeld = false;
    float originalAngularDrag = 0;
    public GameObject heldItem = null;

    void updateHeldItemForces()
    {
        var rigidBody = heldItem.GetComponent<Rigidbody>();

        const float pickupStickFactor = 1000;
        const float velToPosDistFactor = 1.5f;
        const float dropDistFactor = 3f;

        var heldDist = Vector3.Distance(hands.position, heldItem.transform.position);

        if (heldDist < velToPosDistFactor)
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.MovePosition(Vector3.Lerp(heldItem.transform.position, hands.position, 0.8f));

            if (Input.GetKey(KeyCode.Mouse2))
            {
                // TODO: need to factor in player.transform.rotation to maintain rotation relative to player
                rigidBody.MoveRotation(rigidBody.rotation * Quaternion.Euler(playerAppliedItemRotation)); 
            }
            else
            {
                // TODO: remove stuttering caused by this, solution: use quaternions instead, using RigidBody.MoveRotation to apply rotation
                // heldItem.transform.RotateAround(heldItem.transform.position, Vector3.up, mouseX);
                rigidBody.MoveRotation(Quaternion.Lerp(rigidBody.rotation, transform.rotation, 0.05f));
			}
            
            hasCloseHeld = true;
        }
        else if (heldDist > dropDistFactor && hasCloseHeld)
        {
            ReleaseItem();
        }
        else
        {
            // TODO: apply force based on distance from item
            var vel = hands.position - heldItem.transform.position;
            rigidBody.AddForce(vel * pickupStickFactor * Time.deltaTime);
        }
    }

    void ReleaseItem()
    {
        var rigidBody = heldItem.GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        rigidBody.angularDrag = originalAngularDrag;
        rigidBody.useGravity = true;
        rigidBody.AddForce(hands.position - lastHandsPosition);

        heldItem = null;
        hasCloseHeld = false;
        forceRelease = false;
    }

    void handlePlayerLeftClick()
    {
        if (heldItem != null)
        {
            ReleaseItem();
            return;
        }

        var ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));

        const float maxGrabDist = 6f;
        if (Physics.Raycast(ray, out var hit, maxGrabDist))
        {
            if (hit.collider.tag == "Item")
            {
                forceRelease = false;
                heldItem = hit.collider.gameObject;
                var rigidBody = heldItem.GetComponent<Rigidbody>();
                originalAngularDrag = rigidBody.angularDrag;

                const float heldAngularDrag = 5f;
                rigidBody.angularDrag = heldAngularDrag;
                rigidBody.useGravity = false;
                rigidBody.angularVelocity = Vector3.zero;
                //rigidBody.isKinematic = true;
            }
        }
    }

    void handlePlayerRightClick()
    {

        if (heldItem != null)
        {
            heldItem.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
            ReleaseItem();
		}
    }
    #endregion
}
