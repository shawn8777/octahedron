using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bars : MonoBehaviour
{
    private Vector3 [] Vertices = new Vector3[2];

    private FixedJoint[] Joints = new FixedJoint[2];

    Transform [] _Joints=new Transform [2];

     float  Length;

    private int index;

    public void SetupBar(Transform  J0, Transform  J1,float Thickness,int _index)
    {
        Vertices [0] = J0.position ;
        Vertices [1] = J1.position ;

        _Joints[0] = J0;
        _Joints[1] = J1;

        var T = gameObject.GetComponent<Transform>();
        var d = Vertices[1] - Vertices[0];

        var L = d.magnitude;

        T.localScale = new Vector3(Thickness, L/2, Thickness);
        T.position = (Vertices [0]+Vertices [1]) * 0.5f;
        T.localRotation =Quaternion.FromToRotation(T.up , d);

        Joints[0] = gameObject.AddComponent<FixedJoint>();
        Joints[1] = gameObject.AddComponent<FixedJoint>();

        Joints[0].connectedBody = J0.GetComponent<Rigidbody>();
        Joints[1].connectedBody = J1.GetComponent<Rigidbody>();
       

        Length = L;

        index = _index;
    }


    public float  GetLength()
    {
        return Length;
    }

    public int GetIndex()
    {
        return index;
    }

    public Vector3 GetJoint(int _index)
    {
        return Vertices [_index ];
    }

    public Transform GetTransform(int _index)
    {
        return _Joints[_index];
    }
}
