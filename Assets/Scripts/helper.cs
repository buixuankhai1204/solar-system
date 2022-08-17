using UnityEngine;

public class helper : MonoBehaviour
{
    // Start is called before the first frame update
    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }
}
