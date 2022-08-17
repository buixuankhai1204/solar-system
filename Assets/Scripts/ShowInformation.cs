using System;
using TMPro;
using UnityEngine;

public class ShowInformation : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI name;
    public TextMeshProUGUI nameSpeed;
    public TextMeshProUGUI size;
    public TextMeshProUGUI element;
    public TextMeshProUGUI longDescription;
    public TextMeshProUGUI rotary;
    public TextMeshProUGUI nameInf;
    public Vector3 positionNameActive;
    public TextMeshProUGUI changeSpeedCamera;
    public GameObject planet;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        positionNameActive = new Vector3(70, 10, 0);
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
                this.rotary.text = planet.Value.rotary.ToString();
            }
        }
    }

    public void ShowSpeedCamera(float value)
    {
        changeSpeedCamera.gameObject.SetActive(true);
        changeSpeedCamera.text = Math.Round(value, 2) + "x";
    }
}