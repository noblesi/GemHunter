using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    [SerializeField]
    private Transform playerModel; // 좌우 반전을 위한 플레이어 트랜스폼
    [SerializeField]
    private Transform playerArmsModel;
    [SerializeField]
    private ParticleSystem footStepEffect;
    private ParticleSystem.EmissionModule footEmission;
    private Animator animator;

    private void Awake()
    {
        footEmission = footStepEffect.emission;
        animator = GetComponent<Animator>();
    }

    public void OnMovement(float speed)
    {
        animator.SetFloat("moveSpeed", speed);
    }

    public void OnFootStepEffect(bool isMoved)
    {
        footEmission.rateOverTime = isMoved == true ? 20 : 0;
    }

    public void SpriteFlipX(float x)
    {
        Vector3 currentScale = playerModel.localScale;
        currentScale.x = x < 0 ? -1.5f : 1.5f;
        playerModel.localScale = currentScale;
    }

    public void LookRotation(PlayerBase playerBase)
    {
        if(playerBase.IsMoved == true)
        {
            playerArmsModel.rotation = Quaternion.identity;
        }
        else
        {
            if (playerBase.Target == null) return;

            Vector3 target = playerBase.Target.MiddlePoint;

            float flip = target.x - transform.position.x < 0 ? -1 : 1;

            SpriteFlipX(flip);

            playerArmsModel.rotation = Utils.RotateToTarget(playerArmsModel.position, target, (1 - flip) * 90);
        }
    }
}
