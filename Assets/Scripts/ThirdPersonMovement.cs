using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float speed = 6f;
    internal bool playerSpawned = false;
    public float turnSmoothTime = 0.1f;
    float turnSmoothvelocity;

    // Update is called once per frame
    void Update()
    {
        if (playerSpawned)
        { 
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;

        if (dir.magnitude >= 0.1f)
        {
            transform.GetChild(0).GetComponent<Animator>().SetBool("walk", true);
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                speed = 12;
                transform.GetChild(0).GetComponent<Animator>().SetBool("walk", false);
                transform.GetChild(0).GetComponent<Animator>().SetBool("run", true);
            }
            else
            {
                speed = 6;
                transform.GetChild(0).GetComponent<Animator>().SetBool("run", false);
                transform.GetChild(0).GetComponent<Animator>().SetBool("walk", true);
            }
            float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothvelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else
        {
            transform.GetChild(0).GetComponent<Animator>().SetBool("walk", false);
            transform.GetChild(0).GetComponent<Animator>().SetBool("run", false);
        }
    }
        }
}
