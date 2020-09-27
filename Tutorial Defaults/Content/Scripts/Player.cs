using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public GameObject player;

    [SerializeField] private bar healthBar;

    [SerializeField] Transform[] groundPoints;
    [SerializeField] float groundRadius;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private float jumpPower = 5f;

    public bool OnGround { get; set; }
    public bool Jump { get; set; }
    [SerializeField] private bool immortal = false;

    [SerializeField] private float immortalTime;

    public Rigidbody2D rb2d { get; set; }
    private Vector2 startPos;

    public override void  Start()
    {
        base.Start();

        healthBar.Initialize(health,maxHealth);
        startPos = transform.position;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        OnGround = IsGrounded();
        HandleMovement(horizontal);
        Flip();
        HandleLayers();

    }

    private void Flip()
    {
        if ((rb2d.velocity.x > 0.01f && !faceRight) || (rb2d.velocity.x < -0.01f && faceRight))
        {
            ChangeDirection();
        }    
    }


    private void HandleMovement(float horizontal)
    {
        if (!IsDead)
        {
            if (rb2d.velocity.y < 0)
            {
                MyAnimator.SetBool("land", true);
            }
            if (!Attack)
            {
                rb2d.velocity = new Vector2(horizontal * MovementSpeed, rb2d.velocity.y);
            }
            if (Jump && rb2d.velocity.y == 0)
            {
                rb2d.AddForce(new Vector2(0, jumpPower));
            }

            MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
        }
        
    }

    private void HandleInputs()
    {
        if (Input.GetButtonDown("Jump"))
        {
            MyAnimator.SetTrigger("jump");
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (rb2d.velocity.y > 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.5f);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            MyAnimator.SetTrigger("attack");
        }
    }



    private bool IsGrounded()
    {
        if (rb2d.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        MyAnimator.ResetTrigger("jump");
                        MyAnimator.SetBool("land", false);
                        if (colliders[i].tag == "Platform")
                        {
                            transform.parent = colliders[i].gameObject.transform;
                        }
                        
                        return true;
                        
                    }
                }

            }
        }
        transform.parent = null;
        return false;
    }

    private void HandleLayers()
    {
        if (!OnGround)
        {
            MyAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);
        }
    }

    public override void TakeDamage(int str)
    {
        if (!immortal)
        {
            base.TakeDamage(str);
            healthBar.CurrentValue = health;

            if (!IsDead)
            {
                // damage 

                //rb2d.velocity = Vector2.zero;
                MyAnimator.SetTrigger("damage");
            }
            else
            {
                // death
                rb2d.velocity = Vector2.zero;
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("death");
            }
        }
     
    }

    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            spriteRenderer.color = new Color(0,0,0);
            yield return new WaitForSeconds(.1f);

            spriteRenderer.color = new Color(1, 1, 1);
            yield return new WaitForSeconds(.1f);
        }
    }

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.tag == "Platform")
        //{
        //    player.transform.parent = other.gameObject.transform;
        //}
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        //if (other.gameObject.tag == "Platform")
        //{
        //    player.transform.parent = null;
        //}
    }
}
