using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    public CharacterController playerController;
    public float playerSpeed = 12f;
    public float gravity = -9.82f;
    public float jumpHeight = 2f;

    public Transform gCheck;
    public float gDist = 0.5f;
    public LayerMask gMask;

    Vector3 playerVel;
    #endregion


    #region Unity Scripts

    void Update(){

        if(playerController.isGrounded && (playerVel.y < 0)){
            playerVel.y = -3;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * x + transform.forward * z;
        playerController.Move(movement * (playerSpeed * Time.deltaTime));

        if(Input.GetButtonDown("Jump") && playerController.isGrounded){
            //playerVel.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        playerVel.y += gravity * Time.deltaTime;
        playerController.Move(playerVel * Time.deltaTime);
    }
    #endregion
}
