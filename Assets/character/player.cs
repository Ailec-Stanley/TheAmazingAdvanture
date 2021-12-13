using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour{
    Rigidbody2D rb;
    Animator anim;
    public Collider2D coll;
    public GameObject shadow_right;
    public GameObject shadow_left;
    public GameObject shield;
    public GameObject showShieldCD;
    public GameObject hangParticle;

    public float maxFallingSpeed;
    public float moveSpeed = 10f;
    public float jumpForce;
    public float dashTime;
    public float dashForce;

    public float hangingJumpTime;
    
    public float shieldTime;
    public float shieldCD;

    public float deadTime;

    public Transform groundCheck;
    public Transform wallCheck_front;
    public Transform wallCheck_back;
    public LayerMask ground;

    float sacling;

    float horizontalMove;

    bool isGround, isJump, isDash, isHang, inAir;
    bool jumpPressed;
    int jumpCount;

    bool dashPressed;
    int dashCount;
    float remindDashTime;

    bool front_check;
    bool back_check;

    bool hangingJump;
    float hangingJumpRemindTime;

    float shieldRemindTime;
    float shiedlCDTime;
    bool isShield;

    bool isDead;
    

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        shadow_left.SetActive(false);
        shadow_right.SetActive(false);
        shield.SetActive(false);
        sacling = transform.localScale.x;
        isDead = false;
        hangParticle.SetActive(false);
    }

    // Update is called once per frame
    void Update(){
        if(isDead){
            return;
        }
        horizontalMove = Input.GetAxis("Horizontal");
        if(Input.GetButtonDown("Jump") && jumpCount > 0){
            jumpPressed = true;
        }
        if(Input.GetButtonDown("Dash") && dashCount > 0){
            dashPressed = true;
        }
        if(Input.GetButtonDown("Shield") && shiedlCDTime <= 0){
            shiedlCDTime = shieldCD;
            shieldRemindTime = shieldTime;
        }
        showShield();
    }

    void FixedUpdate(){
        if(isDead){
            deadTime -= Time.deltaTime;
            if(deadTime <= 0){
                gameObject.SetActive(false);
            }
            rb.velocity = new Vector3(0,0,0);
            anim.SetBool("dead", true);
            return;
        }
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.15f, ground);
        front_check = Physics2D.OverlapCircle(wallCheck_front.position, 0.1f, ground);
        back_check = Physics2D.OverlapCircle(wallCheck_back.position, 0.1f, ground);
        isHang = ( front_check || back_check) && !isGround;
        isShield = shieldRemindTime > 0;
        checkInAir();

        jump();
        dash();
        if(!isDash){
            movement();
        }
        hang();
        fixFallingSpeed();
        
        switchAnimation();
    }
    
    void movement(){
        hangingJumpRemindTime -= Time.deltaTime;
        if(hangingJumpRemindTime < 0){
            hangingJump = false;
        }
        // Horizontal move
        if(!hangingJump){

            rb.velocity = new Vector2(horizontalMove * moveSpeed, rb.velocity.y);
            // Horizontal direction
            if(horizontalMove > 0 && !isHang){
                transform.localScale = new Vector3(sacling, sacling, sacling);
            }
            if(horizontalMove < 0 && !isHang){
                transform.localScale = new Vector3(-sacling, sacling, sacling);
            }
        }
    }

    void checkInAir(){
        if(isGround){
            inAir = false;
        }
        if(jumpCount == 2 && rb.velocity.y < -0.4f && !isHang){
            inAir = true;
            jumpCount = 1;
        }
    }

    void jump(){
        if(isGround){
            jumpCount = 2;
            isJump = false;
        }
        
        if(jumpPressed && isGround){
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }else if(jumpPressed && jumpCount > 0 && (isJump || isHang || inAir)){
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
            if(isHang){
                hangingJump = true;
                hangingJumpRemindTime = hangingJumpTime;
                rb.velocity = new Vector2(moveSpeed * transform.localScale.x, jumpForce);
            }
        }
    }

    void dash(){
        if(isGround){
            dashCount = 1;
        }

        if(dashPressed && dashCount > 0 && !isDash){
            if(isHang){
                dashCount++;
            }
            if(transform.localScale.x > 0){
                shadow_right.SetActive(true);
            }else{
                shadow_left.SetActive(true);
            }
            isDash = true;
            remindDashTime = dashTime;
            rb.velocity = new Vector2(dashForce * transform.localScale.x, 0);
            dashCount--;
            dashPressed = false;
        }else if(isDash){
            remindDashTime -= Time.deltaTime;
            if(remindDashTime > 0){
                isDash = true;
                rb.velocity = new Vector2(dashForce * transform.localScale.x, 0);
                dashPressed = false;
            }else{
                shadow_left.SetActive(false);
                shadow_right.SetActive(false);
                isDash = false;
                dashPressed = false;
            }
        }
    }

    void hang(){
        if(front_check && !isGround){
            shadow_left.SetActive(false);
            shadow_right.SetActive(false);
            hangParticle.SetActive(false);
            jumpCount = 2;
            dashCount = 1;
            rb.velocity = new Vector2(0f, -0.1f);
            transform.localScale = new Vector3(-1 * transform.localScale.x, sacling, sacling);
            isHang = true;
            isJump = false;
            isDash = false;
        }else if(back_check && !isGround && !isJump && !isDash && !(rb.velocity.x * transform.localScale.x >= 1)){
            isHang = true;
            hangParticle.SetActive(true);
            rb.velocity = new Vector2(0f, -0.16f);
        }else {
            hangParticle.SetActive(false);
            isHang = false;
        }
    }
    void showShield(){
        shieldRemindTime -= Time.deltaTime;
        shiedlCDTime -= Time.deltaTime;
        if(isShield){
            shield.transform.position = transform.position;
            shield.SetActive(true);
        }else{
            shield.SetActive(false);
        }
        if(shiedlCDTime > 0){
            showShieldCD.SetActive(false);
        }else{
            showShieldCD.SetActive(true);
        }
    }

    void fixFallingSpeed(){
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, maxFallingSpeed));
    }

    void die(){
        isDead = true;
    }
    void switchAnimation(){
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));

        if(isGround){
            anim.SetBool("falling", false);
            anim.SetBool("jumping", false);
            anim.SetBool("onGround", true);
        }else if(!isGround && rb.velocity.y > 0){
            anim.SetBool("jumping", true);
            anim.SetBool("falling", false);
            anim.SetBool("onGround", false);
        }else if(rb.velocity.y < 0){
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
            anim.SetBool("onGround", false);
        }

        if(isDash){
            anim.SetBool("dashing", true);
        }else{
            anim.SetBool("dashing", false);
        }

        if(inAir){
            anim.SetBool("air", true);
            anim.SetBool("onGround", false);
        }else{
            anim.SetBool("air", false);
        }

        if(isHang){
            anim.SetBool("hanging", true);
            anim.SetBool("falling", false);
            anim.SetBool("onGround", false);
        }else{
            anim.SetBool("hanging", false);
        }

        if(isDead){
            anim.SetBool("dead", true);
        }
        anim.SetFloat("TEST", shiedlCDTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch(other.tag){
            case "Coin":
                other.gameObject.SetActive(false);
                jumpCount = 1;
                dashCount = 1;
                break;
            case "helper":
                other.gameObject.SetActive(false);
                rb.velocity = new Vector2(rb.velocity.x, 30);
                remindDashTime = 0;
                break;
            case "danger":
                if(!isDash && !isShield){
                    die();
                }else if(isShield){
                    rb.velocity = new Vector3(rb.velocity.x, 7, 0);
                    jumpCount = 1;
                    dashCount = 1;
                }
                break;
            default:
                break;
        }
    }
}
