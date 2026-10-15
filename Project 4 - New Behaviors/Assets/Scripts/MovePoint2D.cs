using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move To Point 2D", story: "[Agent] moves to [Target] at [Speed]", category: "Action", id: "a3f1c2d4e5b64718a9c0d1e2f3a4b5c6")]
public partial class MoveToPoint2DAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Speed;

    private const float StopDistance = 0.1f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    protected override Status OnStart()
    {
        if (Agent.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }

        // find the animator and sprite on the enemy (or its children)
        animator = Agent.Value.GetComponentInChildren<Animator>();
        spriteRenderer = Agent.Value.GetComponentInChildren<SpriteRenderer>();

        // start the run animation
        if (animator != null)
        {
            animator.SetBool("isRunning", true);
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Transform agent = Agent.Value.transform;
        float targetX = Target.Value.transform.position.x;

        // face the direction of travel
        if (spriteRenderer != null)
        {
            float direction = targetX - agent.position.x;
            if (direction > 0.01f) spriteRenderer.flipX = false;
            else if (direction < -0.01f) spriteRenderer.flipX = true;
        }

        // move only along X
        float newX = Mathf.MoveTowards(agent.position.x, targetX, Speed.Value * Time.deltaTime);
        agent.position = new Vector3(newX, agent.position.y, agent.position.z);

        // arrived at the point
        if (Mathf.Abs(newX - targetX) <= StopDistance)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        // back to idle when the move finishes or is interrupted
        if (animator != null)
        {
            animator.SetBool("isRunning", false);
        }
    }
}