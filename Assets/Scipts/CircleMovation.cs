using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CircleMovation : MonoBehaviour
{
    public float x, y, z;
    public float angle;
    private PlanetInformation planetInformation;
    private GameManager gameManager;
    private ShowInformation showInformation;
    private bool checkShowInf = false;
    private string nameActive;
    public Slider slider;
    public Button upWidth;
    public Button downWidth;
    public Button upHeight;
    public Button downHeight;
    public Vector3 positionNameActive;
    public Slider changeSpeedCamera;
    public LineRenderer orbit;
    public int currentIndex = 0;
    public bool isDrawAgain = false;
    private int indexMax = 500;
    private bool startMove = false;
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
            SliderController();
            ChangeSpeedCameraSlier();
            angle += Time.deltaTime * gameManager.listPlanetInformations[transform.name].speed * 1 / 3;
            x = Mathf.Cos(angle) * gameManager.listPlanetInformations[transform.name].width * gameManager.scale;
            y = Mathf.Sin(angle) * gameManager.listPlanetInformations[transform.name].height * gameManager.scale;
            transform.position = new Vector3(x, y, z);
            Draw(transform.position);
            transform.rotation *= Quaternion.Euler(Vector3.left * gameManager.listPlanetInformations[name].speed);
            if (gameManager.checkShowInf && nameActive == this.name)
            {
                showInformation.name.rectTransform.transform.position = Camera.main.WorldToScreenPoint(transform.position) + showInformation.positionNameActive;
            }
            if (isDrawAgain)
            {
                DrawAgain();
                isDrawAgain = false;
            }
        }
        
    }

    private void OnMouseDown()
    {
        gameManager.checkShowInf = true;
        showInformation.ShowInformationPlanet(transform.gameObject.name, this.gameObject);
        UpHeight();
        UpWidth();
        DownHeight();
        DownWidth();
    }

    public void SliderController()
    {
        slider.onValueChanged.AddListener((arg0 => ScrollbarCallback(arg0)));
        if (nameActive != null)
        {
            slider.value = gameManager.listPlanetInformations[nameActive].speed;
        }

        if (Input.GetMouseButtonDown(0))
        {
            CastRay();
        }
    }

    public void ScrollbarCallback(float value)
    {
        gameManager.listPlanetInformations[nameActive].speed = value;
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
        if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, Mathf.Infinity))
        {
            nameActive = raycastHit.transform.name;
        }
    }

    public void UpWidth()
    {
        if (nameActive == "")
        {
            return;
        }

        upWidth.onClick.AddListener((() =>
        {
            gameManager.listPlanetInformations[nameActive].width += 1;
            isDrawAgain = true;

        }));
    }

    public void DownWidth()
    {
        if (nameActive == "")
        {
            return;
        }

        downWidth.onClick.AddListener((() =>
        {
            gameManager.listPlanetInformations[nameActive].width -= 1;
            isDrawAgain = true;
        }));
        isDrawAgain = true;
        
    }

    public void UpHeight()
    {
        if (nameActive == "")
        {
            return;
        }

        upHeight.onClick.AddListener((() =>
        {
            gameManager.listPlanetInformations[nameActive].height += 1;
            isDrawAgain = true;
        }));
        isDrawAgain = true;
    }

    public void DownHeight()
    {
        if (nameActive == "")
        {
            return;
        }

        downHeight.onClick.AddListener((() =>
        {
            gameManager.listPlanetInformations[nameActive].height -= 1;
            isDrawAgain = true;
        }));
        isDrawAgain = true;
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