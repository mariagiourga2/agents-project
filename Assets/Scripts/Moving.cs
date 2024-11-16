using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class Moving : MonoBehaviour
{
    void Start()
    {
        Vector3 Pos= transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.right * Time.deltaTime);
        transform.Translate(Vector3.left * Time.deltaTime);
        transform.Translate(Vector3.forward * Time.deltaTime);
    }
}
