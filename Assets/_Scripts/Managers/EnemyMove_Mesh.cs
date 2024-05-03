using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove_Mesh : MonoBehaviour
{
    [SerializeField] Transform _destination;

    NavMeshAgent _navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

    }


    private void SetDestination()   
    {
        if(_destination != null) 
        
        {
            Vector2 targetVector = _destination.transform.position;
            _navMeshAgent.SetDestination(targetVector);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_navMeshAgent == null)
        {
            Debug.LogError(" the nav mesh agent component is not attachhed to the" + gameObject.name);

        }
        else
            SetDestination();

    }
}
