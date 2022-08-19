using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CircleMovation : MonoBehaviour
{
    public float x, y, z;
    public float angle;
    private PlanetInformation planetInformation;
    private GameManager gameManager;
    private ShowInformation showInformation;
    public Slider changeSpeedCamera;
    public LineRenderer orbit;
    public int currentIndex = 0;
    private int indexMax = 1000;
    private bool startMove;
    private int count;
    private float prevWidth;
    private float prevHeight;

    void Start()
    {
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
            if (prevHeight != gameManager.listPlanetInformations[transform.name].width)
            {
                gameManager.isDrawAgain = true;
                prevHeight = gameManager.listPlanetInformations[transform.name].width;
            }
            
            if (prevWidth != gameManager.listPlanetInformations[transform.name].height)
            {
                gameManager.isDrawAgain = true;
                prevWidth = gameManager.listPlanetInformations[transform.name].height;
            }
            SliderController();
            ChangeSpeedCameraSlier();
            angle += 0.03f * gameManager.listPlanetInformations[transform.name].speed * 1 / 3;
            x = Mathf.Cos(angle) * gameManager.listPlanetInformations[transform.name].width * gameManager.scale;
            y = Mathf.Sin(angle) * gameManager.listPlanetInformations[transform.name].height * gameManager.scale;
            transform.position = new Vector3(x, y, z);
            Draw(transform.position);
            transform.rotation *= Quaternion.Euler(Vector3.left * gameManager.listPlanetInformations[name].speed);
            if (gameManager.checkShowInf && gameManager.nameActive == name)
            {
                showInformation.name.rectTransform.transform.position =
                    Camera.main.WorldToScreenPoint(transform.position) + showInformation.positionNameActive;
            }

            if (gameManager.isDrawAgainAll && gameManager.countPlanetDrew < 9)
            {
                gameManager.countPlanetDrew++;
                DrawAgain();
            }
            else if (gameManager.isDrawAgain && gameManager.nameActive == name)
            {
                DrawAgain();
                gameManager.isDrawAgain = false;
            }
        }
    }

    private void OnMouseDown()
    {
        gameManager.checkShowInf = true;
        showInformation.ShowInformationPlanet(transform.gameObject.name, this.gameObject);
        if (Input.GetButtonDown("Fire1"))
        {
            CastRay();
        }
        gameManager.firtsCheckName = false;
    }

    public void SliderController()
    {
        gameManager.slider.onValueChanged.AddListener((arg0 => ScrollbarCallback(arg0)));
        
    }

    public void ScrollbarCallback(float value)
    {
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
        if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, Mathf.Infinity) && gameManager.firtsCheckName == false)
        {
            gameManager.prevNameActive = gameManager.CloneDictionaryCloningValue(gameManager.nameActive);
            gameManager.firtsCheckName = true;
            gameManager.nameActive = raycastHit.transform.name;
            
            if (gameManager.nameActive == name)
            {
                Debug.Log(gameManager.nameActive + "-" + gameManager.prevNameActive);
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
        if (orbit.positionCount < indexMax)
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
        indexMax = 1000;
    }

}