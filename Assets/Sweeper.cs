using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sweeper : Robot
{
    bool destinationSet = false;
    public override void onRobotClick()
    {
        
    }

    public override void OnRobotDeath()
    {
        health = 0;
        GameObject.Destroy(RobotActual, 5f);
    }

    public override void onRobotFixedUpdate()
    {
        //nothing
    }

    public override void OnRobotStart()
    {
        EnemyScanRadius = 5;
        health = 10;
        destinationSet = false;
        HomeBasePosition = new Vector3(0,0,0);
    }

    public override void onRobotUpdate()
    {
        if (!destinationSet || agent.remainingDistance < 1)
        {
            float RandomXPosition = Random.Range(-200f, 200f);
            float RandomYPosition = Random.Range(-200f, 200f);
            destinationSet = true;
            Vector3 destination = new Vector3(HomeBasePosition.x + RandomXPosition, HomeBasePosition.y, HomeBasePosition.z + RandomYPosition);
            agent.destination = destination;
        }
       
    }
    //this function allows the agent to run in circles using its destination feature
    public override void RobotEnemyResponse(GameObject Enemy)
    {
        if (Enemy!=null)
        {
            if (Vector3.Distance(Enemy.transform.position, RobotActual.transform.position) < EnemyScanRadius)
            {
                agent.destination = -Enemy.transform.position*50;
                InContact = true;
            }
            else
            {
                InContact = false;
            }
        }
        
    }
    
}
