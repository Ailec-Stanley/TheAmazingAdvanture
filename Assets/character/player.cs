using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour{
    Rigidbody2D rb;
    Animator anim;
    //public Collider2D coll;
    public GameObject shadow_right;
    public GameObject shadow_left;
    public GameObject shield;
    public GameObject showShieldCD;
    public GameObject hangParticle;
    public AudioSource jumpAudio;
    public AudioSource doubleJumpAudio;
    public AudioSource dashAudio;
    public AudioSource hitAudio;
    GameObject m;

    GameObject wings;

    public float maxFallingSpeed;
    public float moveSpeed = 10f;
    public float jumpForce;
    public float dashTime;
    public float dashForce;

    public float hangingJumpTime;
    
    public float shieldTime;
    public float shieldCD;


    public Transform groundCheck;
    public Transform wallCheck_front;
    public Transform wallCheck_back;
    public LayerMask ground;

    float sacling;

    float horizontalMove;

    bool isGround, isJump, isDash, isHang, inAir;
    bool jumpPressed;
    bool jumpReleased;
    int jumpCount;
    public float jumpTime;
    float jumpRemindTime;
    public float wingsTime;
    float wingsRemindTime;

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

    Vector3 savedPoint;
    public float resurrectionTime;
    float resurrectionRemindTime;

    public float taijiTime;
    bool isTaiji;
    float taijiRemindTime;
    bool taiji_up;
    bool taiji_down;
    bool taiji_left;
    bool taiji_right;
    Vector3 taijiPosition;
    public float taijiMoveTime;
    float taijiMoveRemindTime;

    ParticleSystem feather;
    

    // Start is called before the first frame update
    void Start(){
        m  = transform.Find("main").gameObject;
        wings = transform.Find("wings").gameObject;
        feather = transform.Find("feather").gameObject.GetComponent<ParticleSystem>();
        var em = feather.emission;
        wings.SetActive(false);
        em.enabled = false;
        rb = GetComponent<Rigidbody2D>();
        anim = m.GetComponent<Animator>();
        shadow_left.SetActive(false);
        shadow_right.SetActive(false);
        shield.SetActive(false);
        sacling = transform.localScale.x;
        isDead = false;
        hangParticle.SetActive(false);
        savedPoint = transform.position;
        jumpReleased = true;
    }

    // Update is called once per frame
    void Update(){
        if(isTaiji){
            horizontalMove = Input.GetAxis("Horizontal");
            float verticalMove = Input.GetAxis("Vertical");
            if(horizontalMove > 0.1){
                taiji_right = true;
                taiji_left = false;
            }else if(horizontalMove < -0.1){
                taiji_right = false;
                taiji_left = true;
            }
            if(verticalMove > 0.1){
                taiji_up = true;
                taiji_down = false;
            }else if(verticalMove < -0.1){
                taiji_up = false;
                taiji_down = true;
            }
            return;
        }
        if(isDead){
            return;
        }
        horizontalMove = Input.GetAxis("Horizontal");
        if(Input.GetButton("Jump")){
            jumpPressed = true;
        }
        if(Input.GetButtonUp("Jump")){
            jumpReleased = true;
            jumpPressed = false;
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
        taiji();
        if(isDead){
            resurrectionRemindTime -= Time.deltaTime;
            if(resurrectionRemindTime < 0){
                transform.position = savedPoint;
                isDead = false;
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
    
    void taiji(){
        taijiMoveRemindTime -= Time.deltaTime;
        if(isTaiji && taijiRemindTime > 0){
            transform.position = taijiPosition;
            taijiRemindTime -= Time.deltaTime;
            Time.timeScale = 0.1f;
        }else if(isTaiji && taijiRemindTime < 0){
            taijiMoveRemindTime = taijiMoveTime;
            isTaiji = false;
            Time.timeScale = 1f;
            if(transform.localScale.x > 0){
                shadow_right.SetActive(true);
            }else{
                shadow_left.SetActive(true);
            }
        }

        if(taijiMoveRemindTime < 0){
            taiji_up = false;
            taiji_down = false;
            taiji_left = false;
            taiji_right = false;
            if(!isDash){
                if(transform.localScale.x > 0){
                    shadow_right.SetActive(false);
                }else{
                    shadow_left.SetActive(false);
                }
            }
        }else{
            float h_s, v_s;
            if(taiji_left){
                h_s = -40f;
            }else if(taiji_right){
                h_s = 40f;
            }else{
                h_s = 0f;
            }
            if(taiji_down){
                v_s = -10f;
            }else if(taiji_up){
                v_s = 15f;
            }else{
                v_s = 0f;
            }
            rb.velocity = new Vector3(h_s, v_s, 0);
        }
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
        var em = feather.emission;
        jumpRemindTime -= Time.deltaTime;
        wingsRemindTime -= Time.deltaTime;
        if(isGround){
            jumpCount = 2;
            isJump = false;
        }
        
        if(jumpPressed && isGround && jumpReleased){
            jumpAudio.Play();
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
            jumpReleased = false;
            jumpRemindTime = jumpTime;
        }else if(jumpPressed && jumpReleased && jumpCount > 0 && (isJump || isHang || inAir) && jumpRemindTime < 0){
            doubleJumpAudio.Play();
            wingsRemindTime = wingsTime;
            jumpRemindTime = jumpTime;
            wings.SetActive(true);
            em.enabled = true;

            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
            jumpReleased = false;
            if(isHang){
                wings.SetActive(false);
                em.enabled = false;
                hangingJump = true;
                hangingJumpRemindTime = hangingJumpTime;
                rb.velocity = new Vector2(moveSpeed * transform.localScale.x, jumpForce);
            }
        }
        if(jumpRemindTime > 0 && jumpPressed && !jumpReleased){
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpPressed = false;
        }
        if(wingsRemindTime < 0){
            wings.SetActive(false);
            em.enabled = false;
        }
    }

    void dash(){
        if(isGround){
            dashCount = 1;
        }

        if(dashPressed && dashCount > 0 && !isDash){
            dashAudio.Play();
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
        remindDashTime = 0f;
        resurrectionRemindTime = resurrectionTime;
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

        if(isDash || taijiMoveRemindTime > 0){
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
        }else{
            anim.SetBool("dead", false);
        }

        anim.SetFloat("TEST", shiedlCDTime);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(isDead){
            return;
        }
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
                jumpRemindTime = 0;
                jumpPressed = false;
                break;
            case "danger":
                if(!isShield){
                    die();
                }else if(isShield){
                    hitAudio.Play();
                    rb.velocity = new Vector3(rb.velocity.x, 20, 0);
                    jumpCount = 1;
                    dashCount = 1;
                }
                break;
            case "checkpoint":
                savedPoint = other.transform.position;
                break;
            case "taiji":
                if(isTaiji){
                    break;
                }
                remindDashTime = 0f;
                taijiPosition = other.transform.position;
                transform.position = taijiPosition;
                jumpCount = 1;
                dashCount = 1;
                isTaiji = true;
                taijiRemindTime = taijiTime;
                Time.timeScale = 0.1f;
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(isDead){
            return;
        }
        switch(other.collider.tag){
            case "danger":
                die();
                break;
        }
    }
}
