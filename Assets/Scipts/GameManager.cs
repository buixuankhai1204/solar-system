using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public ShowInformation showInformation;
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
    private float x, y, z;
    public bool changeValueView = true;
    public Toggle changeView;
    float DegY, DegX;


    void Start()
    {
        showInformation = GameObject.FindWithTag("UiManager").GetComponent<ShowInformation>();
        Camera.main.fieldOfView = 15f;
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
        if (Input.GetButton("Fire2") && Input.mouseScrollDelta != Vector2.zero)
        {
            ChangeSpeedCamera();
            StartCoroutine(showInformation.ShowSpeedCamera(speedCamera));

        }
        else
        {
            ZoomCamera();
        }

        MoveCamera();
        changeView.onValueChanged.AddListener((delegate { ChangeView3D(changeView.isOn); }));
    }

    public void MoveCamera()
    {
        if (!changeValueView)
        {
            FreeCamera2D();
        }
        else
        {
            FreeCamera3D();
        }
    }

    public void ZoomCamera()
    {
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            if (Input.mouseScrollDelta.y == -1)
            {
                Camera.main.fieldOfView += zoomCamera;
            }
            else if (Input.mouseScrollDelta.y == 1)
            {
                Camera.main.fieldOfView -= zoomCamera;
            }
        }
    }

    public void ChangeSpeedCamera()
    {
        if (Input.mouseScrollDelta.y == -1)
        {
            if (speedCamera - Time.deltaTime > 0)
            {
                speedCamera -= Time.deltaTime;
            }
        }
        else if (Input.mouseScrollDelta.y == 1)
        {
            if (speedCamera + Time.deltaTime < 1)
            {
                speedCamera += Time.deltaTime;
            }
        }

    }

    public void ChangeView3D(bool value)
    {
        changeValueView = value;
    }

    public void FreeCamera3D()
    {
        PositionCamera3D();
        RotationCamera3D();
    }

    public void FreeCamera2D()
    {
        if (!Input.GetButton("Fire2"))
        {
            firstCheck = true;
        }

        if (Input.GetButton("Fire2"))
        {
            if (firstCheck)
            {
                firstCheck = false;
                mousePositionUp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, Camera.main.nearClipPlane));
            }

            mousePositionDown = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.nearClipPlane));

            distance = Mathf.Sqrt(Mathf.Pow(mousePositionUp.x - mousePositionDown.x, 2) +
                                  Mathf.Pow(mousePositionUp.y - mousePositionDown.y, 2)) * speedCamera;
            yDistance = (Mathf.Max(mousePositionUp.y, mousePositionDown.y) -
                         Mathf.Min(mousePositionUp.y, mousePositionDown.y)) * speedCamera;

            angle = Mathf.Sin(yDistance / distance);
            if (angle > 0.8f)
            {
                angle = Mathf.Asin(yDistance / distance);
            }

            if ((PrevMousePos != Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, Camera.main.nearClipPlane))))
            {
                if (mousePositionUp.y > mousePositionDown.y)
                {
                    y = Mathf.Sin(angle) * distance;
                }
                else
                {
                    y = -Mathf.Sin(angle) * distance;
                }

                if (mousePositionUp.x > mousePositionDown.x)
                {
                    x = Mathf.Cos(angle) * distance;
                }
                else
                {
                    x = -Mathf.Cos(angle) * distance;
                }
                
                Camera.main.transform.position +=
                    new Vector3(x, y);
                PrevMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y,
                    Camera.main.nearClipPlane));
            }
            else
            {
                mousePositionUp = mousePositionDown;
            }
        }
    }

    public void PositionCamera3D()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.fieldOfView -= zoomCamera * Time.deltaTime * 10 * speedCamera;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Camera.main.fieldOfView += zoomCamera * Time.deltaTime * 10 * speedCamera;
        }

        if (Input.GetKey(KeyCode.A))
        {
            x = -Mathf.Cos(Time.deltaTime) * 2 * speedCamera;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            x = Mathf.Cos(Time.deltaTime) * 2 * speedCamera;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            y = -Mathf.Sin(Time.deltaTime) * 20 * speedCamera;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            y = Mathf.Sin(Time.deltaTime) * 20 * speedCamera;
        }

        Camera.main.transform.position +=
            new Vector3(x, y, 0);
        x = 0;
        y = 0;
    }

    public void RotationCamera3D()
    {
        if (!Input.GetButton("Fire2"))
        {
            firstCheck = true;
        }
        else
        {
            mousePositionUp = mousePositionDown;
            if (firstCheck)
            {
                firstCheck = false;
                mousePositionUp = Camera.main.ViewportToScreenPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y,
                    Camera.main.nearClipPlane));
            }

            mousePositionDown = Camera.main.ViewportToScreenPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.nearClipPlane));

            distance = Mathf.Sqrt(Mathf.Pow(mousePositionUp.x - mousePositionDown.x, 2) +
                                  Mathf.Pow(mousePositionUp.y - mousePositionDown.y, 2)) * speedCamera;
            yDistance = (Mathf.Max(mousePositionUp.y, mousePositionDown.y) -
                         Mathf.Min(mousePositionUp.y, mousePositionDown.y)) * speedCamera;

            angle = Mathf.Sin(yDistance / distance);
            if (angle > 0.8f)
            {
                Debug.Log("angle: " + angle);
                angle = Mathf.Asin(yDistance / distance);
            }
            if ((PrevMousePos - Camera.main.ViewportToScreenPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, Camera.main.nearClipPlane))).sqrMagnitude > 0 &&
                (mousePositionDown - mousePositionUp).sqrMagnitude > 2f)

            {
                if (mousePositionUp.y > mousePositionDown.y)
                {
                    DegX = Mathf.Sin(angle) * speedCamera * Mathf.Rad2Deg;
                }
                else
                {
                    DegX = -Mathf.Sin(angle) * speedCamera * Mathf.Rad2Deg;
                }

                if (mousePositionUp.x > mousePositionDown.x)
                {
                    DegY = -Mathf.Cos(angle) * speedCamera * Mathf.Rad2Deg;
                }
                else
                {
                    DegY = Mathf.Cos(angle) * speedCamera * Mathf.Rad2Deg;
                }

                Quaternion newRotation;
                newRotation = Camera.main.transform.rotation * Quaternion.Euler(new Vector3(DegX,
                    DegY, 0) * Time.deltaTime);
                Camera.main.transform.rotation = newRotation;

                PrevMousePos = Camera.main.ViewportToScreenPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y,
                    Camera.main.nearClipPlane));
            }
            else
            {
                mousePositionUp = mousePositionDown;
            }
        }
    }
}