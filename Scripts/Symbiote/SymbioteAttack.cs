using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbioteAttack : SymbioteBehaviour
{
    readonly List<NPCBehaviour> npc = new();
    Dictionary<NPCBehaviour, List<GameObject>> legOnNpc = new();

    private void Start()
    {
        maxLegs = 5; //GameManager.instance.progress.mainTentaclesQuantity;
        numberOfLegs = maxLegs;
        partsPerLeg = 5;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            npc.Add(other.gameObject.GetComponent<NPCBehaviour>());
            if ((availableLegPool.Count > 0 || legs.Count < maxLegs) && npc[^1].multiplicator < partsPerLeg)
            {
                //npc[^1].GetDamage(1);
                //SetLeg(other.transform);
                CheckAttackNPC();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var _npc = other.gameObject.GetComponent<NPCBehaviour>();
            _npc.StopDamage();
            //npc.Remove(_npc);
            //StopEatLeg();
            RemoveNPC(_npc);
        }
    }

    private void RemoveNPC(NPCBehaviour eaten)
    {
        StopEatLeg(eaten);
        CheckAttackNPC();

    }

    private void CheckAttackNPC()
    {
            for (int i = 0; i < partsPerLeg && (availableLegPool.Count > 0 || legs.Count < maxLegs); i++)
            {
                foreach (var _npc in npc)
                {
                    if (availableLegPool.Count > 0 || legs.Count < maxLegs)
                    {
                        if (_npc.multiplicator < partsPerLeg)
                        {
                        Debug.Log(_npc.multiplicator);
                            _npc.GetDamage(0.1f);
                            SetLeg(_npc);
                        }
                    }
                    else return;
                }
            }
    }

    void StopEatLeg(NPCBehaviour eaten)
    {
        npc.Remove(eaten);
        if (legOnNpc.ContainsKey(eaten))
        {
            foreach(var leg in legOnNpc[eaten])
            {
                leg.SetActive(false);
                availableLegPool.Add(leg);
                legOnNpc.Remove(eaten);
            }
            
            //legOnNpc[eaten].SetActive(false);
            //availableLegPool.Add(legOnNpc[eaten]);
            //legOnNpc.Remove(eaten);
        }
    }
    void SetLeg(NPCBehaviour _npc)
    {
        StartCoroutine(EatLeg(_npc));
    }
    IEnumerator EatLeg(NPCBehaviour _npc)
    {
        Transform food = _npc.transform;
        GameObject newLeg;
        if (availableLegPool.Count > 0)
        {
            newLeg = availableLegPool[^1];
            availableLegPool.RemoveAt(availableLegPool.Count - 1);
        }
        else
        {
            newLeg = Instantiate(legPrefab, transform.position, Quaternion.identity);
            legs.Add(newLeg);
            newLeg.transform.SetParent(this.transform);
        }
        newLeg.SetActive(true);
        var leg = newLeg.GetComponent<Leg>();
        leg.Initialize(food.position, legResolution, maxLegDistance, Random.Range(minGrowCoef, maxGrowCoef), this, 10f);
        if(!legOnNpc.ContainsKey(_npc)) legOnNpc.Add(_npc, new List<GameObject> { newLeg });
        else legOnNpc[_npc].Add(newLeg);

        while (legOnNpc.ContainsKey(_npc))
        {
            yield return new WaitForSeconds(0.02f);
            if (newLeg != null) leg.SetFootPosition(food.position);

        }

        

    }

    private void OnEnable()
    {
        NPCBehaviour.NPCDie += RemoveNPC;
    }

    private void OnDisable()
    {
        NPCBehaviour.NPCDie -= RemoveNPC;
    }
}
