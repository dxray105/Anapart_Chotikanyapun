using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    private Camera cam;
    private int gridSize = 5;
    public static Selector instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        cam = Camera.main;
    }

    public Vector3 GetCurTilePosition()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        float rayDistance = 0.0f;

        if (plane.Raycast(ray, out rayDistance))
        {
            Vector3 newPos = ray.GetPoint(rayDistance);
            newPos = new Vector3(newPos.x, 0.0f, newPos.z);

            return newPos;
        }

        return new Vector3(0, -99, 0);
    }
}
