using JYW.RandomSurvival.Commmons;
using JYW.RandomSurvival.Managers;
using JYW.RandomSurvival.Utils;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected abstract float hp { get; set; }
    public abstract float power { get; set; }
    public abstract int EXP { get; set; }
    public abstract EnemyType enemyType { get; set; }

    protected Transform player;
    protected Coroutine chaseCoroutine;
    

    protected virtual void Awake()
    {
        if (GetType() != typeof(Gate))
            EventManager.Instance.OnDeltaEnemyCount(1);

    }

    protected void Start()
    {

        if (enemyType == EnemyType.Chase)
        {
            InvokeRepeating(nameof(Chase), 0f, 0.5f);
        }
    }

    private void Chase()
    {
        if (chaseCoroutine != null) StopCoroutine(chaseCoroutine);
        chaseCoroutine = StartCoroutine(GridUtil.goToIEnu(EventManager.Instance.OnGetMap().map, this.gameObject.transform, EventManager.Instance.OnGetPlayerPosition(), 3f));

    }



    public virtual void TakeDamage(float getDamage)
    {
        hp -= getDamage;
        if (hp <= 0) Destroy(this.gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Instance.OnHitplayer(power);
        }
    }

    protected virtual void OnDestroy()
    {
        if (GetType() != typeof(Gate))
        {
            EventManager.Instance.OnDeltaEnemyCount(-1);
            EventManager.Instance.OnGetGetPlayerStat().UpdateEXP(EXP);
            EventManager.Instance.OnUpdateEXPUI(EventManager.Instance.OnGetGetPlayerStat().currentEXP, EventManager.Instance.OnGetGetPlayerStat().currentMaxEXP);
        }
    }
}
