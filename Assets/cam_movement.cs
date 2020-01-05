using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;


public class cam_movement : MonoBehaviour
{
    [SerializeField] launch_eye_track eye;


    string text;
    string[] split_text;
    float x, y;
    public int limit;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        text = eye.text;
        split_text = text.Split(',');
        print(">> " + text);
        if (split_text.Length == 2)
        {
            x = float.Parse(split_text[0], CultureInfo.InvariantCulture);
            y = float.Parse(split_text[1], CultureInfo.InvariantCulture);
            this.transform.rotation = Quaternion.Euler(-1 * (y + 0.5f) * limit, -1 * x * limit, 0);
        }
    }
}
