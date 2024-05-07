using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] Transform _destination;

    [SerializeField] Transform[] PathWaypoints;
    private int _pathIndex;

    NavMeshAgent _navMeshAgent;

    private Transform _target = null;

    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
    }


    private void SetDestination()   
    {
        if (_navMeshAgent.remainingDistance < 0.5f)
        {
            _pathIndex++;

            if (_pathIndex >= PathWaypoints.Length)
            {
                _target = _destination;
            }
            else
            {
                _target = PathWaypoints[_pathIndex];
            }
        }

        _navMeshAgent.SetDestination(_target.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (_target)
        {
            Debug.DrawLine(transform.position, _target.position, Color.green);
        }

        if (_navMeshAgent == null)
        {
            Debug.LogError(" the nav mesh agent component is not attached to the" + gameObject.name);

        }
        else
            SetDestination();

    }
}
