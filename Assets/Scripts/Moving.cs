using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class Moving : MonoBehaviour
{
    int energy = 100;
    private int gold = 0;
    public TextMeshProUGUI TextE;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 Pos= transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.right * Time.deltaTime);
        energy --;
        Debug.Log(energy.ToString());
      //  Debug.Log(TextE.text="Remain :" + energy.ToString());
    }
}
