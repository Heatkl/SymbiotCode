using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbioteBehaviour : MonoBehaviour
{
    [Header("Animation")]
    public GameObject legPrefab;

    [Range(1, 20)]
    public int numberOfLegs = 5;
    [Tooltip("The number of splines per leg")]
    [Range(1, 10)]
    public int partsPerLeg = 4;
    protected int maxLegs;

    public int legCount;
    public int deployedLegs;
    [Range(0, 19)]
    public int minimumAnchoredLegs = 2;
    public int minimumAnchoredParts;

    [Tooltip("Minimum duration before leg is replaced")]
    public float minLegLifetime = 5;
    [Tooltip("Maximum duration before leg is replaced")]
    public float maxLegLifetime = 15;

    public Vector3 legPlacerOrigin = Vector3.zero;
    [Tooltip("Leg placement radius offset")]
    public float newLegRadius = 3;

    public float minLegDistance = 4.5f;
    public float maxLegDistance = 6.3f;

    [Range(2, 50)]
    [Tooltip("Number of spline samples per legpart")]
    public int legResolution = 40;

    [Tooltip("Minimum lerp coeficient for leg growth smoothing")]
    public float minGrowCoef = 4.5f;
    [Tooltip("MAximum lerp coeficient for leg growth smoothing")]
    public float maxGrowCoef = 6.5f;

    [Tooltip("Minimum duration before a new leg can be placed")]
    public float newLegCooldown = 0.3f;

    protected bool canCreateLeg = true;

    protected List<GameObject> legs = new();
    protected List<GameObject> availableLegPool = new List<GameObject>();

    [Tooltip("This must be updates as the Mimin moves to assure great leg placement")]
    public Vector3 velocity;


    protected void ResetMimic()
    {
        foreach (GameObject g in legs)
        {
            Destroy(g);
        }
        legCount = 0;
        deployedLegs = 0;

        maxLegs = numberOfLegs * partsPerLeg;
        float rot = 360f / maxLegs;
        Vector2 randV = Random.insideUnitCircle;
        velocity = new Vector3(randV.x, 0, randV.y);
        minimumAnchoredParts = minimumAnchoredLegs * partsPerLeg;
        maxLegDistance = newLegRadius * 2.1f;

    }

    protected IEnumerator NewLegCooldown()
    {
        canCreateLeg = false;
        yield return new WaitForSeconds(newLegCooldown);
        canCreateLeg = true;
    }

    //public void RecycleLeg(GameObject leg)
    //{
    //    availableLegPool.Add(leg);
    //    leg.SetActive(false);
    //}

    public virtual void RecycleLeg(GameObject leg)
    {

    }
}
