using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class TensegrityManager : MonoBehaviour
{
    [SerializeField] private GameObject BarPrefab;
    [SerializeField] private GameObject StringPrefab;
    [SerializeField] private Transform JointPrefab;
    [SerializeField] private Slider LnthSlider;

    private Vector3[] XZBar00 = new Vector3[2];
    private Vector3[] XZBar01 = new Vector3[2];
    private Vector3[] XYBar00 = new Vector3[2];
    private Vector3[] XYBar01 = new Vector3[2];
    private Vector3[] YZBar00 = new Vector3[2];
    private Vector3[] YZBar01 = new Vector3[2];

    private Vector3[] PointCloud = new Vector3[12];

    private Transform[] JointsList = new Transform[12];
    private Transform[,] JointsList2d = new Transform[6, 2];
    GameObject [] _Bars=new GameObject [6];

    private GameObject [] _stng = new GameObject [24];

    private void Awake()
    {
        SetPoints();
    }

    // Use this for initialization
    void Start ()
    {
        SetJoints();
        SetBars();
        SetStrings();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    UpdateStrings();
	
	}

    void SetPoints()
    {
        XZBar00[0] = PointCloud [0]=new Vector3(-5, 0, -10);
        XZBar00[1] = PointCloud[1] = new Vector3(-5, 0, 10);
        XZBar01[0] = PointCloud[2] = new Vector3(5, 0, -10);
        XZBar01[1] = PointCloud[3] = new Vector3(5, 0, 10);
        XYBar00[0] = PointCloud[4] = new Vector3(-10, -5, 0);
        XYBar00[1] = PointCloud[5] = new Vector3(10, -5, 0);
        XYBar01[0] = PointCloud[6] = new Vector3(-10, 5, 0);
        XYBar01[1] = PointCloud[7] = new Vector3(10, 5, 0);
        YZBar00[0] = PointCloud[8] = new Vector3(0, -10, -5);
        YZBar00[1] = PointCloud[9] = new Vector3(0, 10, -5);
        YZBar01[0] = PointCloud[10] = new Vector3(0, -10, 5);
        YZBar01[1] = PointCloud[11] = new Vector3(0, 10, 5);
    }

    void SetJoints()
    {
        for (int i = 0; i < PointCloud.Length; i++)
        {
            Transform JT = Instantiate(JointPrefab, transform);
            JT.position = PointCloud[i];
            JT.GetComponent<TJoint>().SetIndex(i);
            JointsList[i] = JT;
            //JT.GetComponent<Rigidbody>().freezeRotation = true;
        }

        for (int Bi = 0,i=0; Bi < JointsList.Length / 2; Bi++)
        {
            for (int Si = 0; Si < 2; Si++,i++)
            {
                JointsList2d[Bi, Si] = JointsList[i];
            }
        }
    }


    void SetBars()
    {
        for (int i = 0; i < PointCloud.Length; i=i+2)
        {
            GameObject Bar = Instantiate(BarPrefab, transform);
            Bar.GetComponent<Bars>().SetupBar(JointsList[i], JointsList[i + 1], 0.5f, i / 2);
            _Bars[i / 2] = Bar;
        }
    }

    void SetStrings()
    {
        for (int i = 0,index=0; i < _Bars.Length ; i++)
        {
            for (int j = 0; j < 2; j++,index +=2)
            {
                int ToPoint;
                int ToIndex0;
                int ToIndex1;

                Transform _start;
                Transform _end0;
                Transform _end1;
                if (i % 2 == 0)
                {
                    ToPoint = 0;
                    ToIndex0 = 2;
                    ToIndex1 = 3;
                }
                else
                {
                    ToPoint = 1;
                    ToIndex0 = 1;
                    ToIndex1 = 2;
                }
                _start = _Bars[i].GetComponent<Bars>().GetTransform(j);
                if (i < _Bars.Length - 2)
                {
                    _end0 = _Bars[i + ToIndex0].GetComponent<Bars>().GetTransform(ToPoint);
                    _end1 = _Bars[i + ToIndex1].GetComponent<Bars>().GetTransform(ToPoint);
                }
               else
                {
                    _end0 = _Bars[0].GetComponent<Bars>().GetTransform(ToPoint);
                    _end1 = _Bars[1].GetComponent<Bars>().GetTransform(ToPoint);
                }

                var String0 = Instantiate(StringPrefab, transform);
                var String1 = Instantiate(StringPrefab, transform);

                String0.GetComponent<Strings>().ConnectString(_start, _end0, 0.2f, index);
                String1.GetComponent<Strings>().ConnectString(_start, _end1, 0.2f, index+1);
                _stng[index] = String0;
                _stng[index + 1] = String1;
            }
        }
    }

    void UpdateStrings()
    {
        foreach (var s in _stng)
        {
            s.GetComponent<Strings>().UpdateStringPosition();
            s.GetComponent<Strings>().UpdateStringLength();
        }
    }

    public void ChangeStringLength(float _L)
    {
        foreach (var s in _stng)
        {
            s.GetComponent<Strings>().ChangeSliderValue( _L );
        }
    }

    public IEnumerable<GameObject> SourceString()
    {
        return _stng;
    }

}
