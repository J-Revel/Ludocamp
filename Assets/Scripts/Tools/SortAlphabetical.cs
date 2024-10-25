using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SortAlphabetical : MonoBehaviour
{
    public List<string> values;
    public bool sort;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(sort)
        {
            sort = false;
            values.Sort();
        }
    }
}
