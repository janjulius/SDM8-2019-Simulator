using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Traffic
{
    public class TrafficParticipant : MonoBehaviour
    {
        private Path path;

        private int currentNode = 0;
        private int tailGatingDistance = 25;

        protected float origSpeed { get; private set; } = 3;

        public float Speed = 3;

        private float Acceleration;

        private float currentSpeed;
        private float prevSpeed;

        private bool Drive = true;

        protected string status = "Lounging";

        private void Awake()
        {
            origSpeed = Speed;
            Acceleration = origSpeed * 2;
        }

        private void Update()
        {
            CheckForDeath();

            if (Vector3.Distance(gameObject.transform.position, path.Points[currentNode + 1]) < 1)
                currentNode++;

            if (CheckForDeath())
                return;

            UpdateSpeed();

            foreach (StopLight s in path.StopForStopLights)
            {
                if (GetDistance(s) < 5 && InFrontOfStopLight(s))
                {
                    if (s.Status == 0)
                    {
                        Drive = false;
                        status = $"Waiting for stoplight {s.name}";
                        break;
                    }
                }
                Drive = true;
            }
            if (Drive)
            {
                status = $"Driving to point: {currentNode + 1} on path: {path.name}";
                Face(path.Points[currentNode + 1]);
                transform.position = Vector3.MoveTowards(gameObject.transform.position, path.Points[currentNode + 1], currentSpeed * Time.deltaTime);
            }
            TrafficUpdate();
        }

        public virtual void TrafficUpdate()
        {
            //int layerMask = 1 << 8;
            //layerMask = ~layerMask;
            //print(layerMask);
            int layerMask = LayerMask.GetMask("Vehicles", "Stop");
            RaycastHit hit;
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(transform.position, fwd, out hit, tailGatingDistance, layerMask))
            {

                status = $"Somethign on raycast: {hit.transform.name}";
                Speed = 0;
            }
            else
            {
                Speed = origSpeed;
            }

            Debug.DrawRay(transform.position, fwd * tailGatingDistance, Color.green);
        }

        private void UpdateSpeed()
        {
            if (Mathf.Approximately(currentSpeed, Speed))
                return;

            if (Speed < origSpeed)
                currentSpeed = currentSpeed - Acceleration * Time.fixedDeltaTime;
            else
                currentSpeed = currentSpeed + Acceleration * Time.fixedDeltaTime;
            if (prevSpeed > Speed)
            {
                currentSpeed = Mathf.Max(currentSpeed, Speed);
            }
            else if (prevSpeed < Speed)
            {
                currentSpeed = Mathf.Min(currentSpeed, Speed);
            }

            currentSpeed = Mathf.Max(currentSpeed, 0);
        }

        private bool CheckForDeath()
        {
            if (currentNode == (path.Points.Length - 1))
            {
                Camera.main.GetComponent<SdmManager>().trafficParticipants.Remove(this);
                Destroy(gameObject);
                return true;
            }
            return false;
        }

        private bool InFrontOfStopLight(StopLight s)
        {
            int rot = (int)Mathf.Floor(s.transform.rotation.y);

            if (s.transform.rotation.y == 0 && transform.position.z < s.transform.position.z
                || s.transform.rotation.y == 90 && transform.position.x < s.transform.position.x
                || s.transform.rotation.y == 180 && transform.position.z > s.transform.position.z
                || s.transform.rotation.y == 270 && transform.position.x > s.transform.position.x)
            {
                return true;
            }
            return false;
        }

        private float GetDistance(StopLight s)
        {
            var rot = s.transform.rotation.y;
            if (rot == 0 || rot == 180)
                return Math.Abs(transform.position.z - s.transform.position.z);
            if (rot == 90 || rot == 270)
                return Math.Abs(transform.position.x - s.transform.position.x);
           // Debug.LogError("No proper rotation was found. @ Get2DAxis @ TrafficParticipant " + s.gameObject.transform.position);
            return 0;
        }

        public void SetPath(Path path)
            => this.path = path;

        public void Face(GameObject faceTo)
            => Face(faceTo.transform.position);

        public void Face(Vector3 faceTo)
            => transform.LookAt(faceTo);

        void OnDrawGizmos()
        {
            Handles.Label(transform.position, $"{status}");
        }

    }
}
