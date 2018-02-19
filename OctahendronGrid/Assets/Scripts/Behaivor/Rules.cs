using UnityEngine;
using UnityEditor;

public class Rules
{
    public int[] Inst = new int[4];

    public void SetupRule(int Inst0, int inst1, int inst2, int inst3)
    {
        Inst[0] = Inst0;
        Inst[1] = inst1;
        Inst[2] = inst2;
        Inst[3] = inst3;
    }
    public int GetInst(int _index)
    {
        return Inst[_index];
    }

    public void SetInst(int _inst, int _index)
    {
        Inst[_index] = _inst;
    }

    public int[] GetInstAry()
    {
        return Inst;
    }
}