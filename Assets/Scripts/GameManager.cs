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
    public Dictionary<string, PlanetInformation> listPlanetInformationstmp;
    public float scale = 1;
    private Vector2 mousePositionUp;
    private Vector2 mousePositionDown;
    private Vector3 PrevMousePos;
    private Camera camera;
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
    public Slider slider;
    public Slider upWidth;
    public Slider upHeight;
    public string nameActive;
    public bool isDrawAgain = false;




    void Start()
    {
        camera = Camera.main;
        showInformation = GameObject.FindWithTag("UiManager").GetComponent<ShowInformation>();
        camera.fieldOfView = 15f;
        listPlanetInformations = new Dictionary<string, PlanetInformation>();
        listPlanetInformationstmp = new Dictionary<string, PlanetInformation>();
        foreach (var planetInformation in list.PlanetsInformation)
        {
            listPlanetInformations.Add(planetInformation.tag, planetInformation);
            GameObject.Find(planetInformation.tag).transform.rotation =
                Quaternion.Euler(0, 0, listPlanetInformations[planetInformation.tag].rotary);
        }

        listPlanetInformationstmp = listPlanetInformations;

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
                camera.fieldOfView += zoomCamera;
            }
            else if (Input.mouseScrollDelta.y == 1)
            {
                camera.fieldOfView -= zoomCamera;
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
                mousePositionUp = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, camera.nearClipPlane));
            }

            mousePositionDown = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                camera.nearClipPlane));

            distance = Mathf.Sqrt(Mathf.Pow(mousePositionUp.x - mousePositionDown.x, 2) +
                                  Mathf.Pow(mousePositionUp.y - mousePositionDown.y, 2)) * speedCamera;
            yDistance = (Mathf.Max(mousePositionUp.y, mousePositionDown.y) -
                         Mathf.Min(mousePositionUp.y, mousePositionDown.y)) * speedCamera;
            if (distance <= 0)
            {
                return;
            }
            angle = Mathf.Sin(yDistance / distance);
            if (angle > 0.8f)
            {
                angle = Mathf.Asin(yDistance / distance);
            }

            if (PrevMousePos != camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, camera.nearClipPlane)))
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
                camera.transform.position +=  new Vector3(x, y);
                PrevMousePos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, 0));
                x = 0;
                y = 0;
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
            camera.fieldOfView -= zoomCamera * Time.deltaTime * 10 * speedCamera;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            camera.fieldOfView += zoomCamera * Time.deltaTime * 10 * speedCamera;
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
            y = -Mathf.Sin(Time.deltaTime) * 100 * speedCamera;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            y = Mathf.Sin(Time.deltaTime) * 100 * speedCamera;
        }

        camera.transform.position +=  new Vector3(x, y);
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
                mousePositionUp = camera.ViewportToScreenPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y,
                    camera.nearClipPlane));
            }

            mousePositionDown = camera.ViewportToScreenPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y,
                camera.nearClipPlane));

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
            if (
                (mousePositionDown - mousePositionUp).sqrMagnitude > 0.5f)

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
                newRotation = camera.transform.rotation * Quaternion.Euler(new Vector3(DegX,
                    DegY, 0) * Time.deltaTime);
                camera.transform.rotation = newRotation;
            }
            else
            {
                mousePositionUp = mousePositionDown;
            }
        }
    }
    
    public void UpWidth()
    {
        if (nameActive == "")
        {
            return;
        }

        upWidth.onValueChanged.AddListener((arg0 =>
        {
            listPlanetInformations[nameActive].width = arg0;
            isDrawAgain = true;

        }));
    }

    public void UpHeight()
    {
        if (nameActive == "")
        {
            return;
        }

        upHeight.onValueChanged.AddListener((arg0 =>
        {
            listPlanetInformations[nameActive].height = arg0;
            isDrawAgain = true;
        }));
        
    }

    

    
}