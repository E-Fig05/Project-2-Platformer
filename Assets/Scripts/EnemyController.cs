using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private PlatformerPlayer player;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private BoxCollider2D deathCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 EnemyFinishPosition;
    public bool IsAlive;
    public bool StartDeathBehavior;
    private float deathTime = 1;
    private float EnemyDirection = 0;
    private float EnemyTrackPercent = 0;
    private Vector3 EnemyStartPosition = new Vector3(0, 0, 0);
    private Vector3 bump = new Vector3(0, 5f, 0);
    private Vector2 corner1;
    private Vector2 corner2;
    public float health;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsAlive = true;
        StartDeathBehavior = false;
        //speed = 1.5f;
        health = 1;
        EnemyStartPosition = transform.position;
        EnemyDirection = -1;
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 max = boxCollider.bounds.max;
        Vector3 min = boxCollider.bounds.min;

        //Top Left Corner
        corner1 = new Vector2(min.x, max.y);
        //Bottom Left Corner
        corner2 = new Vector2(min.x, min.y + 0.1f);

        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);
        if(hit != null)
        {
            speed *= -1;
        }*/

        if(health <= 0)
        {
            IsAlive = false;
        }

        if (IsAlive)
        {
            Vector2 movement = new Vector2(speed * EnemyDirection, body.linearVelocity.y);
            body.linearVelocity = movement;
            
            EnemyTrackPercent += EnemyDirection * speed * Time.deltaTime;

            float x = (EnemyFinishPosition.x - EnemyStartPosition.x) * EnemyTrackPercent + EnemyStartPosition.x;
            //float y = (EnemyFinishPosition.y - StartPosition.y) * trackPercent + StartPosition.y;

            transform.position = new Vector3(x, transform.position.y, EnemyStartPosition.z);

            if (EnemyDirection == 1 && EnemyTrackPercent > 0.9f || EnemyDirection == -1 && EnemyTrackPercent < 0.1f)
            {
                EnemyDirection *= -1;
            }
        }
        else
        {
            if (StartDeathBehavior == false)
            {
                if (player != null)
                {
                    player.EnemiesKilled += 1;
                }
                StartCoroutine(die());
                StartDeathBehavior = true;
            }
        }
        animator.SetBool("Alive", IsAlive);
    }

    private IEnumerator die()
    {
        body.AddForce(bump, ForceMode2D.Impulse);
        //Debug.Log("Die Coroutine Started");
        boxCollider.enabled = false;
        deathCollider.enabled = false;
        yield return new WaitForSeconds(deathTime);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(corner1, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(corner2, 0.1f);

        if (EnemyFinishPosition != null)
        {
            Gizmos.DrawLine(transform.position, EnemyFinishPosition);

        }

    }
}
