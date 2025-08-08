using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public NavMeshAgent agent;
    public Transform player;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public float attackDelay = 0.7f;
    private float lastAttackTime = -Mathf.Infinity;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthManager playerHealthManager;
    private Animator animator;
    public PointManager pointManager;
    public AudioSource audioSource;
    public AudioClip attackSound;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerHealthManager = playerObject.GetComponent<HealthManager>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        pointManager = playerObject.GetComponent<PointManager>();
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                agent.isStopped = true;
                animator.SetBool("isWalking", false);

                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    lastAttackTime = Time.time;
                    Attack();
                }
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
                if (agent.velocity.magnitude > 0.1f)
                {
                    animator.SetBool("isWalking", true);
                }
                else
                {
                    animator.SetBool("isWalking", false);
                }
            }
        }
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        StartCoroutine(DealDamageAfterDelay(attackDelay));
    }

    IEnumerator DealDamageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!gameObject.CompareTag("BigEnemy"))
        {
            playerHealthManager.TakeDamage(10);
            audioSource.Play();
        }
        else
        {
            playerHealthManager.TakeDamage(30);
            audioSource.Play();
        }
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (gameObject.CompareTag("HealthEnemy"))
        {
            playerHealthManager.Heal(50);
        }
        
        if (gameObject.CompareTag("BigEnemy"))
        {
            pointManager.AddPoints(100 + pointManager.addAmount);
        }
        else
        {
            pointManager.AddPoints(50 + pointManager.addAmount);
        }
        FindObjectOfType<RoundManager>().EnemyKilled();
        Destroy(gameObject);
    }
}
