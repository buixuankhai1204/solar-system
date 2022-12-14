using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleMovation : MonoBehaviour
{
    private helper helper;
    private float x, y, z;
    public float angle;
    private PlanetInformation planetInformation;
    private GameManager gameManager;
    private ShowInformation showInformation;
    public Slider changeSpeedCamera;
    public LineRenderer orbit;
    private int currentIndex;
    private bool startMove;
    private int count;
    private float prevWidth;
    private float prevHeight;
    private bool firstClick = true;
    public float zPosition;
    private float speedAll;
    private List<Vector3> liststartPoints;

    public bool firstAdd = true;
    public int indexMax = 2000;
    public bool firstDraw = true;
    public bool firstDrawHeight = true;
    private bool firstAddPoint = true;
    private Vector3 firstPoint;
    private bool secondCheckWidth = true;
    private bool secondCheckHeight = true;
    private int countPos = 0;

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
        if (transform.name != "Moon")
        {
            for (int i = 0; i < indexMax; i++)
            {
                angle += speedAll * gameManager.listPlanetInformations[transform.name].speed * Time.timeScale;
                x = Mathf.Cos(angle) * gameManager.listPlanetInformations[transform.name].width * gameManager.scale;
                y = Mathf.Sin(angle) * gameManager.listPlanetInformations[transform.name].height * gameManager.scale;
                z = Mathf.Cos(angle) * gameManager.listPlanetInformations[transform.name].zPosition * gameManager.scale;
                orbit.positionCount++;
                currentIndex = orbit.positionCount - 1;
                orbit.SetPosition(currentIndex, new Vector3(x, y, z));
                liststartPoints.Add(new Vector3(x, y, z));
            }
        }

        startMove = true;
    }

    void Update()
    {
        if (startMove)
        {
            SliderController();
            ChangeSpeedCameraSlier();
            RotatePlanet();
            CheckDrawAgain();
            SetPositionPlanet();
            ChangeBoxCollider();
            
            showInformation.ShowNameActive(transform);
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
        if (Time.timeScale == 0)
        {
            return;
        }

        if (gameManager.nameActive == "")
        {
            return;
        }

        gameManager.slider.onValueChanged.AddListener(arg0 =>
        {
            gameManager.listPlanetInformations[gameManager.nameActive].speed = arg0;
            
        });
    }

    public void ChangeSpeedCameraSlier()
    {
        changeSpeedCamera.onValueChanged.AddListener((arg0 => ChangeSpeedCameraCallback(arg0)));
        if (changeSpeedCamera != null)
        {
            changeSpeedCamera.value = gameManager.speedCamera;
        }
    }

    public void ChangeSpeedCameraCallback(float value)
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        gameManager.speedCamera = value;
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

    public void RunDefault(float width, float height)
    {
        orbit.positionCount = 0;
        currentIndex = 0;
        for (int i = 0; i < indexMax; i++)
        {
            angle += speedAll * gameManager.listPlanetInformations[transform.name].speed * Time.timeScale;
            x = Mathf.Cos(angle) * width * gameManager.scale;
            y = Mathf.Sin(angle) * height * gameManager.scale;
            z = Mathf.Cos(angle) * gameManager.listPlanetInformations[transform.name].zPosition * gameManager.scale;
            orbit.positionCount++;
            currentIndex = orbit.positionCount - 1;
            orbit.SetPosition(currentIndex, new Vector3(x, y, z));
        }
        angle = 0;
        
    }

    public void ChangeBoxCollider()
    {
        
        GetComponent<SphereCollider>().radius = Camera.main.fieldOfView / 10;
        if (GetComponent<SphereCollider>().radius < 1)
        {
            GetComponent<SphereCollider>().radius = 1;
        }

        if (transform.name == "Sun Sphere")
        {
            GetComponent<SphereCollider>().radius = 1;
        }

        if (transform.name == "Moon")
        {
            GetComponent<SphereCollider>().radius = Camera.main.fieldOfView / 5;
        }
    }

    private void CheckDrawAgain()
    {
        if (gameManager.isDrawAgainAll && gameManager.countPlanetDrew < 10)
        {
            gameManager.countPlanetDrew++;
            // Invoke(nameof(DrawAgain), 0.05f);
            RunDefault(gameManager.listPlanetInformations[transform.name].width,
                gameManager.listPlanetInformations[transform.name].height);
        }
        else if (gameManager.isDrawAgain && gameManager.nameActive == name)
        {
            RunDefault(gameManager.listPlanetInformations[transform.name].width,
                gameManager.listPlanetInformations[transform.name].height);
            gameManager.isDrawAgain = false;
        }
    }

    private void SetPositionPlanet()
    {
        
        if (transform.name == "Moon")
        {
            transform.position = GameObject.FindWithTag("Earth").transform.position;
            transform.position += new Vector3(x * zPosition, y * zPosition);
        }
        else
        {
            transform.position = orbit.GetPosition(countPos);
            countPos++;
            if (countPos == orbit.positionCount)
            {
                countPos = 0;
            }
        }
    }

    public void RotatePlanet()
    {
        transform.Rotate(
            Vector3.down * gameManager.listPlanetInformations[transform.name].speedrotation * Time.timeScale *
            0.05f * gameManager.listPlanetInformations[transform.name].directionOfRotation,
            Space.Self);
    }
    
}