using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class Strings : MonoBehaviour
{
    private Transform Point0;
    private Transform Point1;

    private GameObject String;
    private GameObject LengthController;
    [SerializeField]private GameObject _CanvasPrefab;
     Transform LengthSlider;

    private Vector3 _Joint0 = new Vector3();
    private Vector3 _Joint1 = new Vector3();

    private SpringJoint S0 = new SpringJoint();
    private SpringJoint S1 = new SpringJoint();

    private GameObject _Canvas;

    private float defualtStringLength;
    private float updateStringLength;
    private float Elasticity = 0.01f;
    private float Thickness;
    private int index;

    public void ConnectString(Transform  J0, Transform  J1, float _thickness, int _index)
    {
        LengthController = GameObject.CreatePrimitive(PrimitiveType.Cube);
        LengthController.GetComponent<MeshRenderer>().material.color = Color.black;

        
        //_Canvas.renderMode = RenderMode.WorldSpace;

       

        Point0 = J0;
        Point1 = J1;
        _Joint0 = J0.position;
        _Joint1 = J1.position;

        var d = _Joint1 - _Joint0;
        var L = d.magnitude;

        defualtStringLength = L;
        Thickness = _thickness;

        var T = gameObject.GetComponent<Transform>();
        var ControllerForm = LengthController.GetComponent<Transform>();

        _Canvas = Instantiate(_CanvasPrefab, ControllerForm);

        LengthSlider = _Canvas.GetComponent<RectTransform>().GetChild(0);
        LengthSlider.GetComponent<Slider>().value = defualtStringLength;

        var CvsForm = _Canvas.GetComponent<RectTransform>();


      
      

        T.position = ControllerForm.position  = (_Joint0 +_Joint1) * 0.5f;
        T.localScale = new Vector3(Thickness, L*0.5f, Thickness);
        T.localRotation =ControllerForm .localRotation  = Quaternion.FromToRotation(T.up, d);
        
        LengthController.AddComponent<Rigidbody>();
        LengthController.GetComponent<BoxCollider>().enabled = false;
        LengthController.GetComponent<Rigidbody>().useGravity = false;

        LengthController.GetComponent<Transform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);

        Rigidbody _Body0 = J0.GetComponent<Rigidbody>();
        Rigidbody _Body1 = J1.GetComponent<Rigidbody>();

        S0 =LengthController.AddComponent<SpringJoint>();
        S1 = LengthController.AddComponent<SpringJoint>();

        S0.autoConfigureConnectedAnchor = false;
        S1.autoConfigureConnectedAnchor = false;

        S0.connectedBody = _Body0;
        S1.connectedBody = _Body1;

        S0.anchor = new Vector3(0, 0, 0);
        S0.connectedAnchor = new Vector3(0, 0, 0);
        S0.minDistance = L * 0.2f ;
        S0.maxDistance = L * 0.5f ;
        S0.spring = 350f;


        S1.anchor = new Vector3(0, 0, 0);
        S1.connectedAnchor = new Vector3(0, 0, 0);
        S1.minDistance = L * 0.2f;
        S1.maxDistance = L * 0.5f ;
        S1.spring = 350f;

        index = _index;
    }

    public float GetStringLength()
    {
        return defualtStringLength;
    }

    public int GetVertexIndex0()
    {
        return Point0.GetComponent<TJoint>().GetIndex();
    }


    public void ChangeSliderValue(float _length)
    {
       
        LengthSlider.GetComponent<Slider>().value = _length;
    }

    public void UpdateStringLength()
    {
        var _L = LengthSlider.GetComponent<Slider>().value;

        //UpdateStringPosition();
        updateStringLength = _L;
        //gameObject.GetComponent<Transform>().localScale = new Vector3( Thickness ,updateStringLength ,Thickness);
        float d = defualtStringLength - _L ;
        S1.anchor = new Vector3(0, -d, 0);
        //S1.spring = 1500f / (Mathf .Pow( _L,2)*0.08f);
        S0.anchor = new Vector3(0, d, 0);
        //S0.spring = 1500f / (Mathf.Pow(_L, 2) * 0.08f);
    }

    public  void UpdateStringPosition()
    {
        var pastD = _Joint1 - _Joint0;
       
        _Joint0 = Point0.position;
        _Joint1 = Point1.position;

        var d = _Joint1 - _Joint0;
        var L = d.magnitude;
        var T = gameObject.GetComponent<Transform>();
        var Lc = LengthController.GetComponent<Transform>();

        T.localScale = new Vector3(Thickness, L * 0.5f, Thickness);
        T.position = (_Joint0 + _Joint1) * 0.5f;
        Lc.position = (_Joint0 + _Joint1) * 0.5f;

        var q = Vector3.Cross(Vector3.down, d);
        
        T.localRotation = Quaternion.LookRotation(q, d);
        Lc.localRotation = Quaternion.LookRotation(q, d);
        UpdateStringColor();
    }

    public void SetElasticity(float _Elasticity)
    {
        Elasticity = _Elasticity;
    }

    void UpdateStringColor()
    {
        var Renderer = gameObject.GetComponent<MeshRenderer>();
        float _L = gameObject.GetComponent<Transform>().localScale.y;

        if (_L > defualtStringLength * 0.5f)
        {
            float T = defualtStringLength * 0.5f / _L;
            Renderer.material.color = Color.Lerp(Color.green , Color.yellow , T);
        }else if (_L < defualtStringLength * 0.5f)
        {
            float T = _L / defualtStringLength * 0.5f;
            Renderer.material.color = Color.Lerp(Color.yellow  , Color.red , T);
        }
    }

    private void Update()
    {
        
    }
}
