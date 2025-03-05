using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class BossBehavior : MonoBehaviour
{
    [SerializeField]
    private Vector3 BossFinishPosition = Vector3.zero;
    [SerializeField]
    private float BossSpeed = 0.2f;
    [SerializeField] private PlatformerPlayer player;
    [SerializeField] private BoxCollider2D BossBoxCollider;
    [SerializeField] private BoxCollider2D BossDeathCollider;
    [SerializeField] private Rigidbody2D BossRB;
    [SerializeField] private Animator animator;
    public GameObject Boss;
    private Vector3 BossStartPosition;
    private Vector3 bump = new Vector3(0, 5f, 0);
    [SerializeField] private float BossTrackPercent = 0;
    private float JumpForce = 10f;
    private float BossDeathTime = 2;
    private int BossDirection = 1;
    private int directionChange = 100;
    private int JumpChance = 1001;
    public int health = 3;
    [SerializeField] private bool BossIsAlive;
    private bool StartBossDeathBehavior = false;

    void Start()
    {
        BossStartPosition = transform.position;
        BossIsAlive = true;
        Boss.SetActive(false);
    }
    void Update()
    {

        Vector3 max = BossBoxCollider.bounds.max;
        Vector3 min = BossBoxCollider.bounds.min;

        Vector2 corner1 = new Vector2(max.x, min.y - 0.1f);
        Vector2 corner2 = new Vector2(min.x, min.y - 0.2f);

        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);
        bool grounded = false;

        if (hit != null)
        {
            grounded = true;
        }

        BossTrackPercent += BossDirection * BossSpeed * Time.deltaTime;

        float BossX = (BossFinishPosition.x - BossStartPosition.x) * BossTrackPercent + BossStartPosition.x;
        //float BossY = (BossFinishPosition.y - BossStartPosition.y) * BossTrackPercent + BossStartPosition.y;

        
        if (health <= 0)
        {
            BossIsAlive = false;
        }

        if (BossIsAlive)
        {
            transform.position = new Vector3(BossX, transform.position.y, BossStartPosition.z);

            if (Random.Range(1, directionChange) == 1)
            {
                BossDirection *= -1;
            }

            if (Random.Range(1, JumpChance) == 1)
            {
                if (grounded)
                {
                    BossRB.AddForce(new Vector3(0, JumpForce, 0), ForceMode2D.Impulse);
                }
            }

            if (BossDirection == 1 && BossTrackPercent > 0.9f || BossDirection == -1 && BossTrackPercent < 0.1f)
            {
                BossDirection *= -1;
            }

            if(health == 3)
            {
                BossSpeed = 0.1f;
                directionChange = 301;
                JumpChance = 301;
            }
            else if(health == 2)
            {
                BossSpeed = 0.15f;
                directionChange = 201;
                JumpChance = 201;
            }
            else if (health == 1)
            {
                BossSpeed = 0.2f;
                directionChange = 101;
                JumpChance = 101;
            }
        }
        else
        {
            if (StartBossDeathBehavior == false)
            {
                if (player != null)
                {
                    player.EnemiesKilled += 1;
                }
                StartCoroutine(die());
                StartBossDeathBehavior = true;
            }
        }

        if (animator != null)
        {
            animator.SetInteger("Health", health);
        }


        //animator.SetBool("Alive", BossIsAlive);
    }

    private IEnumerator die()
    {
        BossRB.AddForce(bump, ForceMode2D.Impulse);
        //Debug.Log("Die Coroutine Started");
        BossBoxCollider.enabled = false;
        BossDeathCollider.enabled = false;
        yield return new WaitForSeconds(BossDeathTime);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (BossFinishPosition != null)
        {
            Gizmos.DrawLine(transform.position, BossFinishPosition);

        }
    }
}
