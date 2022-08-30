using System;
using UnityEngine;

public class PlanetInformation : MonoBehaviour, ICloneable
{
    public string name;
    public string tag;
    public float distaneWithSun;
    public float width;
    public float height;
    public float speed;
    public string longDescription;
    public string element;
    public float size;
    public string radius;
    public float rotary;
    public float speedrotation;
    public float zPosition;
    public int directionOfRotation;
    public int xRotation;




    public object Clone()
    {
        PlanetInformation planetInformation = (PlanetInformation) this.MemberwiseClone();
        planetInformation.element = string.Copy(element);
        planetInformation.name = string.Copy(name);
        planetInformation.tag = string.Copy(tag);
        planetInformation.longDescription = string.Copy(longDescription);
        planetInformation.distaneWithSun = distaneWithSun;
        planetInformation.width = width;
        planetInformation.height = height;
        planetInformation.speed = speed;
        planetInformation.size = size;
        planetInformation.rotary = rotary;
        planetInformation.speedrotation = speedrotation;
        return planetInformation;

    }
    
}
