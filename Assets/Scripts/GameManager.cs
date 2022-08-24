using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private helper helper;
    public ShowInformation showInformation;
    public List list;
    public Dictionary<string, PlanetInformation> listPlanetInformations;
    public Dictionary<string, PlanetInformation> listPlanetInformationsTmp;
    public float scale = 1;
    private Vector2 mousePositionUp;
    private Vector2 mousePositionDown;
    private Vector3 PrevMousePos;
    private Camera camera;
    private float distance;
    private float angle;
    private float yDistance;
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
    public Button resetAll;
    public Button resetOne;
    public Button pause;
    public TextMeshProUGUI pauseText;
    public Slider timeScale;
    public string nameActive;
    public bool isDrawAgain;
    public bool moveY3D;
    public float prevSpeed;
    public int count;
    public bool isDrawAgainAll;
    public int countPlanetDrew;
    public string prevNameActive;
    public bool firtsCheckName;
    private float prevTimeScale;
    public bool isDraw = false;

    private void Awake()
    {
        Time.timeScale = 1/3f;
        timeScale.value = Time.timeScale;
    }

    void Start()
    {
        camera = Camera.main;
        showInformation = GameObject.FindWithTag("UiManager").GetComponent<ShowInformation>();
        camera.fieldOfView = 15f;
        listPlanetInformations = new Dictionary<string, PlanetInformation>();
        listPlanetInformationsTmp = new Dictionary<string, PlanetInformation>();

        foreach (var planetInformation in list.PlanetsInformation)
        {
            listPlanetInformations.Add(planetInformation.tag, planetInformation);
            GameObject.Find(planetInformation.tag).transform.eulerAngles =
                new Vector3(0, 0, -listPlanetInformations[planetInformation.tag].rotary);
            GameObject.Find(planetInformation.tag).transform.localScale = new Vector3(planetInformation.size,
                planetInformation.size, planetInformation.size);
        }

        listPlanetInformationsTmp = CloneDictionaryCloningValues(listPlanetInformations);
        ResetOne();
        ResetAll();
        Pause();
    }

    private void Update()
    {
        if (nameActive != "")
        {
            slider.value = listPlanetInformations[nameActive].speed;
        }

        if (Input.GetButton("Fire2") && Input.mouseScrollDelta != Vector2.zero)
        {
            ChangeSpeedCamera();
            prevSpeed = speedCamera;
            showInformation.ShowSpeedCamera(speedCamera);
        }
        else
        {
            ZoomCamera();
            if (prevSpeed == speedCamera)
            {
                count++;
                if (count == 100)
                {
                    showInformation.changeSpeedCamera.gameObject.SetActive(false);
                    prevSpeed = 0;
                    count = 0;
                }
            }
        }

        MoveCamera();
        changeView.onValueChanged.AddListener((delegate { ChangeView3D(changeView.isOn); }));
    }

    public float GetPrevTimeScale()
    {
        return prevTimeScale;
    }
    
    public void SetPrevTimeScale(float value)
    {
        this.prevTimeScale = value;
    }
    public void MoveCamera()
    {
        if (!changeValueView)
        {
            FreeCamera2D("Fire2");
        }
        else
        {
            FreeCamera3D("Fire3");
        }
    }

    public void ZoomCamera()
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

    public void FreeCamera3D(string inputName)
    {
        if (Input.GetKey(KeyCode.Mouse2))
        {
            FreeCamera2D(inputName);
        }
        else
        {
            PositionCamera3D();
            RotationCamera3D();
        }
    }

    public void FreeCamera2D(string inputName)
    {
        if (!Input.GetButton(inputName))
        {
            firstCheck = true;
        }

        if (Input.GetButton(inputName))
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
            if (angle > 0.83f)
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

                camera.transform.position += new Vector3(x, y);
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

        camera.transform.position += new Vector3(x, y);
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
            if (angle > 0.78f)
            {
                angle = Mathf.Asin(yDistance / distance);
                moveY3D = true;
                DegY = 0;
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
                    if (moveY3D == false)
                        DegY = -Mathf.Cos(angle) * speedCamera * Mathf.Rad2Deg;
                }
                else
                {
                    if (moveY3D == false)
                        DegY = Mathf.Cos(angle) * speedCamera * Mathf.Rad2Deg;
                }

                moveY3D = false;
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
        upWidth.onValueChanged.AddListener(arg0 =>
        {
            if (nameActive == "")
            {
                return;
            }

            if (nameActive == prevNameActive)
            {
                isDrawAgain = false;
            }

            if (arg0 < listPlanetInformations[nameActive].distaneWithSun * 2)
            {
                listPlanetInformations[nameActive].width = arg0;
            }
        });
    }

    public void UpHeight()
    {
        upHeight.onValueChanged.AddListener(arg0 =>
        {
            if (nameActive == "")
            {
                return;
            }

            if (nameActive == prevNameActive)
            {
                isDrawAgain = false;
            }

            if (arg0 < listPlanetInformations[nameActive].distaneWithSun * 2)
            {
                listPlanetInformations[nameActive].height = arg0;
                
            }
        });
    }

    public void ResetAll()
    {
        resetAll.onClick.AddListener(delegate
        {
            if (nameActive == "")
            {
                return;
            }

            countPlanetDrew = 0;
            listPlanetInformations = CloneDictionaryCloningValues(listPlanetInformationsTmp);
            isDrawAgainAll = true;
        });
    }

    public void ResetOne()
    {
        resetOne.onClick.AddListener(delegate
        {
            if (nameActive == "")
            {
                return;
            }

            listPlanetInformations[nameActive] = CloneDictionaryCloningValue(listPlanetInformationsTmp[nameActive]);
            upHeight.value = listPlanetInformations[nameActive].height;
            upWidth.value = listPlanetInformations[nameActive].width;
            isDrawAgain = true;
        });
    }

    public void Pause()
    {
        pause.onClick.AddListener(delegate
        {
            if (Time.timeScale != 0)
            {
                prevTimeScale = Time.timeScale;
                Time.timeScale = 0.0f;
                pauseText.text = "Tiếp tục";

            }
            else
            {
                Time.timeScale = prevTimeScale;
                pauseText.text = "Tạm Dừng";
            }
        });
    }

    public void TimeScale()
    {
        timeScale.onValueChanged.AddListener(arg0 =>
        {
            Time.timeScale = arg0;
        });
    }


    public  Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue>
        (Dictionary<TKey, TValue> original) where TValue : ICloneable
    {
        Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count,
            original.Comparer);
        foreach (KeyValuePair<TKey, TValue> entry in original)
        {
            ret.Add(entry.Key, (TValue)entry.Value.Clone());
        }

        return ret;
    }

    public TValue CloneDictionaryCloningValue<TValue>
        (TValue original) where TValue : ICloneable
    {
        return (TValue)original.Clone();
    }
   
}