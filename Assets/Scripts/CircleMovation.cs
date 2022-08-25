using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CircleMovation : MonoBehaviour
{
    private helper helper;
    public float x, y, z;
    public float angle;
    private PlanetInformation planetInformation;
    private GameManager gameManager;
    private ShowInformation showInformation;
    public Slider changeSpeedCamera;
    public LineRenderer orbit;
    public int currentIndex = 0;
    private bool startMove;
    private int count;
    private float prevWidth;
    private float prevHeight;
    private bool firstClick = true;
    public float zPosition;
    public float speedAll;
    private List<Vector3> liststartPoints;

    public bool firstAdd = true;
    public int indexMax = 2000;
    public bool firstDraw = true;
    private bool secondCheckWidth = true;
    private bool secondCheckHeight = true;


    private void Awake()
    {
        liststartPoints = new List<Vector3>();
    }

    void Start()
    {
        speedAll = 0.03f;
        showInformation = GameObject.FindWithTag("UiManager").GetComponent<ShowInformation>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        StartCoroutine(StartMoving());
    }

    IEnumerator StartMoving()
    {
        yield return new WaitForSeconds(1);
        startMove = true;
    }

    void Update()
    {
        if (startMove)
        {
            SliderController();
            ChangeSpeedCameraSlier();

            angle += speedAll * gameManager.listPlanetInformations[transform.name].speed * Time.timeScale;
            x = Mathf.Cos(angle) * gameManager.listPlanetInformations[transform.name].width * gameManager.scale;
            y = Mathf.Sin(angle) * gameManager.listPlanetInformations[transform.name].height * gameManager.scale;
            z = Mathf.Cos(angle) * gameManager.listPlanetInformations[transform.name].zPosition * gameManager.scale;
            transform.Rotate(Vector3.down * gameManager.listPlanetInformations[name].speed *  Time.timeScale * 10f, Space.Self);

            if (Time.timeScale != 0 && speedAll != 0)
            {
                if (prevWidth != gameManager.listPlanetInformations[transform.name].width)
                {
                    gameManager.isDraw = true;
                    orbit.positionCount = 0;
                    prevWidth = gameManager.listPlanetInformations[transform.name].width;
                    if (secondCheckWidth)
                    {
                        secondCheckWidth = false;
                    }
                    else
                    {
                        firstAdd = false;
                    }
                }
                else if (prevHeight != gameManager.listPlanetInformations[transform.name].height)
                {
                    gameManager.isDraw = true;
                    orbit.positionCount = 0;
                    prevHeight = gameManager.listPlanetInformations[transform.name].height;
                    if (secondCheckHeight)
                    {
                        secondCheckHeight = false;
                    }
                    else
                    {
                        firstAdd = false;
                    }
                }
            }

            if (gameManager.isDrawAgainAll && gameManager.countPlanetDrew < 10)
            {
                gameManager.countPlanetDrew++;
                Invoke(nameof(DrawAgain), 0.05f);
            }
            else if (gameManager.isDrawAgain && gameManager.nameActive == name)
            {
                Invoke(nameof(DrawAgain), 0.05f);
                gameManager.isDrawAgain = false;
            }

            if (transform.name == "Moon")
            {
                transform.position = GameObject.FindWithTag("Earth").transform.position;
                transform.position += new Vector3(x * zPosition, y * zPosition);
            }
            else
            {
                transform.position = new Vector3(x, y, z);
                if (liststartPoints.Count < indexMax)
                {
                    if (firstAdd)
                    {
                        liststartPoints.Add(transform.position);
                        if (liststartPoints.Contains(transform.position))
                        {
                            firstAdd = false;
                        }
                    }

                }
                if (gameManager.isDraw)
                {
                    Draw(transform.position);
                }
                UpdateDraw();
            }

            if (gameManager.checkShowInf && gameManager.nameActive == name)
            {
                showInformation.name.rectTransform.transform.position =
                    Camera.main.WorldToScreenPoint(transform.position) + showInformation.positionNameActive;
            }
        }
    }

    private void OnMouseDown()
    {
        gameManager.SetPrevTimeScale(Time.timeScale);
        Time.timeScale = 1;
        if (firstClick)
        {
            firstClick = false;
            gameManager.isDrawAgain = false;
        }

        gameManager.checkShowInf = true;
        showInformation.ShowInformationPlanet(transform.gameObject.name, this.gameObject);
        if (Input.GetButtonDown("Fire1"))
        {
            CastRay();
        }

        gameManager.firtsCheckName = false;
        Time.timeScale = gameManager.GetPrevTimeScale();
    }

    public void SliderController()
    {
        gameManager.slider.onValueChanged.AddListener((arg0 => ScrollbarCallback(arg0)));
    }

    public void ScrollbarCallback(float value)
    {
        if (gameManager.nameActive == "")
        {
            return;
        }

        gameManager.listPlanetInformations[gameManager.nameActive].speed = value;
    }

    public void ChangeSpeedCameraCallback(float value)
    {
        gameManager.speedCamera = value;
    }

    public void ChangeSpeedCameraSlier()
    {
        changeSpeedCamera.onValueChanged.AddListener((arg0 => ChangeSpeedCameraCallback(arg0)));
        if (changeSpeedCamera != null)
        {
            changeSpeedCamera.value = gameManager.speedCamera;
        }
    }

    private void CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, Mathf.Infinity) &&
            gameManager.firtsCheckName == false)
        {
            gameManager.prevNameActive = gameManager.CloneDictionaryCloningValue(gameManager.nameActive);
            gameManager.firtsCheckName = true;
            gameManager.nameActive = raycastHit.transform.name;

            if (gameManager.nameActive == name)
            {
                gameManager.upHeight.minValue = 0;
                gameManager.upHeight.maxValue =
                    gameManager.listPlanetInformationsTmp[raycastHit.transform.name].distaneWithSun * 2;
                gameManager.upWidth.minValue = 0;
                gameManager.upWidth.maxValue =
                    gameManager.listPlanetInformationsTmp[raycastHit.transform.name].distaneWithSun * 2;
                gameManager.upHeight.value = gameManager.listPlanetInformations[raycastHit.transform.name].height;
                gameManager.upWidth.value = gameManager.listPlanetInformations[raycastHit.transform.name].width;
            }
        }
    }

    public void Draw(Vector3 drawPosition)
    {
        if (orbit.positionCount < liststartPoints.Count)
        {
            orbit.positionCount++;
            currentIndex = orbit.positionCount - 1;
            orbit.SetPosition(currentIndex, drawPosition);
        }
    }

    public void DrawAgain()
    {
        orbit.positionCount = 0;
        currentIndex = 0;
        for (int i = 0; i < liststartPoints.Count; i++)
        {
            orbit.positionCount++;
            currentIndex = orbit.positionCount - 1;
            orbit.SetPosition(currentIndex, liststartPoints[i]);
        }
    }

    public void UpdateDraw()
    {
        if (liststartPoints.Count < indexMax &&
            CheckDuplicate(transform.position, liststartPoints[liststartPoints.Count - 1]))
        {
            firstAdd = true;
            orbit.positionCount++;
            currentIndex = orbit.positionCount - 1;
            liststartPoints.Add(transform.position);
            orbit.SetPosition(currentIndex, transform.position);
        }
    }

    public bool CheckDuplicate(Vector3 firstPosition, Vector3 LastPosition)
    {
        if ((firstPosition - LastPosition).magnitude < 0.5f) return true;
        return false;
    }
}