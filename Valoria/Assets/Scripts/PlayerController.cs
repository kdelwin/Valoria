using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float horizontalMove;
    public float verticalMove;

    private Vector3 playerInput;

    public CharacterController player;
    public float playerSpeed;
    public GameObject Player1, Player2;
    public float gravity = 9.8f;
    public float fallVelocity;
    public float jumpForce;

    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 movePlayer;

    public Image selectPlayer;

    public bool isOnSlope = false;
    private Vector3 hitNormal;
    public float slideVelocity;
    public float slopeForceDown;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        camDirection();

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;

        movePlayer = movePlayer * playerSpeed;

        player.transform.LookAt(player.transform.position + movePlayer);

        SetGravity();

        PlayerSkills();

        player.Move(movePlayer  * Time.deltaTime);

        if (selectPlayer.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)){
                selectPlayer.gameObject.SetActive(false);
                Player1.gameObject.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                selectPlayer.gameObject.SetActive(false);
                Player2.gameObject.SetActive(true);
            }
        }

    }

   void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    //funcion para las habilidades de nuestro jugador

    public void PlayerSkills()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerSpeed = playerSpeed * 2;
            jumpForce = (float)(jumpForce * 1.5);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerSpeed = playerSpeed / 2;
            jumpForce = (float)(jumpForce / 1.5);
        }

    }

    void SetGravity()
    {

       if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
       else
        {
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }

        SlideDown();
    }

    public void SlideDown()
    {
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;

        if (isOnSlope)
        {
            movePlayer.x += ((1f - hitNormal.y) * hitNormal.x) * slideVelocity;
            movePlayer.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;

            movePlayer.y += slopeForceDown;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

}
