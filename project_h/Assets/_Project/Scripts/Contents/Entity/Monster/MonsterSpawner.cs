using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

// Area범위 내에 랜덤 스폰
// 지정된 몬스터 수 유지
// 스폰 : 몬스터 스펙 세팅(스프라이트, 애니메이터 등등 -> MonsterData)
// 스폰한 몬스터는 죽으면 해당 스포너의 event발생??

// 3분에 1번 소환
// 소환 했을 때 전체 몬스터 수 유지
public class MonsterSpawner : InitOnce
{
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private float spawnRange = 8;
    [SerializeField] private float spawnCycle = 10;
    [SerializeField, Min(1)] private int spawnMax = 5;

    private int spawnCount;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        StartCoroutine("CoSpawnMonster");

        return true;
    }

    [ButtonAttribute("OnClickButton")]
    public bool TestButton;

    public void OnClickButton()
    {
        if (spawnCount >= spawnMax)
            return;
        
        SpawnMonster();
    }

    private IEnumerator CoSpawnMonster()
    {
        while (true)
        {
            yield return WaitFor.Seconds(spawnCycle);

            while (spawnCount < spawnMax)
            {
                SpawnMonster();
                yield return WaitFor.Seconds(1);
            }
        }

    }
    private void SpawnMonster()
    {
        Vector2 randomPos = transform.position;
        randomPos = randomPos.RandomPointInAnnulus(1, spawnRange);
        
        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 4, NavMesh.AllAreas))
        {
            Monster monster = Managers.Object.Spawn<Monster>(hit.position, nameof(Monster));
            monster.SetData(monsterData);
            monster.onDead -= HandleMonsterDead;
            monster.onDead += HandleMonsterDead;
            spawnCount++;
        }
    }

    private void HandleMonsterDead(Entity entity)
    {
        spawnCount--;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRange);    
    }

}