using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public enum StructureType
{
    road,
    building,
    wheat,
    melon,
    corn,
    apple
}
 
public abstract class Structure : MonoBehaviour
{
    public StructureType type;
    public bool functional = false;
 
    public string structureName;
    public int hp = 1;
    public int costToBuild;
}