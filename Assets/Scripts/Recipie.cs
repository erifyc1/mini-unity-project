using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Recipie : ScriptableObject
{
    public string _name;
    public List<RecipieComponent> components;

    public Recipie()
    {
        _name = "default";
        components = new List<RecipieComponent>();
    }

    public void AddComponent(RecipieComponent r)
    {
        components.Add(r);
    }

    public RecipieComponent GetRecipieComponent(int idx)
    {
        if (idx < components.Count)
        {
            return components[idx];
        }
        return new RecipieComponent();
    }

    public int NumComponents()
    {
        return components.Count;
    }
    
    public string GetName()
    {
        return _name;
    }

    
}
        // assembled (0 rot)
        // -41.5 31 24
        //    0   0 63
        // -41.5 31 87
        //    0   0 126

        // disassembled (-90 x rot)
        // 115 46 400
        // 69.6 38.6 497.3
        // 0 46 430
        // 69.1 38.6 257.1
        // 0 46 308.5
