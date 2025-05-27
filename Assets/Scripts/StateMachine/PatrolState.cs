using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class PatrolState : State
    {
        [SerializeField] private float speed;
        [SerializeField] private Transform route;
        [SerializeField] private bool followXOnly = true;
        private List<Transform> _waypoints = new();
        private Transform _currentWaypoint;
        private int _currentWaypointIndex = 0;

        public override void OnEnterState(FSM_Controller controller)
        {
            base.OnEnterState(controller);
            foreach (Transform point in route)
            {
                _waypoints.Add(point);
            }

            _currentWaypoint = _waypoints[_currentWaypointIndex];
        }

        public override void OnUpdateState()
        {
            Vector3 targetPosition;

            if (followXOnly)
            {
                // Mantiene la posición Y actual del enemigo
                targetPosition = new Vector3(_currentWaypoint.position.x, transform.position.y, transform.position.z);
            }
            else
            {
                // Va directamente al punto (X, Y)
                targetPosition = new Vector3(_currentWaypoint.position.x, _currentWaypoint.position.y,
                    transform.position.z);
            }
            
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
            //Animation Direction
            transform.eulerAngles = transform.position.x >= _currentWaypoint.position.x
                ? new Vector3(0, 180, 0)
                : Vector3.zero;
            if (transform.position == targetPosition)
            {
                CalculateNextWaypoint();
            }
        }

        private void CalculateNextWaypoint()
        {
            _currentWaypointIndex++;
            _currentWaypointIndex = _currentWaypointIndex % _waypoints.Count; //evitar salirse del limite de la lista
            _currentWaypoint = _waypoints[_currentWaypointIndex];
        }

        public override void OnExitState()
        {
            _waypoints.Clear();
        }
    }
}