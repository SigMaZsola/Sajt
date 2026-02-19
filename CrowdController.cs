using System.Linq;
using System.Numerics;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Collections.Generic;
using System;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

using Quaternion = UnityEngine.Quaternion;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using TMPro;

namespace Asino
{
    public class CrowdController : MonoBehaviour
    {
        public GameObject ragdollPrefab;
        public float speed = 5f;
        public bool joined = true;
        public int gap = 3;
        public Transform directionRing;
        [SerializeField] CurvedAimLine curveLine;
        [SerializeField] TMP_Text bubiText;
        [SerializeField] ParticleSystem csihiPuhi;

        Transform crowd;
        Vector3 movement = Vector3.zero;
        public healthbar healthBar;

        //Local multi
            private Vector2 moveInput;
            public PlayerInput playerInput;

        //Local multi

        public Grab grabber;


        Dictionary<GameObject, UnityEngine.Vector3> originalPositions = new Dictionary<GameObject, UnityEngine.Vector3>();

        public ActionTimer actionTimer;

        public GameObject main;

        public int playerNumber = 1;

        public Transform mainContainer;

        char fomration = 'p';

        GameObject[] wayPoints = new GameObject[30];
        GameObject[] crowdMember = new GameObject[30];

        public GameObject[] combined;
        public UnityEngine.Vector3[] points = new UnityEngine.Vector3[30];

        public Bubi textBubi;

        public Image ring;
        public Image cricle;

        public SettingsScript settings;


        private float nextInputTime = 0f;
        public float inputDelay = 0.2f;

        [SerializeField] private AudioClip audioClip;

        public void OnEscape(InputValue v)
        {
            if (v.isPressed)
            {
                if (playerInput.playerIndex != 0) return;
                SettingsScript.Instance.OpenSettings();
            }
        }

    public void OnNavigate(InputValue value)
    {

        if (playerInput.playerIndex != 0) return;
        if (Time.unscaledTime < nextInputTime) return;

        Vector2 dir = value.Get<Vector2>();

        if (dir.y > 0.5f)
        {
            SettingsScript.Instance.rounds++;
            nextInputTime = Time.unscaledTime + inputDelay;
        }
        else if (dir.y < -0.5f)
        {
            SettingsScript.Instance.rounds--;
            nextInputTime = Time.unscaledTime + inputDelay;
        }
    }



        void Awake()
        {
            playerNumber = playerInput.playerIndex + 1;

            if (healthBar == null){
            var bars = FindObjectsByType<healthbar>(FindObjectsSortMode.None);

            foreach (var bar in bars)
            {
                
                if (bar != null &&bar.playerIndex-1 == playerInput.playerIndex)
                {
                    healthBar = bar;
                    break;
                }
            }
            LoadColors();
            if (playerNumber ==1)
            {
                transform.position -= new Vector3(-20f, 9,0);
            }
            else
            {
                transform.position -= new Vector3(20f,9,0);
            }
    }

}
    private void LoadColors()
        {
            Color teamColor = (playerNumber == 2)
            ? new Color32(120, 200, 255, 255)
            : new Color32(230, 60, 60, 255);
            cricle.color = teamColor;
            ring.color = teamColor;
            bubiText.color = teamColor;

        }

        void Start()
        {
            crowd = GetComponent<Transform>();
            combined = fillContainer(mainContainer).ToArray();
        }

        public Vector3 GetMovement()
        {
            return movement;
        }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnHit(InputValue v)
    {
        if (v.isPressed) crowdHit();
    }

    public void OnGrab(InputValue v)
    {
        if (v.isPressed) crowdGrab();
    }

    public void OnThrow(InputValue v)
    {
        if (v.isPressed) Throw();
    }

    public void OnButtonO(InputValue v)
    {
        if (v.isPressed)
            {
                fomration = 'O';
                textBubi.AppearBubi("CIRCLE!!!");
            }
    }

    public void OnButtonRectangle(InputValue v)
    {
            if (v.isPressed)
            {
                fomration = 'p';
                textBubi.AppearBubi("RECTANGLE!!!");
            }
    }

    public void OnButtonArrow(InputValue v)
    {
        if (v.isPressed)
            {
                textBubi.AppearBubi("ARROW!!!");
                fomration = '>';
            }
    }



        void Update()
        {
            UpdateHealthbar();
            if (!actionTimer.isRunning)
            {
                curveLine.canEmmit = false;
            }

        }


        public void Throw()
        {
            if(grabber.grabbedObject == null)
            {
                return;
            }
            grabber.ThrowCsacsi();
        }

        void crowdGrab()
        {
            if (actionTimer == null)
                return;

            actionTimer.StopTimer();
            actionTimer.duration = 5f;
            textBubi.AppearBubi("GRAB!!!");
            curveLine.canEmmit = true;
            foreach (GameObject member in crowdMember)
            {
                if (member == null) continue;

                var script = member.GetComponentInChildren<PlayerController>();
                if (script != null && fomration == 'p')
                    script.grab();
                    
            }
            
            grabber.grabCsacsi();

            actionTimer.StartTimer();
        }


        void crowdHit()
        {
            if (actionTimer == null || actionTimer.isRunning)
                return;
            actionTimer.duration = 3f;
            textBubi.AppearBubi("ATTACK!!!");
            csihiPuhi.Play();
            foreach (GameObject member in crowdMember)
            {
                if (member == null)
                    continue;
                    
                PlayerController script = member.GetComponentInChildren<PlayerController>();
                if (script != null)
                {
                    script.hit();
                }
            }
            SoundFXManager.instance.PlaySoundFXClip(audioClip,transform, 10f);
            startActionTimer();
        }
        void startActionTimer()
        {
        if (!actionTimer.isRunning)
            actionTimer.StartTimer();

    }


        private void FixedUpdate()
        {
            positionContainer(mainContainer, fomration);
            Move();

        }

        private void UpdateHealthbar()
        {
            double crowdHealth = 0;
            
            foreach (GameObject member in crowdMember)
            {
                PlayerController script = member.GetComponentInChildren<PlayerController>();
                if (script != null)
                {
                    crowdHealth += script.healthPoints;
                }
            }

        
            
            if(healthBar == null)return;
            healthBar.UpdateHealthbar(30*3.3f, (float)crowdHealth);
        }

        public void Move()
        {
            movement = new Vector3(moveInput.x, 0, moveInput.y);

            if (movement.sqrMagnitude > 0.01f)
            {
                movement.Normalize();

                crowd.transform.position += movement * speed * Time.deltaTime;

                Quaternion targetRotation =
                    Quaternion.LookRotation(movement, Vector3.up);

                main.transform.rotation =
                    Quaternion.Slerp(main.transform.rotation, targetRotation, Time.deltaTime * 10f);
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
                    float xOffset = (rows - 1) * gap * 0.5f;
                    float zOffset = (cols - 1) * gap * 0.5f;

                    Vector3 position = new Vector3(
                        row * gap - xOffset,
                        0f,
                        col * gap - zOffset
                    );

                    GameObject obj = new GameObject($"Cell_{row}_{col}");
                    wayPoints[index] = obj;
                    obj.transform.parent = container;
                    obj.transform.localPosition = position;
                    result[index] = obj;

                    GameObject ragdoll = Instantiate(ragdollPrefab);
                    crowdMember[index] = ragdoll;
                    ragdoll.name = $"Ragdoll_{row}_{col}";
                    ragdoll.transform.position = obj.transform.position;

                    PlayerController script = ragdoll.GetComponentInChildren<PlayerController>();
                    if (script != null)
                    {
                        script.setTargetPosition(obj.transform.position);
                        script.loadTexture(playerNumber);
                        script.crowd = crowd.GetComponent<CrowdController>();
                        script.actionTimer = actionTimer;
                    }

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
                            PlayerController script = ragdoll.GetComponentInChildren<PlayerController>();
                            if (script != null)
                            {
                                script.setTargetPosition(obj.transform.position);
                                script.loadTexture(playerNumber);
                            }
                                
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
                                    row * -gap + 10
                                );

                                GameObject obj = wayPoints[index];

                                obj.transform.parent = container;
                                obj.transform.localPosition = position;
                                GameObject ragdoll = crowdMember[index];
                                PlayerController script = ragdoll.GetComponentInChildren<PlayerController>();
                                if (script != null)
                                {
                                    script.setTargetPosition(obj.transform.position);
                                    script.loadTexture(playerNumber);
                                }

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
                                float xOffset = (rows - 1) * gap * 0.5f;
                                float zOffset = (cols - 1) * gap * 0.5f;

                                Vector3 position = new Vector3(
                                    row * gap - xOffset,
                                    0f,
                                    col * gap - zOffset
                                );


                                GameObject obj = wayPoints[index];

                                obj.transform.parent = container;
                                obj.transform.localPosition = position;
                                GameObject ragdoll = crowdMember[index];
                                PlayerController script = ragdoll.GetComponentInChildren<PlayerController>();
                                if (script != null)
                                {
                                    script.setTargetPosition(obj.transform.position);
                                    script.loadTexture(playerNumber);
                                }

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




