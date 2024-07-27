using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPCMovement : MonoBehaviour
{
    [SerializeField] Transform destination;

    NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        // Error Message if no object attached, otherwise set destination for object to move towards
        if (navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent component is not attached.");
        }
        else
        {
            SetDestination();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // The Vector is the destination
    private void SetDestination()
    {
        if (destination != null)
        {
            Vector3 targetVector = destination.transform.position;
            navMeshAgent.SetDestination(targetVector);
        }
    }
}
