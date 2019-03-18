using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float hitPoints;   

    private bool attak = true;
       
    public void DamageToObstacles(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            Destroy(transform.gameObject);
            StatPlayer.countEnemy += 1;
            Debug.Log(StatPlayer.countEnemy);
        }       
    }         
        
    [Header("Nav mesh settings")]
    [SerializeField]
    private Transform player;

    [SerializeField]
    private UnityEngine.AI.NavMeshAgent navMesh;

    [SerializeField]
    private Animator anim;

    // состояние врага
    private string state = "idle";
    private bool isAlive = true;

    [Header("Enemy settings")]
    [SerializeField]
    private float searchRadius;

    [SerializeField]
    private float waitTime;
    private float wait;
    
    private bool highAlert = false;
    private float alertLevel = 0;

    private void Start()
    {
        navMesh.speed = 1;
        anim.speed = 1;
    }

    private void Update()
    {
        if (isAlive == false)
            return;

        if (attak)
        {
            anim.SetFloat("speed", navMesh.velocity.magnitude);

            // если остановился
            if (state == "idle")
            {
                // иду к следующей точке
                GoToRandomPoint();
            }

            // пока иду
            if (state == "walk")
            {
                // проверяю дистанцию до цели
                CheckDistance();
            }

            // если осматриваюсь
            if (state == "search")
            {
                // осматриваюсь
                Search();
            }

            if (state == "chase")
            {
                ChaseForPlayer();
            }

            if (state == "hunt")
            {
                CheckDistance();
            }

            if (state == "killed")
            {
                var buf = GetComponent<Enemy>();
                Destroy(buf.gameObject);
            }
        }
        else
        {
            Rotate();           
        }
        CheckTarget();
    }

    public void SetStatekilled()
    {
        state = "killed";
    }


    private void ChaseForPlayer()
    {
        navMesh.SetDestination(player.position);

        float distance = Vector3.Distance(transform.position, player.position);

        var remainingDistance = navMesh.remainingDistance;
        var stoppingDistance = navMesh.stoppingDistance;

        if (distance > 10)
        {
            state = "hunt";
            highAlert = true;
            alertLevel = 20;
        }        
    }   

    private void RestartLevel()
    {
        var currentScena = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScena);
    }    

    private void GoToRandomPoint()
    {
        // генерируем случайную позицию внутри сферы
        Vector3 randomPos = UnityEngine.Random.insideUnitSphere * searchRadius;
        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(
            transform.position + randomPos,
            out navHit,
            searchRadius,
            UnityEngine.AI.NavMesh.AllAreas
        );

        if (highAlert)
        {
            UnityEngine.AI.NavMesh.SamplePosition(
           player.position,
           out navHit,
           searchRadius,
           UnityEngine.AI.NavMesh.AllAreas
       );
        }
        alertLevel -= 5;
        if (alertLevel <= 0)
        {
            highAlert = false;
            navMesh.speed = 1;
            anim.speed = 1;
        }
        navMesh.SetDestination(navHit.position);
        state = "walk";
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
            transform.LookAt(other.transform);
            anim.SetBool("attak", true);
            attak = false;
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10))
            {
                StatPlayer.hp -= 0.02f;
            }
        }
    }

    private void CheckTarget()
    {
        var enemy = GameObject.FindGameObjectWithTag("Player");

        float shortestDistance = Mathf.Infinity;

        GameObject nearestEnemy = null;                
           
            var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

        if (nearestEnemy != null && shortestDistance <= 5)
        {
            transform.LookAt(player.transform);
            anim.SetBool("attak", true);
            attak = false;
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10))
            {
                StatPlayer.hp -= 1;
            }
            state = "chase";
        }
        else
        {
            anim.SetBool("attak", false);
            attak = true;            
        }
            
    }        

    private void Rotate()
    {
        var direction = player.position - transform.position;
        Quaternion lookRotatino = Quaternion.LookRotation(direction);
        var rotation = Quaternion.Lerp(transform.rotation, lookRotatino, Time.deltaTime * 100).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetBool("attak", false);
            attak = true;
            state = "chase";
        }
    }

    private void CheckDistance()
    {
        var remainingDistance = navMesh.remainingDistance;
        var stoppingDistance = navMesh.stoppingDistance;
        // когда достигли цели
        if (remainingDistance <= stoppingDistance && navMesh.pathPending == false)
        {
            state = "search";
            wait = waitTime;
        }
    }

    private void Search()
    {
        if (wait <= 0)
        {
            state = "idle";
            return;
        }

        wait -= Time.deltaTime;
        transform.Rotate(0, 120f * Time.deltaTime, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}