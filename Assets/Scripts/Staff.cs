using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

 

public enum UnitState
{
    Idle,
    Walk,
    Plow,
    Sow,
    Water,
    Harvest
}

 

public class Staff : MonoBehaviour
{
    private int _id;
    public int ID { get { return _id; } set { ID = value; } }

 

    private int _charSkinId;
    public int CharSkinID { get { return _charSkinId; } set { _charSkinId = value; } }
    public GameObject[] charSkin;

 

    public string staffName;
    public int dailyWage;

 

    //Animation State
    [SerializeField] private UnitState state;
    public UnitState State { get { return state; } set { state = value; } }

 

    //Navmesh Agent
    private NavMeshAgent navAgent;
    public NavMeshAgent NavAgent { get { return navAgent; } set { navAgent = value; } }

 

    [SerializeField] private GameObject workPlace;
    public GameObject WorkPlace { get { return workPlace; } set { workPlace = value; } }

 

    private float CheckStatusTime = 0.5f;

 

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

 

    void Update()
    {
        SwitchStaffState();
    }

 

    private void SwitchStaffState()
    {
        switch (state)
        {
            case UnitState.Walk:
                CheckStop();
                break;
        }

 

    }

 

    public void InitCharID(int id)
    {
        _id = id;
        _charSkinId = Random.Range(0, charSkin.Length - 1);
        staffName = "XXXX";
        dailyWage = Random.Range(80, 125);
    }

 

    public void ChangeCharSkin()
    {
        for (int i = 0; i < charSkin.Length; i++)
        {
            if (i == _charSkinId)
                charSkin[i].SetActive(true);
            else
                charSkin[i].SetActive(false);
        }
    }

 

    public void SetToWalk(Vector3 dest)
    {
        state = UnitState.Walk;

 

        navAgent.SetDestination(dest);
        navAgent.isStopped = false;
    }

 

    private void CheckStop()
    {
        float dist = Vector3.Distance(transform.position, navAgent.destination);

 

        if (dist <= 3f)
        {
            state = UnitState.Idle;
            navAgent.isStopped = true;
        }
    }

 

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject != workPlace)
            return;

 

        Farm farm = other.gameObject.GetComponent<Farm>();

 

        if (farm != null && farm.hp < 100 && navAgent.isStopped == true)
        {
            switch(farm.stage)
            {
                case FarmStage.plowing:
                    state = UnitState.Plow;
                    Debug.Log("Plowing");
                    farm.CheckTimeForWork();
                    break;

 

                case FarmStage.sowing:
                    state = UnitState.Sow;
                    farm.CheckTimeForWork();
                    break;

 

                case FarmStage.maintaining:
                    state = UnitState.Water;
                    farm.CheckTimeForWork();
                    break;

 

                case FarmStage.harvesting:
                    state = UnitState.Harvest;
                    farm.CheckTimeForWork();
                    break;
            }
        }
    }
}
