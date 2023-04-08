using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    public Staff staff;

    public void ButtonIdle()
    {
        staff.State = UnitState.Idle;
    }

    public void ButtonWalk()
    {
        staff.State = UnitState.Walk;
    }

    public void ButtonPlow()
    {
        staff.State = UnitState.Plow;
    }
}