using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CircleMovation : MonoBehaviour
{
    public float x, y, z;
    public float angle;
    private PlanetInformation planetInformation;
    private GameManager gameManager;
    private ShowInformation showInformation;
    private bool checkShowInf = false;

    public Slider changeSpeedCamera;
    public LineRenderer orbit;
    public int currentIndex = 0;
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
            angle += 0.05f * gameManager.listPlanetInformations[transform.name].speed * 1 / 3;
            x = Mathf.Cos(angle) * gameManager.listPlanetInformations[transform.name].width * gameManager.scale;
            y = Mathf.Sin(angle) * gameManager.listPlanetInformations[transform.name].height * gameManager.scale;
            transform.position = new Vector3(x, y, z);
            Draw(transform.position);
            transform.rotation *= Quaternion.Euler(Vector3.left * gameManager.listPlanetInformations[name].speed);
            if (gameManager.checkShowInf && gameManager.nameActive == this.name)
            {
                showInformation.name.rectTransform.transform.position = Camera.main.WorldToScreenPoint(transform.position) + showInformation.positionNameActive;
            }
            if (gameManager.isDrawAgain && name == gameManager.nameActive)
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
    }

    public void SliderController()
    {
        gameManager.slider.onValueChanged.AddListener((arg0 => ScrollbarCallback(arg0)));
        if (gameManager.nameActive != "")
        {
            gameManager.slider.value = gameManager.listPlanetInformations[gameManager.nameActive].speed;

        }

        if (Input.GetMouseButtonDown(0))
        {
            CastRay();
            
        }
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
        if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, Mathf.Infinity))
        {
            gameManager.nameActive = raycastHit.transform.name;
            if (gameManager.nameActive == name)
            {
                gameManager.upHeight.value = gameManager.listPlanetInformationstmp[gameManager.nameActive].height;
                gameManager.upHeight.maxValue = gameManager.listPlanetInformationstmp[gameManager.nameActive].height * 2;
                gameManager.upHeight.minValue = 0;
                gameManager.upWidth.value = gameManager.listPlanetInformationstmp[gameManager.nameActive].width;
                gameManager.upWidth.maxValue = gameManager.listPlanetInformationstmp[gameManager.nameActive].width * 2;
                gameManager.upWidth.minValue = 0; 
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
        orbit.material = orbit.materials[Random.Range(0,9)];
        orbit.positionCount = 0;
        currentIndex = 0;
        indexMax = 1000;
    }
}