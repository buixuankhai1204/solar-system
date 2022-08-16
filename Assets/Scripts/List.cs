using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class List : ScriptableObject
{
    public PlanetInformation[] PlanetsInformation;
    public PlanetInformation[] PlanetsInformationTmp;
}