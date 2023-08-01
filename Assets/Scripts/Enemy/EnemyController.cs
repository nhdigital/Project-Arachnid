
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using UnityEditor.ShaderGraph;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public EnemyStats attributes;
    private float maxHealth;
    private float currentHealth;

    private FieldOfView fov;

    [SerializeField] 
    private Shader shader;
    [SerializeField]
    private Material material;
    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer;
    public float startValue = 1f;
    public float targetValue = 0f;
    public float duration = 2f; // duration in seconds



    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Gradient healthGradient;
    [SerializeField]
    private Image healthFillImage;

    private NavMeshAgent agent;
    public float lookRadius = 10f;

    [SerializeField]
    private Collider enemyCollider;

    private Transform target;
    
    [SerializeField]
    private Animator anim;

    public Transform[] waypoints;
    int waypointIndex = 0;
    
    [SerializeField]
    private float pauseDuration = 8f;
    private float waitTimer = 0f;

    bool isDead;
    bool isWaiting = false;

    // canvas
    [SerializeField]
    private Canvas enemyCanvas;
    [SerializeField]
    private TextMeshProUGUI enemyName;
    [SerializeField]
    private TextMeshProUGUI enemyHealth;
    


    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfView>();
        enemyCanvas.enabled = false;
        maxHealth = attributes.health;
        currentHealth = maxHealth;
        enemyName.text = attributes.name;
        enemyHealth.text = attributes.health.ToString();
        skinnedMeshRenderer.material.SetFloat("_FadeIn", startValue);
        Debug.Log(maxHealth);
        isDead = false;
        SetNextWaypoint();
    }




    void Update()
    {

      

        float distance = Vector3.Distance(target.position, transform.position);

        // movement only occurs when AI is not dead
        if (!isDead)
        {
            // if player distance is within the look radius, chasing starts
            if (fov.canSeePlayer)
            {
                enemyCanvas.enabled = true;
                // increase speed of AI when chasing
                agent.speed = 1.5f;
                // AI state shifts to chasing animation
                anim.SetBool("isChasing", true);
                // move the nav mesh towards player
                agent.SetDestination(target.position);

                // attacking logic starts when AI reaches player
                if (distance <= agent.stoppingDistance + 3f)
                {
                    // attack the target
                    FaceTarget();
                    Attack();
                    agent.isStopped = true;
                }
                else
                {
                    anim.SetBool("isAttacking", false);
                    agent.isStopped = false;
                }
            }
            else
            {
                enemyCanvas.enabled = false;
                // when the player is outside of the defined look radius, patrolling starts
                Patrolling();
                
            }
        }
        else
        {
            Debug.Log("DEAD");
        }
    }

    private void Attack()
    {
        anim.SetBool("isAttacking", true); 
    }

    private void Patrolling()
    {
        // slower patrol speed
        agent.speed = 1.2f;
        // ensure state returns to walking
        anim.SetBool("isChasing", false);
        // check if AI has reached its destination waypoint
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // If waiting, increment the wait timer
            if (isWaiting)
            {
                // AI state shifts to idle animation during waiting period
                anim.SetBool("isWalking", false);
                waitTimer += Time.deltaTime;
                // check if the waiting duration has been reached
                if (waitTimer >= pauseDuration)
                {
                    isWaiting = false;
                    waitTimer = 0f;
                    SetNextWaypoint(); // move to the next waypoint after the pause
                }
            }
        }
        else
        {
            isWaiting = true; 
        }
    }

    private void SetNextWaypoint()
    {
        // AI state shifts to walking animation
        anim.SetBool("isWalking", true);
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints defined for AI patrol");
            return;
        }
        // move AI to waypoint
        int randomWaypointIndex = Random.Range(0, waypoints.Length);
        agent.destination = waypoints[randomWaypointIndex].position;
        // Increment waypoint array
        waypointIndex = (waypointIndex + 1) % waypoints.Length;

        // start waiting at the waypoint
        isWaiting = true;
        // reset waitTimer
        waitTimer = 0f;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        enemyHealth.text = currentHealth.ToString();
        UpdateHealthBar();
        if(currentHealth <= 0f)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        healthSlider.value = currentHealth / maxHealth;
        healthFillImage.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }


    void Die()
    {

        //material.SetFloat("_FadeIn", 0.5f);
        StartCoroutine(Dissolve());
        
       // Destroy(gameObject);
    }

    public IEnumerator Dissolve()
    {
        enemyCollider.enabled = false;
        agent.isStopped = true;
        isDead = true;
        anim.SetTrigger("killed");
        enemyHealth.text = "Killed";
        enemyName.enabled = false;



  
        float currentTime = 0f;

        while (currentTime < duration)
        {
            float normalizedTime = currentTime / duration;
            float lerpedValue = Mathf.Lerp(startValue, targetValue, normalizedTime);
            skinnedMeshRenderer.material.SetFloat("_FadeIn", lerpedValue);
            currentTime += Time.deltaTime;
            yield return null;
        }



        Destroy(gameObject);
   

     
    }
}
