using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipieComponent : ScriptableObject
{
    public string _name;
    public GameObject model;
    public Material material;
    [Header("Assembled Configuration")]
    public Vector3 assembledPosition;
    public Vector3 assembledRotation;
    [Header("Disassembled Configuration")]
    public Vector3 disassembledPosition;
    public Vector3 disassembledRotation;

    public RecipieComponent(string name, GameObject obj, Material mat, Vector3 assembledPos, Vector3 assembledRot, Vector3 disassembledPos, Vector3 disassembledRot)
    {
        _name = name;
        model = obj;
        material = mat;
        assembledPosition = assembledPos;
        assembledRotation = assembledRot;
        disassembledPosition = disassembledPos;
        disassembledRotation = disassembledRot;
    }

    public RecipieComponent()
    {
        _name = "";
        model = null;
        material = null;
        assembledPosition = Vector3.zero;
        assembledRotation = Vector3.zero;
        disassembledPosition = Vector3.zero;
        disassembledRotation = Vector3.zero;
    }

    public string GetName()
    {
        return _name;
    }

    public GameObject GetModel()
    {
        return model;
    }

    public Material GetMaterial()
    {
        return material;
    }

    public Vector3 GetAssembledPosition()
    {
        return assembledPosition;
    }

    public Vector3 GetAssembledRotation()
    {
        return assembledRotation;
    }

    public Vector3 GetDisassembledPosition()
    {
        return disassembledPosition;
    }

    public Vector3 GetDisassembledRotation()
    {
        return disassembledRotation;
    }

}
