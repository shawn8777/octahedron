using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TJoint : MonoBehaviour
{
    private int index;
    private int BarIndex;
    private int StartEndIndex;

    public void SetIndex(int _index)
    {
        index = _index;
    }

    public void SetIndex2d(int _Bi, int _Si)
    {
        BarIndex = -_Bi;
        StartEndIndex = _Si;
    }

    public int GetIndex()
    {
        return index;
    }

    public int GetBarIndex()
    {
        return BarIndex;
    }

    public int GetStartEndIndex()
    {
        return StartEndIndex;
    }
}
