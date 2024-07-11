using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EmeyMove : MonoBehaviour
{
    Transform player;
    NavMeshAgent nav;
	
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(nav.enabled)
        {
            nav.SetDestination(player.position);
        }
		
	}
}
