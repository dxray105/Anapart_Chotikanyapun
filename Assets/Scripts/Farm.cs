using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 

public enum FarmStage
{
    plowing,
    sowing,
    maintaining,
    harvesting
}

 

public class Farm : Structure
{
    public FarmStage stage = FarmStage.plowing;

 

    [SerializeField] private int _maxStaffNum = 3;
    public int MaxStaffNum { get { return _maxStaffNum; } set { _maxStaffNum = value; } }

 

    public int dayRequired; //Days required until cultivation
    public int dayPassed; //Day passed since last cultivation
    public float produceTimer = 0f; //Timer in seconds for converting to dayPassed
    private int secondsPerDay = 10; //Number of seconds equals to a day in game

 

    public int cultivateDuration;

 

    public float WorkTimer = 0f; //Timer for working to increase progress in each stage
    private float WorkTimeWait = 1f;

 

    public GameObject FarmUI;

 

    [SerializeField] private List<Staff> _workingStaff;
    public List<Staff> WorkingStaff { get { return _workingStaff; } set { _workingStaff = value; } }

 

    void Update()
    {
        //ShowUIText();

 

        CheckPlowing();
        CheckSowing();
        CheckMaintaining();
        CheckHarvesting();
    }

 

    private void Working()
    {
        hp += 3;
    }

 

    public void CheckTimeForWork()
    {
        WorkTimer += Time.deltaTime;

 

        if (WorkTimer > WorkTimeWait)
        {
            WorkTimer = 0;
            Working();
        }
    }

 

    private void CheckPlowing()
    {
        if ((hp >= 100) && (stage == FarmStage.plowing))
        {
            stage = FarmStage.sowing;
            hp = 1;
        }
    }

 

    private void CheckSowing()
    {
        if ((hp >= 100) && (stage == FarmStage.sowing))
        {
            functional = true; //Plant will automatically grows
            stage = FarmStage.maintaining;
            hp = 1;
        }
    }

 

    public void CheckMaintaining()
    {
        if ((hp >= 100) && (stage == FarmStage.maintaining))
        {
            produceTimer += Time.deltaTime;
            dayPassed = Mathf.CeilToInt(produceTimer / secondsPerDay);

 

            if ((functional == true) && (dayPassed >= dayRequired))
            {
                stage = FarmStage.harvesting;
                hp = 1;
                produceTimer = 0;
            }
        }
    }

 

    private void CheckHarvesting()
    {
        if ((hp >= 100) && (stage == FarmStage.harvesting))
        {
            HarvestResult();
            hp = 1;
            stage = FarmStage.sowing;
        }
    }

 

    private void HarvestResult()
    {
        switch (type)
        {
            case StructureType.wheat:
            {
                GameManager.instance.wheat += 1000;
                break;
            }
            case StructureType.melon:
            {
                GameManager.instance.melon += 1000;
                break;
            }
            case StructureType.corn:
            {
                GameManager.instance.corn += 1000;
                break;
            }
            case StructureType.apple:
            {
                GameManager.instance.apple += 1000;
                break;
            }
        }

 

        UI.instance.UpdateHeaderPanel();
    }

 

    public void AddStaffToFarm(Staff staff)
    {
        _workingStaff.Add(staff);
    }
}