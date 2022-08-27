using System;
using TMPro;
using UnityEngine;

public class ShowInformation : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI name;
    public TextMeshProUGUI nameSpeed;
    public TextMeshProUGUI size;
    public TextMeshProUGUI Radius;
    public TextMeshProUGUI element;
    public TextMeshProUGUI longDescription;
    public TextMeshProUGUI rotary;
    public TextMeshProUGUI nameInf;
    public Vector3 positionNameActive;
    public TextMeshProUGUI changeSpeedCamera;
    public GameObject planet;
    public GameObject Information;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        positionNameActive = new Vector3(70, 10, 0);
    }


    private void Update()
    {
        if (gameManager.nameActive == "")
        {
            gameManager.slider.transform.gameObject.SetActive(false);
            gameManager.upHeight.transform.gameObject.SetActive(false);
            gameManager.upWidth.transform.gameObject.SetActive(false);
            gameManager.resetOne.transform.gameObject.SetActive(false);
            Information.SetActive(false);
        }
        else
        {
            gameManager.slider.transform.gameObject.SetActive(true);
            gameManager.upHeight.transform.gameObject.SetActive(true);
            gameManager.upWidth.transform.gameObject.SetActive(true);
            gameManager.resetOne.transform.gameObject.SetActive(true);
            Information.SetActive(true);
        }
    }

    public void ShowInformationPlanet(string name, GameObject gameObject)
    {
        foreach (var planet in gameManager.listPlanetInformations)
        {
            this.planet = gameObject;
            if (planet.Key == name)
            {
                this.name.text = planet.Value.name;
                this.name.fontSize = 14f;
                this.nameSpeed.text = "Vận tốc của: " + planet.Value.name;
                this.nameInf.text = planet.Value.name;
                this.size.text = planet.Value.size.ToString();
                this.element.text = planet.Value.element;
                this.longDescription.text = planet.Value.longDescription;
                this.rotary.text = planet.Value.rotary + " độ";
                this.Radius.text = planet.Value.radius;
            }
        }
    }

    public void ShowSpeedCamera(float value)
    {
        changeSpeedCamera.gameObject.SetActive(true);
        changeSpeedCamera.text = Math.Round(value, 2) + "x";
    }
}