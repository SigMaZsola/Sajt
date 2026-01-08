using System.Linq;
using System.Numerics;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Collections.Generic;

namespace Asino
{
    public class NewBehaviourScript : MonoBehaviour
    {
        public float speed = 5f;
        public bool joined = true;
        public int gap = 3;
        Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();

        public Transform leftContainer;
        public Transform rightContainer;

        public GameObject[] combined;
        public Vector3[] points = new Vector3[30];

        int pointIndex = 0;

        void Start()
        {
            combined = fillContainer(leftContainer)
                .Concat(fillContainer(rightContainer))
                .ToArray();

            foreach (var obj in combined)
            {
                originalPositions[obj] = obj.transform.position;
            }
        }


        void Update()
        {
            if (joined)
            {
                Move();
            }

            moveOutOfTheWay();
        }   

        public void Move()
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            if (Input.GetKey(KeyCode.A)) rb.AddForce(Vector3.left * speed);
            if (Input.GetKey(KeyCode.D)) rb.AddForce(Vector3.right * speed);
            if (Input.GetKey(KeyCode.W)) rb.AddForce(Vector3.up * speed);
            if (Input.GetKey(KeyCode.S)) rb.AddForce(Vector3.down * speed);
        }

        private GameObject[] fillContainer(Transform container)
        {
            GameObject[] result = new GameObject[15];
            int index = 0;

            for (int col = 0; col < 3; col++)
            {
                for (int row = 0; row < 5; row++)
                {
                    Vector3 position = new Vector3(row * gap, 0, col * gap);

                    GameObject obj = new GameObject($"Cell_{row}_{col}");
                    obj.transform.parent = container;
                    obj.transform.localPosition = position;

                    result[index] = obj;

                    points[pointIndex] = obj.transform.position;
                    pointIndex++;

                    index++;
                }
            }

            return result; 
        }
        public void moveOutOfTheWay() {
            foreach (var obj in combined)
            {
                if (obj) {
                }
            }

        }
    }
}
