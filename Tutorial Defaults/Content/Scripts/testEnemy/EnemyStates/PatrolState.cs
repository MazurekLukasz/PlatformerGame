using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private TestEnemy enemy;
    private float patrolTimer;
    private float patrolDuration = 10f;

    public void Enter(TestEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void Execute()
    {
        if (enemy.Target == null)
        {
            Patrol();
            enemy.Move();
        }
        else if (enemy.Target != null && enemy.InMeeleRange)
        {
            enemy.ChangeState(new MeeleState());
        }
        else
        {
            enemy.Move();
        }
    }

    public void Exit()
    {
   
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "Edge")
        {
            enemy.ChangeDirection();
        }
    }

    private void Patrol()
    {
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
