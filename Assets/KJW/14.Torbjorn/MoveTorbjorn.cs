using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Idle 에서 Move 로 전환되는 애니메이션 처리를 하고 싶다.
// 필요속성 : Animator
// 타겟으로 이동할 수 없는 범위에서는 패트롤 하고 싶다.
// 필요속성 : 이동할 수 있는 범위
public class MoveTorbjorn : MonoBehaviour
{
    #region 상태정의
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Damage,
        Die
    };

    EnemyState m_state = EnemyState.Idle;
    #endregion

    #region Idle 속성
    // 필요속성 : 대기시간, 경과시간
    float idleDelayTime = 120;
    float currentTime = 0;
    #endregion

    #region Move 속성
    // 필요속성 : 타겟, 이동속도, CharacterController
    public Transform target;
    public float speed = 5;
    CharacterController cc;
    // 필요속성 : 이동할 수 있는 범위
    public float moveToTargetRange = 0.5f;
    #endregion

    // 필요속성 : Animator
    Animator anim;

    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        print("state : " + m_state);
        switch (m_state)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Damage:
                //Damage();
                break;
            case EnemyState.Die:
                // Die();
                break;
        }
    }

    // 일정시간이 지나면 상태를 이동으로 전환하고 싶다.

    private void Idle()
    {
        // 일정시간이 지나면 상태를 이동으로 전환하고 싶다.
        // 1. 시간이 흘렀으니까
        currentTime += Time.deltaTime;
        // 2. 시간이 됐으니까
        if (currentTime > idleDelayTime)
        {
            // 3. 상태를 이동으로 전환
            m_state = EnemyState.Move;
            currentTime = 0;
            // 애니메이션의 상태도 이동으로 전환
            anim.SetTrigger("Move");

            agent.enabled = true;
        }
    }

    // 타겟쪽으로 이동하고 싶다.
    // 타겟 방향으로 회전하고 싶다.
    // 타겟이 공격범위안에 들어오면 상태를 공격으로 전환하고 싶다.
    // 필요속성 : 공격범위
    public float attackRange = 2;
    bool result = false;
    Vector3 randPos = Vector3.zero;
    private void Move()
    {
        // 타겟쪽으로 이동하고 싶다.
        // 1. 방향이필요(target - me)
        Vector3 dir = target.position - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();
        dir.y = 0;

        // 내 시야 범위안에 없고,
        float dot = Vector3.Dot(transform.forward, dir);
        //// 내 전방벡터
        //Debug.DrawLine(transform.position, transform.position + transform.forward * 5, Color.red);
        //// 타겟쪽으로 향하는 벡터
        //Debug.DrawLine(transform.position, transform.position + dir * 5, Color.red);

        // 타겟으로 이동
        if (true)//distance < moveToTargetRange && dot > 0.5f)
        {
            // 타겟으로 이동하기
            agent.destination = target.position;
        }
        // 그렇지 않으면
        // 타겟으로 이동할 수 없는 범위에서는 패트롤 하고 싶다.
        else
        {
            // 패트롤
            // 아직 이동할 곳을 못찾았을 때
            if (result == false)
            {
                result = GetRandomPosition(transform.position, out randPos, 10);
            }
            // 만약 찾았다면
            else
            {
                // -> 목적지를 randPos 로 정해주기
                agent.destination = randPos;
                // -> 목적지에 거의 다왔다면
                if (Vector3.Distance(transform.position, randPos) < 0.1f)
                {
                    // -> 다시 찾은 곳이 없도록 해주기
                    result = false;
                }
            }
        }
        // 2. 이동하고 싶다.
        //cc.SimpleMove(dir * speed);
        // Enemy 의 방향을 dir 로 하자
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);

        // 타겟이 공격범위안에 들어오면 상태를 공격으로 전환하고 싶다.
        if (distance < attackRange)
        {
            m_state = EnemyState.Attack;
            currentTime = attackDelayTime;
            agent.enabled = false;
        }
    }

    // 특정 위치를 기준으로 네비게이션이 가능한 위치를 넘겨주는다.
    private bool GetRandomPosition(Vector3 position, out Vector3 randPos, float range = 3)
    {
        Vector3 center = Random.insideUnitSphere * range;
        center.y = 0;
        center += position;
        NavMeshHit hitInfo;
        bool result = NavMesh.SamplePosition(center, out hitInfo, range, 1);

        randPos = hitInfo.position;
        return result;
    }

    // 일정시간에 한번씩 공격하고 싶다.
    // 필요속성 : 공격대기시간
    // 타겟이 공격범위를 벗어나면 상태를 이동으로 전환하고 싶다.
    public float attackDelayTime = 5;
    private void Attack()
    {
        Vector3 dir = target.position - transform.position;
        dir.Normalize();
        dir.y = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);

        currentTime += Time.deltaTime;
        if (currentTime > attackDelayTime)
        {
            currentTime = 0;
            print("attack!!!");
            anim.SetTrigger("Attack");
        }
        // 타겟이 공격범위를 벗어나면 상태를 이동으로 전환하고 싶다.
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance > attackRange)
        {
            m_state = EnemyState.Move;
            anim.SetTrigger("Move");

            agent.enabled = true;
            //anim.Play("Move");
            //anim.CrossFade()
        }
    }

    // 피격 이벤트 받아서 처리 함수
    // 3대맞으면 죽도록 처리하자
    // 필요속성 : 체력
    // 체력이 남아있으면 상태를 피격으로 전환하고 싶다.
    // 그렇지않으면 상태를 죽음으로 전환하고 싶다.
    // 죽음 상태가 되면 충돌되지 않도록 처리
    // 넉백처리
    // 필요속성 : 넉백스피드
    public float knockBackSpeed = 2;
    Vector3 knockEndPos;

    public int hp = 3;
    public void OnDamageProcess(Vector3 shootDirection)
    {
        agent.enabled = false;
        StopAllCoroutines();
        // 3대맞으면 죽도록 처리하자
        hp--;
        // 체력이 없을 때 상태를 죽음으로 전환하고 싶다.
        if (hp <= 0)
        {
            m_state = EnemyState.Die;
            cc.enabled = false;
            anim.SetTrigger("Die");
            StartCoroutine(Die());
        }
        // 그렇지 않으면 상태를 피격으로 전환하고 싶다.
        else
        {
            m_state = EnemyState.Damage;
            anim.SetTrigger("Damage");
            //transform.position += shootDirection * knockBackSpeed;
            //cc.Move(shootDirection * knockBackSpeed);
            shootDirection.y = 0;
            knockEndPos = transform.position + shootDirection * knockBackSpeed;
            StartCoroutine(Damage());
        }

        currentTime = 0;
    }

    // 일정시간 기다렸다가 상태를 대기로 전환하고 싶다.
    // 필요속성 : 피격대기시간
    public float damageDelayTime = 2;
    private IEnumerator Damage()
    {
        float curTime = 0;
        // 넉백 애니메이션 구현
        // 넉백 위치에 도착할 때 까지 반복하고 싶다.
        while (curTime < damageDelayTime)
        {
            curTime += Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, knockEndPos, 10 * Time.deltaTime);

            yield return null;
        }
        transform.position = knockEndPos;

        // 일정시간 기다렸다가
        //yield return new WaitForSeconds(damageDelayTime);
        // 상태를 Idle 로 전환하고 싶다.
        m_state = EnemyState.Idle;

        //currentTime += Time.deltaTime;
        //// 2. 시간이 됐으니까
        //if (currentTime > damageDelayTime)
        //{
        //    // 3. 상태를 이동으로 전환
        //    m_state = EnemyState.Idle;
        //    currentTime = 0;
        //}
    }

    // 아래로 사라지도록 하자.
    // 없어지면 제거하자. -2
    public float dieSpeed = 0.5f;
    //private void Die()
    //{
    //    // 애니메이션이 다 재생되고 
    //    // 2 초 기다렸다가 
    //    currentTime += Time.deltaTime;
    //    if (currentTime > 2)
    //    {
    //        // 아래로 사라진다.
    //        transform.position += Vector3.down * dieSpeed * Time.deltaTime;
    //        if (transform.position.y < -3)
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //}

    private IEnumerator Die()
    {
        // 애니메이션이 다 재생되고 
        // 2 초 기다렸다가 
        yield return new WaitForSeconds(2);

        while (true)
        {
            // 아래로 사라진다.
            transform.position += Vector3.down * dieSpeed * Time.deltaTime;
            if (transform.position.y < -3)
            {
                Destroy(gameObject);
                yield break;
            }
            yield return null;
        }
    }
}