using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public List list;
    public Dictionary<string, PlanetInformation> listPlanetInformations;
    public float scale = 1;
    private Vector2 mousePositionUp;
    private Vector2 mousePositionDown;
    private Vector3 PrevMousePos;
    private float distance;
    private float angle;
    private float yDistance;
    private float xDistance;
    public bool firstCheck;
    public float speedCamera = 0.003f;
    public float zoomCamera = 1f;
    public bool checkShowInf = false;

    void Start()
    {
        listPlanetInformations = new Dictionary<string, PlanetInformation>();
        foreach (var planetInformation in list.PlanetsInformation)
        {
            listPlanetInformations.Add(planetInformation.tag, planetInformation);
            GameObject.Find(planetInformation.tag).transform.rotation =
                Quaternion.Euler(0, 0, listPlanetInformations[planetInformation.tag].rotary);
        }
    }

    private void Update()
    {
        ZoomCamera();
        MoveCamera();
    }

    public void MoveCamera()
    {
        if (!Input.GetButton("Fire2"))
        {
            Debug.Log("aaa");
            firstCheck = true;
        }

        if (Input.GetButton("Fire2"))
        {
            if (firstCheck)
            {
                firstCheck = false;
                mousePositionUp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            mousePositionDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            distance = Mathf.Sqrt(Mathf.Pow(mousePositionUp.x - mousePositionDown.x, 2) +
                                  Mathf.Pow(mousePositionUp.y - mousePositionDown.y, 2)) * speedCamera;
            yDistance = (Mathf.Max(mousePositionUp.y, mousePositionDown.y) -
                         Mathf.Min(mousePositionUp.y, mousePositionDown.y)) * speedCamera;

            angle = Mathf.Sin(yDistance / distance);
            if (angle > 0.8f)
            {
                angle = Mathf.Asin(yDistance / distance);
                // Debug.Log("angle change: " + angle);
            }

            if ((PrevMousePos != Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                if (mousePositionUp.y > mousePositionDown.y && mousePositionUp.x > mousePositionDown.x)
                {
                    Camera.main.transform.position +=
                        new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0);
                }
                else if (mousePositionUp.y > mousePositionDown.y && mousePositionUp.x < mousePositionDown.x)
                {
                    Camera.main.transform.position +=
                        new Vector3(-Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0);
                }
                else if (mousePositionUp.y < mousePositionDown.y && mousePositionUp.x > mousePositionDown.x)
                {
                    Camera.main.transform.position +=
                        new Vector3(Mathf.Cos(angle) * distance, -Mathf.Sin(angle) * distance, 0);
                }
                else if (mousePositionUp.y < mousePositionDown.y && mousePositionUp.x < mousePositionDown.x)
                {
                    Camera.main.transform.position +=
                        new Vector3(-Mathf.Cos(angle) * distance, -Mathf.Sin(angle) * distance, 0);
                }

                PrevMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                mousePositionUp = mousePositionDown;
            }
        }
    }

    public void ZoomCamera()
    {
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            if (Input.mouseScrollDelta.y == -1)
            {
                Camera.main.orthographicSize += zoomCamera;
            }
            else if (Input.mouseScrollDelta.y == 1)
            {
                Camera.main.orthographicSize -= zoomCamera;
            }
        }
    }
}