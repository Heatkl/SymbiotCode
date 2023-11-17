using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbioteMotion : SymbioteBehaviour
{
    void Start()
    {
        ResetMimic();
    }

    private void OnValidate()
    {
        ResetMimic();
    }

    void Update()
    {
        if (!canCreateLeg)
            return;

        // New leg origin is placed in front of the mimic
        legPlacerOrigin = transform.position + velocity.normalized * newLegRadius;

        if (legCount <= maxLegs - partsPerLeg)
        {
            // Offset The leg origin by a random vector
            Vector2 offset = Random.insideUnitCircle * newLegRadius;
            Vector3 newLegPosition = legPlacerOrigin + new Vector3(offset.x, 0, offset.y);

            // If the mimic is moving and the new leg position is behind it, mirror it to make
            // it reach in front of the mimic.
            if (velocity.magnitude > 1f)
            {
                float newLegAngle = Vector3.Angle(velocity, newLegPosition - transform.position);

                if (Mathf.Abs(newLegAngle) > 90)
                {
                    newLegPosition = transform.position - (newLegPosition - transform.position);
                }
            }

            if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(legPlacerOrigin.x, 0, legPlacerOrigin.z)) < minLegDistance)
                newLegPosition = ((newLegPosition - transform.position).normalized * minLegDistance) + transform.position;

            // if the angle is too big, adjust the new leg position towards the velocity vector
            if (Vector3.Angle(velocity, newLegPosition - transform.position) > 45)
                newLegPosition = transform.position + ((newLegPosition - transform.position) + velocity.normalized * (newLegPosition - transform.position).magnitude) / 2f;

            RaycastHit hit;
            Physics.Raycast(newLegPosition + Vector3.up * 10f, -Vector3.up, out hit);
            Vector3 myHit = hit.point;
            if (Physics.Linecast(transform.position, hit.point, out hit))
                myHit = hit.point;

            float lifeTime = Random.Range(minLegLifetime, maxLegLifetime);

            StartCoroutine(NewLegCooldown());
            for (int i = 0; i < partsPerLeg; i++)
            {
                RequestLeg(myHit, legResolution, maxLegDistance, Random.Range(minGrowCoef, maxGrowCoef), this, lifeTime);
                if (legCount >= maxLegs)
                    return;
            }
        }
    }

    void RequestLeg(Vector3 footPosition, int legResolution, float maxLegDistance, float growCoef, SymbioteBehaviour myMimic, float lifeTime)
    {
        GameObject newLeg;
        if (availableLegPool.Count > 0)
        {
            newLeg = availableLegPool[availableLegPool.Count - 1];
            availableLegPool.RemoveAt(availableLegPool.Count - 1);
        }
        else
        {
            newLeg = Instantiate(legPrefab, transform.position, Quaternion.identity);
            legs.Add(newLeg);
        }
        newLeg.SetActive(true);
        newLeg.GetComponent<Leg>().Initialize(footPosition, legResolution, maxLegDistance, growCoef, myMimic, lifeTime);
        newLeg.transform.SetParent(myMimic.transform);
    }

    public override void RecycleLeg(GameObject leg)
    {
        availableLegPool.Add(leg);
        leg.SetActive(false);
    }
}
