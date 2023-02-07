using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPoint : MonoBehaviour
{
    public enum BuildMode
    {
        Automatic,
        Manual
    };

    public Recipie recipie;
    public BuildMode buildMode;

    // variables for placing
    private int currentStep;
    private GameObject currentGhost;
    private bool completed;
    private List<GameObject> assembledComponents;
    private List<GameObject> disassembledComponents;

    private void OnEnable()
    {
        completed = false;
        currentStep = -1;
        assembledComponents = new List<GameObject>();
        disassembledComponents = new List<GameObject>();
    }
    void Start()
    {
        AnimateNextStep();
    }

  

    public void ShowStep(int stepNum)
    {
        if (completed) return;
        // set current ghost to become solid
        if (currentGhost != null)
        {
            // configure material to be opaque
            Material mat = currentGhost.transform.GetComponentInChildren<MeshRenderer>().material;
            currentGhost.transform.GetComponentInChildren<MeshRenderer>().material = ConvertToOpaque(mat);
            Destroy(currentGhost.transform.GetChild(0).gameObject.GetComponent<CollisionDetection>());

            assembledComponents.Add(currentGhost);
            currentGhost = null;
        }

        // generate next ghost
        if (stepNum < recipie.NumComponents())
        {
            RecipieComponent newComponent = recipie.GetRecipieComponent(stepNum);
            currentGhost = Instantiate(newComponent.GetModel(), transform);
            currentGhost.transform.localPosition = newComponent.GetAssembledPosition();
            currentGhost.transform.localRotation = Quaternion.Euler(newComponent.GetAssembledRotation());

            // configure material to be semi-transparent
            Material materialOverride = new Material(newComponent.GetMaterial());
            Material mat = materialOverride != null ? materialOverride : currentGhost.transform.GetComponentInChildren<MeshRenderer>().material;
            currentGhost.transform.GetComponentInChildren<MeshRenderer>().material = ConvertToTransparent(mat);

            // add trigger collider if on manual mode
            if (buildMode == BuildMode.Manual)
            {
                MeshCollider trigger = currentGhost.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
                trigger.convex = true;
                trigger.isTrigger = true;
                currentGhost.transform.GetChild(0).gameObject.AddComponent<CollisionDetection>();
            }
        }
        else
        {
            completed = true;
        }

    }

    public void AnimateNextStep()
    {
        if (currentStep == -1)
        {
            SpawnDisassembled();
            currentStep = 0;
            ShowStep(0);
        }
        else if (!completed)
        {
            RecipieComponent rc = recipie.components[currentStep];
            GameObject component = disassembledComponents[0];
            disassembledComponents.RemoveAt(0);
            StartCoroutine(AnimateComponent(component, ++currentStep, rc.GetDisassembledPosition(), rc.GetDisassembledRotation(), rc.GetAssembledPosition(), rc.GetAssembledRotation(), 0, 100));
            if (currentStep > recipie.NumComponents())
            {
                completed = true;
            }
        }
    }

    public void SpawnDisassembled()
    {
        foreach (RecipieComponent rc in recipie.components)
        {
            GameObject comp = Instantiate(rc.GetModel(), transform);
            comp.transform.localPosition = rc.GetDisassembledPosition();
            comp.transform.localRotation = Quaternion.Euler(rc.GetDisassembledRotation());
            comp.transform.GetChild(0).GetComponent<MeshRenderer>().material = rc.GetMaterial();
            if (buildMode == BuildMode.Manual)
            {
                comp.layer = 7;
                comp.transform.GetChild(0).gameObject.layer = 7;
                comp.tag = "pickup";
                MeshCollider mc = comp.transform.GetChild(0).gameObject.AddComponent<MeshCollider>();
                mc.convex = true;
                Rigidbody rb = comp.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
                rb.isKinematic = true;
            }
            disassembledComponents.Add(comp);
        }
    }

    IEnumerator AnimateComponent(GameObject component, int stepNumber, Vector3 startPosition, Vector3 startRotation, Vector3 endPosition, Vector3 endRotation, int iteration, int maxIterations)
    {
        if (iteration > maxIterations)
        {
            Destroy(component);
            ShowStep(stepNumber);
            yield break;
        }

        Vector3 diffPos = endPosition - startPosition;
        component.transform.localPosition = (float)iteration / maxIterations * diffPos + startPosition;
        Vector3 diffRot = endRotation - startRotation;
        component.transform.localRotation = Quaternion.Euler((float)iteration / maxIterations * diffRot + startRotation);

        iteration++;
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(AnimateComponent(component, stepNumber, startPosition, startRotation, endPosition, endRotation, iteration, maxIterations));
    }

    public void PartCollision(GameObject target)
    {
        if (!completed && target == disassembledComponents[currentStep])
        {
            ShowStep(++currentStep);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<BuildController>().PlacedItem();
            Destroy(target);
        }
    }


    private Material ConvertToTransparent(Material mat)
    {
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        /*
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        */
        mat.EnableKeyword("_ALPHA_ON");
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.renderQueue = 3000;
        
        Color c = mat.color;
        c.a = 0.25f;
        mat.color = c;
        
        return mat;
    }

    private Material ConvertToOpaque(Material mat)
    {
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = -1;
        Color c = mat.color;
        c.a = 1f;
        mat.color = c;
        return mat;
    }
}
