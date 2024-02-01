using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharProperties
{
    public float moveSpeed;
    public float moveSpeedPlus;
    public float rotSpeed;
    public float gravity;
    public float jumpHeight;
}

public class CharProxyMain : MonoBehaviour
{
    float deltaTime;
    public Camera viewCam;
    public CharacterController characterController;
    public bool isHeadShake;
    public Animation headShakeAnim;
    string animationRecord;

    public CharProperties charProperties;
    public Vector3 moveDir;
    public Vector3 rotDir;
    float moveSpeedPlusTemp;




    void Start()
    {

    }


    void Update()
    {
        deltaTime = Time.deltaTime;

        //
        CursuorAction();
        CameraAction();
        MoveAction();
    }

    public void CursuorAction()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void CameraAction()
    {
        if (Input.GetMouseButton(1))
        {
            rotDir.x += Input.GetAxis("Mouse X") * charProperties.rotSpeed * deltaTime;
            rotDir.y -= Input.GetAxis("Mouse Y") * charProperties.rotSpeed * deltaTime;
            rotDir.y = Mathf.Clamp(rotDir.y, -90, 90);
            viewCam.transform.localRotation = Quaternion.Euler(new Vector3(rotDir.y, 0, 0));
            transform.rotation = Quaternion.Euler(new Vector3(0, rotDir.x, 0));
        }

        if (isHeadShake && (characterController.isGrounded && (moveDir.x != 0 || moveDir.z != 0)))
        {
            if (animationRecord != "ANIM_HeadShake")
            {
                animationRecord = "ANIM_HeadShake";
                headShakeAnim.CrossFade(animationRecord, 0.25f);
            }
        }
        else
        {
            animationRecord = "ANIM_HeadShakeEnd";
            headShakeAnim.CrossFade(animationRecord, 0.25f);
        }
    }

    public void MoveAction()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeedPlusTemp = charProperties.moveSpeedPlus;
            headShakeAnim[animationRecord].speed = charProperties.moveSpeedPlus / 2f;
        }
        else
        {
            moveSpeedPlusTemp = 1;
            headShakeAnim[animationRecord].speed = 1;
        }

        //
        moveDir.x = Input.GetAxis("Horizontal") * charProperties.moveSpeed * moveSpeedPlusTemp;
        moveDir.z = Input.GetAxis("Vertical") * charProperties.moveSpeed * moveSpeedPlusTemp;

        if (characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                moveDir.y = charProperties.jumpHeight;
            }
            else
            {
                moveDir.y = -1;
            }
        }
        else
        {
            moveDir.y -= charProperties.gravity;
        }

        moveDir = transform.TransformDirection(moveDir);
        characterController.Move(moveDir * deltaTime);
    }
}
