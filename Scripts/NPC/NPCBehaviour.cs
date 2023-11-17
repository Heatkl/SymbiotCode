using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    [Header("Basic Settings")]

    [Tooltip("NPC normal speed")]
    [Range(1, 30f)]
    public float speed = 1f;

    [Tooltip("x,y (x,z) - upper left corner of the zone;  z,w (x,z) - lower right corner of the zone")]
    public Vector4 moveZone;
    [Space(20)]
    [Tooltip("Number of seats taken from eating")]
    [Min(1)]
    public int weight = 1;

    public float health = 1;
    [Tooltip("Respawn time in seconds")]
    public int respawnTime = 1;



    [Header("")]

    public Reward rewardSettings;



    [Header("GameObject references")]
    public GameObject player;
    protected EnemyUI EnemyUI;

    protected bool isRunning;
    protected bool isEaten = false;
    protected bool isDead = false;
    protected float currentHealth;
    protected Animator animator;
    protected Transform _transform;
    public Vector3 target;
    public int multiplicator = 0;

    public float PlayerDamage { get; set; }

    public static event System.Action<NPCBehaviour> NPCDie;
    protected virtual void StartSettings()
    {
        _transform = transform;
        currentHealth = health;
        animator = GetComponent<Animator>();
        EnemyUI = GetComponentInChildren<EnemyUI>();
        SetUI();
        target = _transform.position;
        CalculateCoords();

    }
    protected virtual void MainLogic()
    {
        Move();
        if (isEaten)
        {
            TakeDamage();
        }
        else
        {
            HPRegeneration();
        }

    }
    protected void RunAway()
    {
        animator.Play("Run");
        Move();
    }

    protected virtual void IdleWalk()
    {
        animator.Play("Idle");
        Move();
    }

    private void Move()
    {

        _transform.position = Vector3.MoveTowards(_transform.position, target, speed * Time.deltaTime);
        //transform.Translate(speed*Time.deltaTime, 0, 0);
        if (_transform.position == target)
            CalculateCoords();
    }

    public void GetDamage(float damage)
    {
        multiplicator++;
        PlayerDamage = damage * multiplicator;
        isEaten = true;
        EnemyUI.gameObject.SetActive(true);
        if (!isRunning) StartCoroutine(AnimationDelay());
    }

    public void StopDamage()
    {
        isEaten = false;
        multiplicator = 0;
    }

    protected void TakeDamage()
    {
        currentHealth -= PlayerDamage * Time.deltaTime;
        EnemyUI.ChangeValue(currentHealth);

        if (currentHealth <= 0)
            Death();
    }

    protected void HPRegeneration()
    {
        if (currentHealth == health)
        {
            EnemyUI.gameObject.SetActive(false);
            return;
        }
            

        if (currentHealth < health)
        {
            currentHealth += health / 2 * Time.deltaTime;
        }
        if (currentHealth > health)
        {
            currentHealth = health;
        }
        EnemyUI.ChangeValue(currentHealth);
    }

    protected void Death()
    {
        if (!isDead)
        {
            isDead = true;
            NPCDie?.Invoke(this);
            Debug.Log("Death");
            gameObject.SetActive(false);
            GameManager.instance.progress.gem += rewardSettings.gem;
            GameManager.instance.progress.DNA += rewardSettings.DNA;
            GameManager.instance.progress.UpdateEXP(rewardSettings.EXP);
            GameManager.instance.progress.crystall += rewardSettings.crystal;
            GameManager.instance.progress.meat += rewardSettings.meat;
            GameManager.instance.Respawner.RespawnSeconds(_transform.GetSiblingIndex(), respawnTime);
        }

    }

    protected void CalculateCoords()
    {
        target = new Vector3(Random.Range(moveZone.x, moveZone.z), _transform.position.y, Random.Range(moveZone.w, moveZone.y));
        _transform.LookAt(target);
    }

    protected void SetUI()
    {
        string rewards = "MeatReward";
        List<int> quantity = new();
        quantity.Add(rewardSettings.meat);
        if (rewardSettings.hasOtherReward)
        {
            if (rewardSettings.gem > 0)
            {
                rewards += "/GemReward";
                quantity.Add(rewardSettings.gem);
            }
            if (rewardSettings.crystal > 0)
            {
                rewards += "/CrystallReward";
                quantity.Add(rewardSettings.crystal);
            }
            if (rewardSettings.DNA > 0)
            {
                rewards += "/DNAReward";
                quantity.Add(rewardSettings.DNA);
            }
            if (rewardSettings.blackReagent > 0)
            {
                rewards += "/BlackReagentReward";
                quantity.Add(rewardSettings.blackReagent);
            }
            if (rewardSettings.yellowReagent > 0)
            {
                rewards += "/YellowReagentReward";
                quantity.Add(rewardSettings.yellowReagent);
            }
            if (rewardSettings.blueReagent > 0)
            {
                rewards += "/BlueReagentReward";
                quantity.Add(rewardSettings.blueReagent);
            }
            if (rewardSettings.redReagent > 0)
            {
                rewards += "/RedReagentReward";
                quantity.Add(rewardSettings.redReagent);
            }
            if (rewardSettings.greenReagent > 0)
            {
                rewards += "/GreenReagentReward";
                quantity.Add(rewardSettings.greenReagent);
            }

        }
        EnemyUI.SetValue(health);
        EnemyUI.ActivateEnemyRewards(rewards, quantity);
    }



    IEnumerator AnimationDelay()
    {
        animator.Play("Run");
        isRunning = true;
        while (isEaten)
        {
            yield return new WaitForSeconds(2f);
        }
        animator.Play("Idle");
        isRunning = false;
    }

    private void OnEnable()
    {
        currentHealth = health;
        isDead = false;
        isEaten = false;
        multiplicator = 0;
    }

}
    [System.Serializable]
    public class Reward
    {
        [Min(1)]
        public int meat = 1;
        [Min(1)]
        public int EXP = 1;
        [Space(20)]
        public bool hasOtherReward = false;
        [Space(10)]
#if UNITY_EDITOR 
    [ConditionalHide("hasOtherReward", true)] 
        #endif
    public int gem = 0;
#if UNITY_EDITOR 
    [ConditionalHide("hasOtherReward", true)] 
    #endif
    public int crystal = 0;
#if UNITY_EDITOR
    [ConditionalHide("hasOtherReward", true)]
        #endif
        public int DNA = 0;

        [Space(20)]
#if UNITY_EDITOR
    [ConditionalHide("hasOtherReward", true)]
        #endif
        public int redReagent = 0;
#if UNITY_EDITOR
    [ConditionalHide("hasOtherReward", true)]
        #endif
        public int blueReagent = 0;
#if UNITY_EDITOR
    [ConditionalHide("hasOtherReward", true)]
        #endif
        public int greenReagent = 0;
#if UNITY_EDITOR
    [ConditionalHide("hasOtherReward", true)]
        #endif
        public int yellowReagent = 0;
#if UNITY_EDITOR
    [ConditionalHide("hasOtherReward", true)]
        #endif
        public int blackReagent = 0;
    }


