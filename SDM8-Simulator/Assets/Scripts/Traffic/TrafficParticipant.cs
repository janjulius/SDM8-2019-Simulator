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

            Face(path.Points[currentNode + 1]);
            transform.position = Vector3.MoveTowards(gameObject.transform.position, path.Points[currentNode + 1], Speed * Time.deltaTime);
        }

        public void SetPath(Path path)
            => this.path = path;

        public void Face(GameObject faceTo)
            => Face(faceTo.transform.position);

        public void Face(Vector3 faceTo)
            => transform.LookAt(faceTo);
    }
}
