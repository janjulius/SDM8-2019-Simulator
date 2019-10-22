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

        public int Speed = 3;

        private void Update()
        {
            if(currentNode == (path.Points.Length - 1))
                Destroy(this);
            
            if (Vector3.Distance(gameObject.transform.position, path.Points[currentNode + 1]) < 1)
                currentNode++;

            foreach(StopLight s in path.StopForStopLights)
            {
                if(Vector3.Distance(gameObject.transform.position, s.gameObject.transform.position) < 1
                    && InFrontOfStopLight(s))
                {
                    if (s.Status == 0)
                        return;
                }
            }
            Face(path.Points[currentNode + 1]);
            transform.position = Vector3.MoveTowards(gameObject.transform.position, path.Points[currentNode + 1], Speed * Time.deltaTime);
        }

        private bool InFrontOfStopLight(StopLight s)
        {
            var rot = s.transform.rotation.y;
            if (rot != 0 || rot != 90 || rot != 180 || rot != 270)
                Debug.LogError("Stoplight does not have the correct rotation");

            if(s.transform.rotation.y == 0 && transform.position.z < s.transform.position.z
                || s.transform.rotation.y == 90 && transform.position.x < s.transform.position.x
                || s.transform.rotation.y == 180 && transform.position.z > s.transform.position.z
                || s.transform.rotation.y == 270 && transform.position.x > s.transform.position.x)
            {
                return true;
            }
            return false;
        }

        public void SetPath(Path path)
            => this.path = path;

        public void Face(GameObject faceTo)
            => Face(faceTo.transform.position);

        public void Face(Vector3 faceTo)
            => transform.LookAt(faceTo);


    }
}
