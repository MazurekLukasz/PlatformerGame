using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Character
{
    private IEnemyState currentState;
    public GameObject Target { get; set; }

    [SerializeField] private float meeleRange;

    public bool InMeeleRange
    { get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meeleRange;
            }
            return false;
        }
    }



    public override void Start()
    {
        base.Start();
        ChangeState(new IdleState());
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }

            LookAtTarget();
        }
        else
        {
            Debug.Log("death");
            Destroy(this.gameObject);
        }
    }


    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDirection = Target.transform.position.x - transform.position.x;

            if (xDirection < 0 && faceRight || xDirection > 0 && !faceRight)
            {
                ChangeDirection();
            }
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }

    public void Move()
    {
        if (!Attack)
        {
            MyAnimator.SetFloat("speed", 1);
            transform.Translate(GetDirection() * (MovementSpeed * Time.deltaTime));
        }
    }

    public Vector2 GetDirection()
    {
        return faceRight ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
       base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);

    }

    public override void TakeDamage(int str)
    {
        base.TakeDamage(str);

        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            MyAnimator.SetTrigger("death");
        }
    }

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }
}
