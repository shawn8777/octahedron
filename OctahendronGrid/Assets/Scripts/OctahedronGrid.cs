using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctahedronGrid : MonoBehaviour {


    [SerializeField ]Transform PrefabX;
    [SerializeField] Transform PrefabY;
    [SerializeField] Transform PrefabZ;

    public Transform pivot;

    [SerializeField] int CountX = 5;
    int CountZ;

    [SerializeField] int HeightY = 5;

    [SerializeField] float Spaceing;
    [SerializeField] private float GrayScaleDegree;

    Transform[,,] OctaX;
    Transform[,,] OctaY;
    Transform[,,] OctaZ;

    [SerializeField] Texture2D SeedImage;
    [SerializeField] private Color DisplayColor;

    private Rules RuleInUse = new Rules();

    private Rules[] ruleStore = new Rules[13];

    private Rules Rule0 = new Rules();
    private Rules Rule1 = new Rules();
    private Rules Rule2 = new Rules();
    private Rules Rule3 = new Rules();
    private Rules Rule4 = new Rules();
    private Rules Rule5 = new Rules();
    private Rules Rule6 = new Rules();
    private Rules Rule7 = new Rules();
    private Rules Rule8 = new Rules();
    private Rules Rule9 = new Rules();
    private Rules Rule10 = new Rules();
    private Rules Rule11= new Rules();
    private Rules Rule12 = new Rules();





    private int w;
    private int h;
    private float ratio;
    private float ratioScale;

    private int CurrenFrame = 0;

    private int TotalAlive = 0;

    private int MaxAge = 0;


    public int RuleIndexStart;
    public int RuleIndexAge;
    public int RuleIndex3DNB;
    public int RuleIndexFrame;

    public int Changing3dNB;
    public int ChangeingAge;
    public int ChangingFrame;
    public int AgeLimit = 5;

    private void Awake()
    {
        Rule0.SetupRule(1,2,2,2);
        Rule1.SetupRule(1,2,3,3);
        Rule2 .SetupRule(1,2,3,4);
        Rule3.SetupRule(1, 3, 3, 3);
        Rule4.SetupRule(1, 3, 3, 6);
        Rule5.SetupRule(2, 3, 3, 3);
        Rule6.SetupRule(2, 3, 3, 4);
        Rule7.SetupRule(3, 3, 2, 2);
        Rule8.SetupRule(3, 4, 1, 1);
        Rule9.SetupRule(3, 4, 3, 4);
        Rule10.SetupRule(3, 6, 3, 3);
        Rule11.SetupRule(4, 5, 2, 2);
        Rule12.SetupRule(5, 5, 1, 1);


        ruleStore[0] = Rule0;
        ruleStore[1] = Rule1;
        ruleStore[2] = Rule2;
        ruleStore[3] = Rule3;
        ruleStore[4] = Rule4;
        ruleStore[5] = Rule5;
        ruleStore[6] = Rule6;
        ruleStore[7] = Rule7;
        ruleStore[8] = Rule8;
        ruleStore[9] = Rule9;
        ruleStore[10] = Rule10;
        ruleStore[11] = Rule11;
        ruleStore[12] = Rule12;



        RuleInUse = ruleStore[RuleIndexStart];

        GetXZ();
        SetPivot();
    }

    private void SetPivot()
    {
        pivot.transform.position = new Vector3(CountX / 2, HeightY / 3, CountZ / 2);
    }

    

    // Use this for initialization
    void Start ()
    {
       
        CreateGrid();
        print(CountZ);
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (CurrenFrame < HeightY-1)
	    {
	        CalculateOcta();
	        UpdateOcta();
	        SaveToUp();
	        DisplayModel();
	        CurrenFrame++;
	    }

	    CheckConnectionAll();

	}

    void GetXZ()
    {
         w = SeedImage.width;
         h = SeedImage.height;
         ratio = (float)h / w;
        ratioScale = w / CountX;
        CountZ = Mathf.FloorToInt(CountX * ratio);
        print(ratio);
        print(w);
        print(h);
    }

    void CreateGrid()
    {
        OctaX = new Transform[CountX, HeightY, CountZ];
        OctaY = new Transform[CountX, HeightY, CountZ];
        OctaZ = new Transform[CountX, HeightY, CountZ];


        for (int index = 0, y = 0; y < HeightY; y++)
        {
            for (int z = 0; z < CountZ; z++)
            {
                for (int x = 0; x < CountX; x++, index++)

                {
                    Transform ObjX = Instantiate(PrefabX, transform);
                    Transform ObjZ = Instantiate(PrefabZ, transform);
                    Transform ObjY = Instantiate(PrefabY, transform);

                    Vector3 CurrentPositionX = new Vector3(x, y, z);
                    Vector3 CurrentPositionY = new Vector3(x + Spaceing / 2, y + Spaceing / 2, z);
                    Vector3 CurrentPositionZ = new Vector3(x + Spaceing / 2, y, z + Spaceing / 2);

                    ObjX.position = CurrentPositionX;
                    ObjY.position = CurrentPositionY;
                    ObjZ.position = CurrentPositionZ;

                    ObjX.GetComponent<OctaVoxel>().SetupOcta(x, y, z);
                    ObjY.GetComponent<OctaVoxel>().SetupOcta(x, y, z);
                    ObjZ.GetComponent<OctaVoxel>().SetupOcta(x, y, z);

                    ObjX.name = "X" + ObjX.GetComponent<OctaVoxel>().Adress;

                    ObjY.name = "Y" + ObjY.GetComponent<OctaVoxel>().Adress;

                    ObjZ.name = "Z" + ObjZ.GetComponent<OctaVoxel>().Adress;

                    OctaX[x, y, z] = ObjX;
                    OctaY[x, y, z] = ObjY;
                    OctaZ[x, y, z] = ObjZ;



                    float Tx = SeedImage.GetPixel(Mathf.FloatToHalf(x * ratioScale), Mathf.FloatToHalf(z * ratioScale))
                        .grayscale;
                    //float Ty = Tx;
                    //float Tz = Tx;

                    float Ty = SeedImage.GetPixel(Mathf.FloatToHalf((x + Spaceing / 2) * ratioScale),
                        Mathf.FloatToHalf(z * ratioScale)).grayscale;
                    float Tz = SeedImage.GetPixel(Mathf.FloatToHalf((x + Spaceing / 2) * ratioScale),
                        Mathf.FloatToHalf((z + Spaceing / 2) * ratioScale)).grayscale;

                    if (y == 0)
                    {
                        if (Tx > GrayScaleDegree)
                        {
                            ObjX.GetComponent<OctaVoxel>().SetState(0);
                        }
                        else
                        {
                            ObjX.GetComponent<OctaVoxel>().SetState(1);
                        }

                        if (Ty > GrayScaleDegree)
                        {
                            ObjY.GetComponent<OctaVoxel>().SetState(0);
                        }
                        else
                        {
                            ObjY.GetComponent<OctaVoxel>().SetState(1);
                        }

                        if (Tz > GrayScaleDegree)
                        {
                            ObjZ.GetComponent<OctaVoxel>().SetState(0);
                        }
                        else
                        {
                            ObjZ.GetComponent<OctaVoxel>().SetState(1);
                        }
                    }
                }
            }
        }
    }

    void CalculateOcta()
    {
        for (int z = 1; z < CountZ - 1; z++)
        {
            for (int x = 1; x < CountX - 1; x++)
            {
                Transform CurrentObjX = OctaX[x, 0, z];
                Transform CurrentObjY = OctaY[x, 0, z];
                Transform CurrentObjZ = OctaZ[x, 0, z];

                //Neighbor Countt Of ObjXs
                int NX1 = OctaX[x - 1, 0, z].GetComponent<OctaVoxel>().GetState();
                int NX2 = OctaY[x , 0, z].GetComponent<OctaVoxel>().GetState();
                int NX3 = OctaX[x + 1, 0, z].GetComponent<OctaVoxel>().GetState();
                int NX4 = OctaY[x-1 , 0, z].GetComponent<OctaVoxel>().GetState();
                int NX5 = OctaZ[x - 1, 0, z - 1].GetComponent<OctaVoxel>().GetState();
                int NX6 = OctaZ[x - 1, 0, z].GetComponent<OctaVoxel>().GetState();
                int NX7 = OctaZ[x , 0, z].GetComponent<OctaVoxel>().GetState();
                int NX8 = OctaX[x , 0, z-1].GetComponent<OctaVoxel>().GetState();
               /* int NX1 = OctaX[x - 1, 0, z - 1].GetComponent<OctaVoxel>().GetState();
                int NX2 = OctaX[x - 1, 0, z].GetComponent<OctaVoxel>().GetState();
                int NX3 = OctaX[x - 1, 0, z + 1].GetComponent<OctaVoxel>().GetState();
                int NX4 = OctaX[x, 0, z - 1].GetComponent<OctaVoxel>().GetState();
                int NX5 = OctaX[x, 0, z + 1].GetComponent<OctaVoxel>().GetState();
                int NX6 = OctaX[x + 1, 0, z - 1].GetComponent<OctaVoxel>().GetState();
                int NX7 = OctaX[x + 1, 0, z].GetComponent<OctaVoxel>().GetState();
                int NX8 = OctaX[x + 1, 0, z + 1].GetComponent<OctaVoxel>().GetState();*/

                int NBCountX = NX1 + NX2 + NX3 + NX4 + NX5 + NX6 + NX7 + NX8;

                //Neighbor Count of ObjZs
                int NZ1 = OctaY[x , 0, z].GetComponent<OctaVoxel>().GetState();
                int NZ2 = OctaZ[x, 0, z + 1].GetComponent<OctaVoxel>().GetState();
                int NZ3 = OctaY[x, 0, z+1].GetComponent<OctaVoxel>().GetState();
                int NZ4 = OctaZ[x, 0, z - 1].GetComponent<OctaVoxel>().GetState();
                int NZ5 = OctaX[x, 0, z].GetComponent<OctaVoxel>().GetState();
                int NZ6 = OctaX[x, 0, z + 1].GetComponent<OctaVoxel>().GetState();
                int NZ7 = OctaX[x + 1, 0, z + 1].GetComponent<OctaVoxel>().GetState();
                int NZ8 = OctaX[x+1, 0, z].GetComponent<OctaVoxel>().GetState();
                /*
                int NZ1 = OctaZ[x - 1, 0, z - 1].GetComponent<OctaVoxel>().GetState();
                int NZ2 = OctaZ[x - 1, 0, z].GetComponent<OctaVoxel>().GetState();
                int NZ3 = OctaZ[x - 1, 0, z + 1].GetComponent<OctaVoxel>().GetState();
                int NZ4 = OctaZ[x, 0, z - 1].GetComponent<OctaVoxel>().GetState();
                int NZ5 = OctaZ[x, 0, z + 1].GetComponent<OctaVoxel>().GetState();
                int NZ6 = OctaZ[x + 1, 0, z - 1].GetComponent<OctaVoxel>().GetState();
                int NZ7 = OctaZ[x + 1, 0, z].GetComponent<OctaVoxel>().GetState();
                int NZ8 = OctaZ[x + 1, 0, z + 1].GetComponent<OctaVoxel>().GetState();
                */

                int NBCountZ = NZ1 + NZ2 + NZ3 + NZ4 + NZ5 + NZ6 + NZ7 + NZ8;

                //Neighbor Count of ObjYs

                int NY1 = OctaY[x - 1, 0, z ].GetComponent<OctaVoxel>().GetState();
                int NY2 = OctaY[x , 0, z-1].GetComponent<OctaVoxel>().GetState();
                int NY3 = OctaY[x , 0, z + 1].GetComponent<OctaVoxel>().GetState();
                int NY4 = OctaY[x +1, 0, z ].GetComponent<OctaVoxel>().GetState();
                int NY5 = OctaX[x , 0, z ].GetComponent<OctaVoxel>().GetState();
                int NY6 = OctaX[x + 1, 0, z ].GetComponent<OctaVoxel>().GetState();
                int NY7 = OctaZ[x , 0, z].GetComponent<OctaVoxel>().GetState();
                int NY8 = OctaZ[x , 0, z - 1].GetComponent<OctaVoxel>().GetState();


                int NBCountY = NY1 + NY2 + NY3 + NY4 + NY5 + NY6 + NY7 + NY8;

                int NBCountAll = NBCountX + NBCountY + NBCountZ;

                int currentAgeX = CurrentObjX.GetComponent<OctaVoxel>().GetAge();
                int currentAgeY = CurrentObjY.GetComponent<OctaVoxel>().GetAge();
                int currentAgeZ = CurrentObjZ.GetComponent<OctaVoxel>().GetAge();

              
                RuleInUse = ruleStore[RuleIndexStart];

                /*if (currentAgeX > ChangeingAge || currentAgeY > ChangeingAge || currentAgeZ > ChangeingAge)
                {
                    RuleInUse = ruleStore[RuleIndexAge];
                }

                if (CurrenFrame > 0)
                {
                    int NX3d = Check3DNeighborX(CurrentObjX, x, CurrenFrame, z);
                    int NY3d = Check3DNeighborY(CurrentObjY, x, CurrenFrame, z);
                    int NZ3d = Check3DNeighborZ(CurrentObjZ, x, CurrenFrame, z);

                    CurrentObjX.GetComponent<OctaVoxel>().Set3DNeighbor(NX3d);
                    CurrentObjY.GetComponent<OctaVoxel>().Set3DNeighbor(NY3d);
                    CurrentObjZ.GetComponent<OctaVoxel>().Set3DNeighbor(NZ3d);

                    if (NX3d < Changing3dNB || NY3d < Changing3dNB || NZ3d < Changing3dNB)
                    {
                        RuleInUse = ruleStore[RuleIndex3DNB];
                    }
                }

                if (CurrenFrame > ChangingFrame)
                {
                    RuleInUse = ruleStore[RuleIndexFrame];
                }*/


                CheckNeighborToState(CurrentObjX, NBCountX , AgeLimit, RuleInUse);
                CheckNeighborToState(CurrentObjY, NBCountY , AgeLimit, RuleInUse);
                CheckNeighborToState(CurrentObjZ, NBCountZ , AgeLimit, RuleInUse);
            }
        }
    }

    void UpdateOcta()
    {
        for (int z = 0; z < CountZ ; z++)
        {
            for (int x = 0; x < CountX ; x++)
            {
                Transform CurrentObjX = OctaX[x, 0, z];
                Transform CurrentObjY = OctaY[x, 0, z];
                Transform CurrentObjZ = OctaZ[x, 0, z];

                CurrentObjX.GetComponent<OctaVoxel>().UpdateOctaVoxel();
                CurrentObjY.GetComponent<OctaVoxel>().UpdateOctaVoxel();
                CurrentObjZ.GetComponent<OctaVoxel>().UpdateOctaVoxel();
            }
        }
    }

    void SaveToUp()
    {
        TotalAlive = 0;
        for (int z = 0; z < CountZ; z++)
        {
            for (int x = 0; x < CountX; x++)
            {
                Transform CurrentObjX = OctaX[x, 0, z];
                Transform CurrentObjY = OctaY[x, 0, z];
                Transform CurrentObjZ = OctaZ[x, 0, z];

                int StateX = CurrentObjX.GetComponent<OctaVoxel>().GetState();
                int StateY = CurrentObjY.GetComponent<OctaVoxel>().GetState();
                int StateZ = CurrentObjZ.GetComponent<OctaVoxel>().GetState();

                Transform SavedOctaX = OctaX[x, CurrenFrame, z];
                Transform SavedOctaY = OctaY[x, CurrenFrame, z];
                Transform SavedOctaZ = OctaZ[x, CurrenFrame, z];

                SavedOctaX.GetComponent<OctaVoxel>().SetState(StateX);
                SavedOctaY.GetComponent<OctaVoxel>().SetState(StateY);
                SavedOctaZ.GetComponent<OctaVoxel>().SetState(StateZ);
                if (StateX == 1)
                {
                    int CurrentAgeX = CurrentObjX.GetComponent<OctaVoxel>().GetAge();
                    int Current3dNB= CurrentObjX.GetComponent<OctaVoxel>().Get3DNeighbor();
                    SavedOctaX.GetComponent<OctaVoxel>().SetAge(CurrentAgeX);
                    SavedOctaX.GetComponent<OctaVoxel>().Set3DNeighbor( Current3dNB);

                    //print(Current3dNB);

                    if (CurrentAgeX > MaxAge)
                    {
                        MaxAge = CurrentAgeX;
                    }
                }
                if (StateY == 1)
                {
                    int CurrentAgeY = CurrentObjY.GetComponent<OctaVoxel>().GetAge();
                    int Current3dNB = CurrentObjY.GetComponent<OctaVoxel>().Get3DNeighbor();
                    SavedOctaY.GetComponent<OctaVoxel>().SetAge(CurrentAgeY);
                    SavedOctaY.GetComponent<OctaVoxel>().Set3DNeighbor( Current3dNB);
                    //print(Current3dNB);

                    if (CurrentAgeY > MaxAge)
                    {
                        MaxAge = CurrentAgeY;
                    }
                }
                if (StateZ == 1)
                {
                    int CurrentAgeZ = CurrentObjZ.GetComponent<OctaVoxel>().GetAge();
                    int Current3dNB = CurrentObjZ.GetComponent<OctaVoxel>().Get3DNeighbor();
                    SavedOctaZ.GetComponent<OctaVoxel>().SetAge(CurrentAgeZ);
                    SavedOctaZ.GetComponent<OctaVoxel>().Set3DNeighbor( Current3dNB );
                    //print(Current3dNB);
                    if (CurrentAgeZ > MaxAge)
                    {
                        MaxAge = CurrentAgeZ;
                    }
                }
            }
        }
    }

    void DisplayModel()
    {
        for (int z = 0; z < CountZ; z++)
        {
            for (int x = 0; x < CountX; x++)
            {
                if (CurrenFrame > 0)
                {
                    OctaX[x, CurrenFrame, z].GetComponent<OctaVoxel>().DisplayOcta(Color.black*0.7f);
                    OctaY[x, CurrenFrame, z].GetComponent<OctaVoxel>().DisplayOcta(Color.gray );
                    OctaZ[x, CurrenFrame, z].GetComponent<OctaVoxel>().DisplayOcta(Color.white );
                }
            }
        }
    }

    void CheckNeighborToState(Transform _Obj,int _NBCount ,int _age, Rules _rule)
    {
        int _inst0 = _rule.GetInst(0);
        int _inst1 = _rule.GetInst(1);
        int _inst2 = _rule.GetInst(2);
        int _inst3 = _rule.GetInst(3);

        //If Is Alive
        if (_Obj.GetComponent<OctaVoxel>().GetState() == 1)
        {
            if (_NBCount < _inst0)
            {
                _Obj.GetComponent<OctaVoxel>().SetFutureState(0);
            }

            if (_NBCount >= _inst0 && _NBCount <= _inst1)
            {
                _Obj.GetComponent<OctaVoxel>().SetFutureState(1);
            }

            if (_NBCount > _inst1)
            {
                _Obj.GetComponent<OctaVoxel>().SetFutureState(0);
            }

            if (_Obj.GetComponent<OctaVoxel>().Get3DNeighbor() < 1)
            {
                _Obj.GetComponent<OctaVoxel>().SetFutureState(0);
            }
        }

        //If Is Dead
        if (_Obj.GetComponent<OctaVoxel>().GetState() == 0)
        {
            if (_NBCount >= _inst2 && _NBCount <= _inst3)
            {
                _Obj.GetComponent<OctaVoxel>().SetFutureState(1);
            }

            if (_Obj.GetComponent<OctaVoxel>().Get3DNeighbor() > 2)
            {
                _Obj.GetComponent<OctaVoxel>().SetFutureState(1);
            }
        }

        if (_Obj.GetComponent<OctaVoxel>().GetAge() > _age)
        {
            _Obj.GetComponent<OctaVoxel>().SetFutureState(0);
        }

       
    }

    int Check3DNeighborX(Transform _CurrentOcta,int x,int y, int z)
    {
        int Neighbor3d = 0;
        if (y > 0)
        {
            int N1 = OctaY[x, y - 1, z].GetComponent<OctaVoxel>().GetState();
            int N2 = OctaY[x, y, z].GetComponent<OctaVoxel>().GetState();
            int N3 = OctaY[x - 1, y - 1, z].GetComponent<OctaVoxel>().GetState();
            int N4 = OctaY[x - 1, y, z].GetComponent<OctaVoxel>().GetState();
            int N5 = OctaZ[x - 1, y, z - 1].GetComponent<OctaVoxel>().GetState();
            int N6 = OctaZ[x - 1, y, z].GetComponent<OctaVoxel>().GetState();
            int N7 = OctaZ[x, y, z].GetComponent<OctaVoxel>().GetState();
            int N8 = OctaZ[x, y, z - 1].GetComponent<OctaVoxel>().GetState();

             Neighbor3d = N1 + N2 + N3 + N4 + N5 + N6 + N7 + N8;
            _CurrentOcta.GetComponent<OctaVoxel>().Set3DNeighbor(Neighbor3d);
        }
        return Neighbor3d;
    }

    int Check3DNeighborZ(Transform _CurrentOcta, int x, int y, int z)
    {
        int Neighbor3d = 0;
        if (y > 0)
        {
            int N1 = OctaY[x, y , z].GetComponent<OctaVoxel>().GetState();
            int N2 = OctaY[x, y-1, z].GetComponent<OctaVoxel>().GetState();
            int N3 = OctaY[x , y , z+1].GetComponent<OctaVoxel>().GetState();
            int N4 = OctaY[x , y-1, z+1].GetComponent<OctaVoxel>().GetState();
            int N5 = OctaX[x , y, z ].GetComponent<OctaVoxel>().GetState();
            int N6 = OctaX[x , y, z+1].GetComponent<OctaVoxel>().GetState();
            int N7 = OctaX[x+1, y, z+1].GetComponent<OctaVoxel>().GetState();
            int N8 = OctaX[x+1, y, z ].GetComponent<OctaVoxel>().GetState();

            Neighbor3d = N1 + N2 + N3 + N4 + N5 + N6 + N7 + N8;
            _CurrentOcta.GetComponent<OctaVoxel>().Set3DNeighbor(Neighbor3d);
        }
        return Neighbor3d;
    }
    int Check3DNeighborY(Transform _CurrentOcta, int x, int y, int z)
    {
        int Neighbor3d = 0;
        if (y > 0)
        {
            int N1 = OctaX[x, y, z].GetComponent<OctaVoxel>().GetState();
            int N2 = OctaX[x, y + 1, z].GetComponent<OctaVoxel>().GetState();
            int N3 = OctaX[x + 1, y, z].GetComponent<OctaVoxel>().GetState();
            int N4 = OctaX[x + 1, y + 1, z].GetComponent<OctaVoxel>().GetState();
            int N5 = OctaZ[x, y, z].GetComponent<OctaVoxel>().GetState();
            int N6 = OctaZ[x, y + 1, z].GetComponent<OctaVoxel>().GetState();
            int N7 = OctaZ[x, y, z - 1].GetComponent<OctaVoxel>().GetState();
            int N8 = OctaZ[x, y + 1, z - 1].GetComponent<OctaVoxel>().GetState();

            Neighbor3d = N1 + N2 + N3 + N4 + N5 + N6 + N7 + N8;
            _CurrentOcta.GetComponent<OctaVoxel>().Set3DNeighbor(Neighbor3d);
        }
        return Neighbor3d;
    }

    void CheckConnectionAll()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            for (int y = 1; y < HeightY-1; y++)
            {
                for (int z = 1; z < CountZ-1; z++)
                {
                    for (int x = 1; x < CountX-1; x++)
                    {
                        
                        int nb3dx = Check3DNeighborX(OctaX[x, y, z], x, y, z);
                        int nb3dy = Check3DNeighborY(OctaY[x, y, z], x, y, z);
                        int nb3dz = Check3DNeighborZ(OctaZ[x, y, z], x, y, z);

                        CheckConnectionSingle(OctaX[x, y, z],nb3dx, Color. black  );
                        CheckConnectionSingle(OctaY[x, y, z],nb3dy,Color.gray );
                        CheckConnectionSingle(OctaZ[x, y, z],nb3dz,Color.white  );
                    }
                }
            }
        }
    }

    void CheckConnectionSingle(Transform _obj,int nb3d,Color color)
    {
        int state = _obj.GetComponent<OctaVoxel>().GetState();
         _obj.GetComponent<OctaVoxel>().Set3DNeighbor( nb3d );

        if (state == 1)
        {
            if (nb3d == 0)
            {
                _obj.GetComponent<OctaVoxel>().SetState(0);
                _obj.GetComponent<OctaVoxel>().DisplayOcta();
            }
        }
        if (state == 0)
        {
            if (nb3d >= 3&&nb3d<=3)
            {
                _obj.GetComponent<OctaVoxel>().SetState(1);
                _obj.GetComponent<OctaVoxel>().DisplayOcta(color);
            }
        }
    }
}



