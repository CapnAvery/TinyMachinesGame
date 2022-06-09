using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Robot
{
    /*robots can
     Patrol a certain area
    Go to a certain place
    What happens when it sees an enemy
    what happens when it attacks
     */
    public int health;
    public int EnemyScanRadius;
    public bool InContact;
    public GameObject RobotActual;
    public Vector3 HomeBasePosition;
    public NavMeshAgent agent;
    public void SetRobotAgentAndActual(GameObject Go, NavMeshAgent NMA)
    {
        agent = NMA;
        RobotActual = Go;
    }
    public abstract void OnRobotStart();
    public abstract void RobotEnemyResponse(GameObject Enemy);
    public abstract void OnRobotDeath();
    public abstract void onRobotUpdate();
    public abstract void onRobotClick();
    public abstract void onRobotFixedUpdate();
    

}
