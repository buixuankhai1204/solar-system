using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

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
    private Vector3 prevMousePos;
    private Camera camera;
    private float distance;
    private float angle;
    private float yDistance;
    public bool firstCheck;
    public float speedCamera = 0.003f;
    public float zoomCamera = 1f;
    public bool checkShowInf;
    private float x, y, z;
    public bool changeValueView = true;
    public Toggle changeView;
    float degY, degX;
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
    public bool checkClickUi;

    private void Awake()
    {
        speedCamera = 0.3f;
        Time.timeScale = 1 / 3f;
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
                new Vector3(listPlanetInformations[planetInformation.tag].xRotation, 0,
                    listPlanetInformations[planetInformation.tag].rotary);
            GameObject.Find(planetInformation.tag).transform.localScale = new Vector3(planetInformation.size,
                planetInformation.size, planetInformation.size);
        }

        listPlanetInformationsTmp = CloneDictionaryCloningValues(listPlanetInformations);
        ResetOne();
        ResetAll();
    }

    private void Update()
    {
        if (camera.transform.position.y <= -300)
        {
            camera.nearClipPlane = -camera.transform.position.y - 100f;
        }
        else
        {
            camera.nearClipPlane = 200f;
        }
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

        TimeScale();
        changeView.onValueChanged.AddListener((delegate { ChangeView3D(changeView.isOn); }));
        MoveCamera();
        DeActiveGameObject();
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
            if (distance <= 0f)
            {
                return;
            }

            angle = Mathf.Sin(yDistance / distance);
            if (angle > 0.83f)
            {
                angle = Mathf.Asin(yDistance / distance);
            }

            if (prevMousePos != camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
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

                camera.transform.position += new Vector3(x, y) * speedCamera * 9.95f;
                prevMousePos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, camera.nearClipPlane));
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
            camera.fieldOfView -= zoomCamera * Time.deltaTime * 100 * speedCamera;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            camera.fieldOfView += zoomCamera * Time.deltaTime * 100 * speedCamera;
        }

        if (Input.GetKey(KeyCode.A))
        {
            x = -Mathf.Cos(0.05f) * 10 * speedCamera;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            x = Mathf.Cos(0.05f) * 10 * speedCamera;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            y = -Mathf.Sin(0.05f) * 200 * speedCamera;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            y = Mathf.Sin(0.05f) * 200 * speedCamera;
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
                degY = 0;
            }

            if (
                (mousePositionDown - mousePositionUp).sqrMagnitude > 0.5f)

            {
                if (mousePositionUp.y > mousePositionDown.y)
                {
                    degX = Mathf.Sin(angle) * speedCamera * Mathf.Rad2Deg;
                }
                else
                {
                    degX = -Mathf.Sin(angle) * speedCamera * Mathf.Rad2Deg;
                }

                if (mousePositionUp.x > mousePositionDown.x)
                {
                    if (moveY3D == false)
                        degY = -Mathf.Cos(angle) * speedCamera * Mathf.Rad2Deg;
                }
                else
                {
                    if (moveY3D == false)
                        degY = Mathf.Cos(angle) * speedCamera * Mathf.Rad2Deg;
                }

                moveY3D = false;
                Quaternion newRotation;
                newRotation = camera.transform.rotation * Quaternion.Euler(new Vector3(degX,
                    degY, 0) * 0.05f);
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

            if (Time.timeScale == 0.0f)
            {
                return;
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

            if (Time.timeScale == 0)
            {
                return;
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
            if (Time.timeScale == 0)
            {
                return;
            }

            countPlanetDrew = 0;
            listPlanetInformations = CloneDictionaryCloningValues(listPlanetInformationsTmp);
            isDrawAgainAll = true;
            camera.transform.position = new Vector3(0, 0, -300);
            camera.fieldOfView = 22.3f;
            camera.transform.rotation = new Quaternion(0, 0, 0, 0);
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

            if (Time.timeScale == 0)
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
        if (Time.timeScale > 0.0f)
        {
            pauseText.text = "Ti???p t???c";
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0.0f;
        }
        else
        {
            timeScale.value = prevTimeScale;
            pauseText.text = "T???m D???ng";
            Time.timeScale = prevTimeScale;
        }
    }

    public void TimeScale()
    {
        timeScale.onValueChanged.AddListener(arg0 =>
        {
            if (Time.timeScale == 0f)
            {
                return;
            }

            Time.timeScale = arg0;
        });
    }

    public void DeActiveGameObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out raycastHit, 1000f))
            {
                if (checkClickUi == false)
                {
                    nameActive = "";
                }
            }
            
        }
    }

    public Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue>
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

    public IEnumerator waitting()
    {
        yield return new WaitForSeconds(0.1f);
    }
}