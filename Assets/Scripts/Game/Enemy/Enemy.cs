using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject skin;
    private Animator _animator;
    
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public GameObject projectile;

    //health
    public HealthBar healthBar;
    public int maxHealth;
    public int currentHealth;

    //Patrol
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    private bool _alreadyAttacked;
    

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        _animator = skin.GetComponent<Animator>();
        
        //health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(maxHealth);
    }


    // Update is called once per frame
    void Update()
    {
        //Check sight and range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }


    private void Patroling()
    {
        _animator.SetBool("isRunning", false);
        _animator.SetBool("isWalking", true);
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);

        //una vez llega al punto busca una nueva posicion
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //para que no pueda generar el punto fuera del mapa
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    private void ChasePlayer()
    {
        _animator.SetBool("isRunning", true);
        _animator.SetBool("isWalking", false);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        _animator.SetBool("isRunning", false);
        _animator.SetBool("isWalking", false);
        // Para que no se mueva cuando ataca
        agent.SetDestination(transform.position);

        transform.LookAt(player);
        
        if (!_alreadyAttacked)
        {
            //Ataques
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
            rb.AddForce(transform.up * 6f, ForceMode.Impulse);
            

            _alreadyAttacked = true;
            Invoke(nameof(AttackAnimation), timeBetweenAttacks - 2.5f);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void AttackAnimation()
    {
        _animator.ResetTrigger("isAttacking");
        _animator.SetTrigger("isAttacking");
    }
    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        DestroyEnemy();
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        
        Vector3 direction = transform.forward * 10;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, direction);
    }
}