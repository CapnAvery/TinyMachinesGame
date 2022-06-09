using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotGeneral : MonoBehaviour
{
    Robot BotType;
    Sweeper SweeperBot;
    bool EnemySensor = false;
    public LayerMask RobotMask;
    public LayerMask EnemyMask;
    public GameObject Enemy;
    public NavMeshAgent agent;
   

    public void Awake()
    {
        Sweeper sweeper = new Sweeper();
        BotType = sweeper;
        BotType.SetRobotAgentAndActual(this.gameObject, agent);
        
        BotType.OnRobotStart();
    }
    //used for the update command
    public void Update()
    {
        if (BotType.health > 0)
        {
            if (EnemySensor || BotType.InContact)
            {
                BotType.RobotEnemyResponse(Enemy);
            }
            else
            {
                BotType.onRobotUpdate();
            }
        }
        else
        {
            BotType.OnRobotDeath();
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // Casts the ray and get the first game object hit
            if (Physics.Raycast(ray, out hit, 9999f, RobotMask))
            {
                if (hit.transform == this.transform)
                {
                    BotType.onRobotClick();
                }
            }
        }

    }
    //used for enemy detection
    public void FixedUpdate()
    {
        BotType.onRobotFixedUpdate();
        EnemySensor = ScanForEnemies();
    }

    //this function scans for enemies
    public bool ScanForEnemies()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position + this.transform.forward*2, BotType.EnemyScanRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.layer == EnemyMask)
            {
                Enemy = hitCollider.gameObject;
                Debug.Log("Enemy Found!!!!");
                return true;
            }
            
        }
        Enemy = null;
        return false;
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(this.transform.position + this.transform.forward * 2, BotType.EnemyScanRadius);
    }
}
