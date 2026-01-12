using System.Linq;
using System.Numerics;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Collections.Generic;
using System;
using Vector3 = UnityEngine.Vector3;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Asino
{
    public class CrowdController : MonoBehaviour
    {
        public GameObject ragdollPrefab;
        public float speed = 5f;
        public bool joined = true;
        public int gap = 3;
        Dictionary<GameObject, UnityEngine.Vector3> originalPositions = new Dictionary<GameObject, UnityEngine.Vector3>();

        public Transform mainContainer;

        char fomration = 'p';

        GameObject[] wayPoints = new GameObject[30];
        GameObject[] crowdMember = new GameObject[30];

        public GameObject[] combined;
        public UnityEngine.Vector3[] points = new UnityEngine.Vector3[30];


        void Start()
        {
            combined = fillContainer(mainContainer).ToArray();
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.O))
            {
                fomration = 'O';
            }
            if (Input.GetKey(KeyCode.P))
            {
                fomration = 'p';
            }
            if (Input.GetKey(KeyCode.I))
            {
                fomration = '>';
            }
        }

        private void FixedUpdate()
        {
            positionContainer(mainContainer, fomration);
            Move();

        }

        public void Move()
        {
            Rigidbody crowd = GetComponent<Rigidbody>();
            if (Input.GetKey(KeyCode.W))
            {
                crowd.AddForce(crowd.transform.forward * speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                crowd.AddForce(-crowd.transform.forward * speed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                crowd.AddForce(-crowd.transform.right * speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                crowd.AddForce( crowd.transform.right * speed);
            }
        }

        private GameObject[] fillContainer(Transform container)
        {
            int count = 30;
            GameObject[] result = new GameObject[count];
            int index = 0;

            int cols = 6;
            int rows = 5;

            for (int col = 0; col < cols; col++)
            {
                for (int row = 0; row < rows; row++)
                {
                    Vector3 position = new Vector3(row * gap, 0, col * gap);
                    GameObject obj = new GameObject($"Cell_{row}_{col}");
                    wayPoints[index] = obj;
                    obj.transform.parent = container;
                    obj.transform.localPosition = position;
                    result[index] = obj;

                    GameObject ragdoll = Instantiate(ragdollPrefab);
                    crowdMember[index] = ragdoll;
                    ragdoll.name = $"Ragdoll_{row}_{col}";
                    ragdoll.transform.position = obj.transform.position;

                    PLayerController script = ragdoll.GetComponentInChildren<PLayerController>();
                    if (script != null)
                        script.setTargetPosition(obj.transform.position);

                    index++;
                }
            }
            return result;
        }

        private void positionContainer(Transform container, char formation)
        {
            int count = 30;

            int index = 0;
            int pointIndex = 0;

            int cols = 6;
            int rows = 5;

            switch (formation)
            {

                case 'O':
                    {
                        float radius = gap * 4f;

                        for (int i = 0; i < count; i++)
                        {
                            float angle = (2f * Mathf.PI / count) * i;

                            Vector3 position = new Vector3(
                                Mathf.Cos(angle) * radius,
                                0f,
                                Mathf.Sin(angle) * radius
                            );

                            GameObject obj = wayPoints[i];
                            obj.transform.parent = container;
                            obj.transform.localPosition = position;
                            GameObject ragdoll = crowdMember[i];
                            PLayerController script = ragdoll.GetComponentInChildren<PLayerController>();
                            if (script != null)
                                script.setTargetPosition(obj.transform.position);
                                
                            points[pointIndex++] = obj.transform.position;
                            index++;
                        }
                        break;
                    }

                case '>':
                    {

                        int row = 0;

                        while (index < count)
                        {
                            int elementsInRow = row + 1;

                            if (index + elementsInRow > count)
                                elementsInRow = count - index;


                            float rowWidth = (elementsInRow - 1) * gap;
                            float startX = -rowWidth / 2f;

                            for (int col = 0; col < elementsInRow; col++)
                            {
                                if (index >= count)
                                    break;

                                Vector3 position = new Vector3(
                                    startX + col * gap,
                                    0f,
                                    row * -gap
                                );

                                GameObject obj = wayPoints[index];

                                obj.transform.parent = container;
                                obj.transform.localPosition = position;
                                GameObject ragdoll = crowdMember[index];
                                PLayerController script = ragdoll.GetComponentInChildren<PLayerController>();
                                if (script != null)
                                    script.setTargetPosition(obj.transform.position);

                                points[pointIndex++] = obj.transform.position;
                                index++;
                            }

                            row++;
                        }
                    }
                    break;

                default:
                    {

                        for (int col = 0; col < cols; col++)
                        {
                            for (int row = 0; row < rows; row++)
                            {
                                Vector3 position = new Vector3(row * gap, 0, col * gap);

                                GameObject obj = wayPoints[index];

                                obj.transform.parent = container;
                                obj.transform.localPosition = position;
                                GameObject ragdoll = crowdMember[index];
                                PLayerController script = ragdoll.GetComponentInChildren<PLayerController>();
                                if (script != null)
                                    script.setTargetPosition(obj.transform.position);
                                    

                                points[pointIndex++] = obj.transform.position;
                                index++;
                            }
                        }
                        break;
                    }
            }
        }
    }
}




