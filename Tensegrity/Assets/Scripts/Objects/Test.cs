using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    [SerializeField] private Transform Obj0;
    [SerializeField] private Transform Obj1;
    

    private Vector3 p0 = new Vector3();
    private Vector3 p1 = new Vector3();
    

    private Vector3 d0;
    private Vector3 d1;
    private Vector3 d2;

    private SpringJoint fj = new SpringJoint() ;


    // Use this for initialization
    void Start ()
	{
	    p0 = Obj0.localPosition;
	    p1 = Obj1.localPosition;
	    
	    d0 = p0 - p1;
	    d1 = p1 - p0;
	  
	}
	
	// Update is called once per frame
	void Update ()
	{
	    p0 = Obj0.localPosition;
	    p1 = Obj1.localPosition;

        Obj0.localPosition = p1  + d0;
        Obj1.localPosition = p0  + d1;

    }
}
