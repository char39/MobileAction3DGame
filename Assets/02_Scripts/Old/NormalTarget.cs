using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTarget : MonoBehaviour
{
    public List<Collider> targetList;
	
	void Awake ()
    {
        targetList = new List<Collider>();
	}
    private void OnTriggerEnter(Collider other)
    {
        targetList.Add(other);
    }
    private void OnTriggerExit(Collider other)
    {
        targetList.Remove(other);
    }

}
