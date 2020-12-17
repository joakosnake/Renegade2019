using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    Animator animator;

    //Run
    public int runSpeed = 3;

    float horizontal;
    float vertical;
    bool facingRight;

    //Crouch
    bool isCrouching;

    //Slider
    float countSlide = 0;

    //Jump
    public float jumpForce = 300;
    Rigidbody2D rigidbody1;
    float axisY;
    bool isJumping;

    //Attack
    bool isAttacking;

    //Clothes
    public GameObject knight;
    public GameObject wizard;

    //Weapon
    bool swordWeapon;
    bool fireMagic;

    //Magic
    public Magic magic;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody1 = GetComponent<Rigidbody2D>();
        rigidbody1.Sleep();

        if (knight.activeSelf)
        {
            swordWeapon = true;
            fireMagic = false;
        }
        else
        {
            swordWeapon = false;
            fireMagic = true;
        }
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        animator.SetFloat("Speed", Mathf.Abs(horizontal != 0 ? horizontal : vertical));

        if (Input.GetButton("Crouch") && (vertical == 0 && horizontal == 0))
        {
            isCrouching = true;
            animator.SetBool("IsSliding", false);
            animator.SetBool("IsCrouching", isCrouching);
        }
        else if (Input.GetButtonDown("Crouch") && horizontal != 0 && !isCrouching)
        {
            countSlide = 0.5f;
            animator.SetFloat("Speed", 0.0f);
            animator.SetBool("IsSliding", true);
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            isCrouching = false;
            animator.SetBool("IsCrouching", isCrouching);
        }

        if (countSlide > 0)
        {
            animator.SetFloat("Speed", 0.0f);
            countSlide = countSlide - (1f * Time.deltaTime);
            if (countSlide <= 0)
                animator.SetBool("IsSliding", false);
        }

        if(Input.GetKey("1"))
        {
            swordWeapon = true;
            fireMagic = false;
            wizard.SetActive(false);
            knight.SetActive(true);
        }
        if(Input.GetKey("2"))
        {
            swordWeapon = false;
            fireMagic = true;
            knight.SetActive(false);
            wizard.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Fire1") && !isJumping && swordWeapon)
        {
            isAttacking = true;
            if (vertical != 0 || horizontal != 0)
            {
                vertical = 0;
                horizontal = 0;
                animator.SetFloat("Speed", 0);
            }
            animator.SetTrigger("SwordSlash");
        }

        if (Input.GetButton("Fire2") && !isJumping && fireMagic)
        {
            isAttacking = true;
            if (vertical != 0 || horizontal != 0)
            {
                vertical = 0;
                horizontal = 0;
                animator.SetFloat("Speed", 0);
            }
            animator.SetTrigger("MagicFire");
        }

        if (transform.position.y <= axisY && isJumping)
            OnLanding();

        if (Input.GetButton("Jump") && !isJumping)
        {
            axisY = transform.position.y;
            isJumping = true;
            rigidbody1.gravityScale = 1.5f;
            rigidbody1.WakeUp();
            rigidbody1.AddForce(new Vector2(0, jumpForce));
            animator.SetBool("IsJumping", isJumping);
        }

        if ((vertical != 0 || horizontal != 0) && !isCrouching && !isAttacking)
        {
            Vector3 movement = new Vector3(horizontal * runSpeed, vertical * runSpeed, 0.0f);
            transform.position = transform.position + movement * Time.deltaTime;
        }

        Flip(horizontal);
    }

    public void AlertObservers(string message)
    {
        if (message == "MagicFire")
        {
            magic.FireAttack(!facingRight);
        }
            
        isAttacking = false;
    }

    private void Flip(float horizontal)
    {
        if (horizontal < 0 && !facingRight || horizontal > 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    //This is public because when you landing in some high o low area we have to stop the animation
    public void OnLanding()
    {
        isJumping = false;
        //isAttacking = false;
        rigidbody1.gravityScale = 0f;
        rigidbody1.Sleep();
        axisY = transform.position.y;
        animator.SetBool("IsJumping", false);
    }
}
