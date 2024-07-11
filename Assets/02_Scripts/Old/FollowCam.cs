using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

    public Transform Target;
    public float Height = 5f;
    public float Distance = 10f;
    private float Damping = 3f;
    private Transform Camtr;
    private Quaternion rot = Quaternion.identity;
	void Start ()
    {
        Camtr = Camera.main.transform;
		
	}
	
	void LateUpdate ()
    {
        float angle = Mathf.LerpAngle(Target.eulerAngles.y, Camtr.eulerAngles.y, 
                                       Time.deltaTime * Damping);
        rot = Quaternion.Euler(0f, angle, 0f);
        Camtr.position = Target.position - ( Vector3.forward * Distance) + (rot * Vector3.up * Height);
        Camtr.LookAt(Target.transform);
	}
}
