using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Diagnostics;
using Random = UnityEngine.Random;


public class Monster : Entity
{
    public bool EnablePatrol
    {
        set
        {
            if (value)
            {
                if (coPatrol == null)
                    coPatrol = StartCoroutine("Patrol");
            }
            else
            {
                if (coPatrol != null)
                {
                    StopCoroutine(coPatrol);
                    coPatrol = null;
                }
            }
        }
    }

    UI_WorldText infoText;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Monster;
        infoText = Util.FindChild<UI_WorldText>(gameObject);
        
        return true;
    }

    public override void SetData(EntityData data)
    {
        base.SetData(data);

        MonsterData monsterData = data as MonsterData;
        if (monsterData != null)
        {
            // Set MonsterData
        }

        onDead += HandleOnDead;
        Movement.AgentEnabled = true;
        Movement.enabled = true;
    }

    private void OnDisable()
    {
        onDead -= HandleOnDead;
    }

    private void HandleOnDead(Entity entity)
    {
        Movement.AgentEnabled = false;
        EnableSearching = false;
        EnablePatrol = false;
        Target = null;
        Movement.TraceTarget = null;
        Invoke("Despawn", 5.0f);
    }


    void Update()
    {
        infoText.SetInfo(StateMachine.GetCurrentState().ToString());
    }

    private void Despawn()
    {
        Managers.Object.Despawn(this);
    }

    private Coroutine coPatrol;
    private IEnumerator Patrol()
    {
        while (true)
        {
            Vector2 randomPos = transform.position;
            randomPos = randomPos.RandomPointInAnnulus(2, 5);

            NavMesh.Raycast(Position, randomPos, out NavMeshHit hit, NavMesh.AllAreas);
            randomPos = hit.position;

            Movement.Destination = randomPos;
            yield return new WaitUntil(() => Movement.AtDestination);

            int idleTime = Random.Range(2, 6);
            yield return WaitFor.Seconds(idleTime);
        }
    }

    public override void FindNearestEnemy()
    {
        float searchRange = Stats.GetValue(Stats.SearchRangeStat);

        int heroLayer = Util.GetLayerMask("Hero");
        var colliders = Physics2D.OverlapCircleAll(Position, searchRange, heroLayer);

        float nearestDistance = Mathf.Infinity;
        Entity nearestEnemy = null;
        foreach (var collider in colliders)
        {
            var entity = collider.GetComponent<Entity>();
            if (entity == this || entity.IsDead)
                continue;

            if (!entity.HasCategory(enemyCategory))
                continue;

            float distance = Vector2.Distance(entity.Position, transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = entity;
            }
        }

        if (nearestEnemy)
        {
            Target = nearestEnemy;
            EnablePatrol = false;

            if (SkillSystem.DefaultAttack.IsUseable)
                SkillSystem.DefaultAttack.Use();
        }
        else
        {
            Target = null;
            EnablePatrol = true;
        }

        return;
    }
}