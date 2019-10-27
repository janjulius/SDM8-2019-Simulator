using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Traffic
{
    public class TrafficParticipant : MonoBehaviour
    {
        private Path path;

        private int currentNode = 0;

        public float Speed = 3;

        public float Acceleration = 0.5f;

        private float currentSpeed;
        private float prevSpeed;

        private bool Drive = true;

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
                        break;
                    }
                }
                Drive = true;
            }
            if (Drive)
            {
                Face(path.Points[currentNode + 1]);
                transform.position = Vector3.MoveTowards(gameObject.transform.position, path.Points[currentNode + 1], currentSpeed * Time.deltaTime);
            }
        }

        private void UpdateSpeed()
        {
            if (Mathf.Approximately(currentSpeed, Speed))
            {
                return;
            }

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
                Destroy(gameObject);
                return true;
            }
            return false;
        }

        private bool InFrontOfStopLight(StopLight s)
        {
            int rot = (int)Mathf.Floor(s.transform.rotation.y);

            if(s.transform.rotation.y == 0 && transform.position.z < s.transform.position.z
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
            Debug.LogError("No proper rotation was found. @ Get2DAxis @ TrafficParticipant");
            return 0;
        }

        public void SetPath(Path path)
            => this.path = path;

        public void Face(GameObject faceTo)
            => Face(faceTo.transform.position);

        public void Face(Vector3 faceTo)
            => transform.LookAt(faceTo);


    }
}
