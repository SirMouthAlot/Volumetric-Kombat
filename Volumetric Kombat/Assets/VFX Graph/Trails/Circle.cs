using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public float speed;
    public float width;
    public float height;
    public GameObject circle; 


    // Start is called before the first frame update
    void Start()
    {
       speed = 5;
       width = 4;
       height = 4;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Mathf.Cos(Time.deltaTime) * width;
        float y = 0;
        float z = Mathf.Sin(Time.deltaTime) * height; 

        circle.transform.position = new Vector3(x, y, z);
    }
}
