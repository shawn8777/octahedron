using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctaVoxel : MonoBehaviour {


    int State=0;
    int FutureState=0;
    int Age=0;
    int Neighbor3D;

    MeshRenderer Renderer;

    public Vector3 Adress;



    public void SetupOcta(int x,int y, int z)
    {
        Renderer = gameObject .GetComponent<MeshRenderer>();
        Renderer.enabled = false;
        Adress = new Vector3(x, y, z);
    }

    public void SetState(int _State)
    {
        State = _State;
    }

    public int GetState()
    {
        return State;
    }

    public void SetFutureState(int _FutureState)
    {
        FutureState = _FutureState;
    }

    public int GetFutureState()
    {
        return FutureState;
    }

    public void UpdateOctaVoxel()
    {
        State = FutureState;

        if (State == 1)
        {
            Age++;
        }
        if (State == 0)
        {
            Age = 0;
        }
    }

    public void SetAge(int _age)
    {
        Age = _age;
    }
    public int GetAge()
    {
        return Age;
    }

    public void Set3DNeighbor(int _3DNBCount)
    {
        Neighbor3D = _3DNBCount;
    }

    public int Get3DNeighbor()
    {
        return Neighbor3D;
    }


    public void DisplayOcta()
    {
        if (State == 1)
        {
            Renderer.enabled = true;
            Renderer.material.color = Color.gray;
        }
        if(State == 0)
        {
            Renderer.enabled = false;
        }
    }
    public void DisplayOcta(Color _color)
    {
        if (State == 1)
        {
            Renderer.enabled = true;
            Renderer.material.color = _color;
        }
        if (State == 0)
        {
            Renderer.enabled = false;
        }
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
