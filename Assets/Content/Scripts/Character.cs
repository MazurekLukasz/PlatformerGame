using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float MovementSpeed = 5f;

    [SerializeField] protected int health;

    // [SerializeField] protected int maxHealth;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int strength;
    [SerializeField] protected int inteligence;

    protected bool faceRight;
    public bool Attack { get; set; }
   
    public bool TakingDamage { get; set; }
    public abstract bool IsDead { get; }

    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private GameObject spellPos;

    [SerializeField] private GameObject attackPos;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask whatIsEnemies;
    public Animator MyAnimator { get; private set; }
    protected SpriteRenderer spriteRenderer;

    public virtual void Start()
    {
        health = maxHealth;
        faceRight = true;
        MyAnimator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        
    }

    public virtual void TakeDamage(int str)
    {
        Debug.Log("ouch !!!");
        health -= str;
    }

    public void ChangeDirection()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        faceRight = !faceRight;

        flipObject(attackPos);
        flipObject(spellPos);
    }

    void flipObject(GameObject obj)
    {
        obj.transform.localPosition = new Vector3(obj.transform.localPosition.x * (-1), obj.transform.localPosition.y, obj.transform.localPosition.z);
    }

    public void MeeleAttack()
    {
        Debug.Log("Attack !!!!!!!");
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.transform.position,attackRange, whatIsEnemies); 

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<Character>().TakeDamage(strength);
        }
    }

    public void Shot(int value)
    {
        if (faceRight)
        {
            GameObject tmp = (GameObject) Instantiate(spellPrefab, spellPos.transform.position,Quaternion.Euler(new Vector3(0,0,0)));
            tmp.GetComponent<Projectile>().Initialize(Vector2.right, inteligence);
        }
        else
        {
            GameObject tmp = (GameObject)Instantiate(spellPrefab, spellPos.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            tmp.GetComponent<Projectile>().Initialize(Vector2.left, inteligence);
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.transform.position, attackRange);
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Damage")
        {
            TakeDamage(other.GetComponent<Projectile>().Power);
            //Destroy(other);
            //DestroyObject(other);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
    }



}
