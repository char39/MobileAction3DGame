using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]    // Animator 컴포넌트를 요구함. 삭제 불가.

public class Player : MonoBehaviour
{
    public static Player instance;    // 싱글톤을 할당할 전역 변수.
    protected Animator animator;    // Animator 컴포넌트를 저장할 변수.
    protected Rigidbody rb;         // Rigidbody 컴포넌트를 저장할 변수.
    private float h = 0.0f , v = 0.0f;      // 수평, 수직 이동 변수.
    private float moveSpeed;         // 이동 속도.
    private bool isSkill = false;     // 스킬 사용 여부.
    private bool isAttack = false;    // 공격 사용 여부.
    private bool isDash = false;      // 대쉬 사용 여부.
    private AudioSource source;
    public AudioClip skillSound;
    private float lastAttackTime = 0.0f;

    IEnumerator Start()
    {
        instance = this;    // 자신을 할당.
        animator = GetComponent<Animator>();    // Animator 컴포넌트를 가져와서 저장.
        rb = GetComponent<Rigidbody>();         // Rigidbody 컴포넌트를 가져와서 저장.
        source = GetComponent<AudioSource>();   // AudioSource 컴포넌트를 가져와서 저장.
        moveSpeed = 8.0f;
        yield return null;
    }

    void Update()
    {
        PlayerMove();
    }
    public void PlayerSkillOn()                   // 플레이어 스킬 함수.
    {
        if (animator != null && !isSkill)
        {
            animator.SetTrigger("SkillTrigger");
            isSkill = true;
            Invoke("PlayerSkillEnd", 2.5f);   
        }
    }
    private void PlayerSkillEnd()                 // 스킬 사용 종료 함수.
    {
        isSkill = false;
    }
    public void PlayerSkillSound()                // 스킬 사용 사운드 함수.
    {
        source.PlayOneShot(skillSound);
    }
    
    public void PlayerAttackOn()                    // 플레이어 공격 함수.
    {
        if (animator != null && !isAttack)
        {
            isAttack = true;
            animator.SetBool("ComboAttack", true);
            StartCoroutine(ComboAttackTiming());
        }
    }
    public void PlayerAttackOff()                   // 공격 사용 종료 함수.
    {
        isAttack = false;
        animator.SetBool("ComboAttack", false);
    }
    IEnumerator ComboAttackTiming()                 // 콤보 공격 타이밍
    {
        if (Time.time - lastAttackTime > 1.0f)      // 1초 이상이 지났을 때
        {
            lastAttackTime = Time.time;             // 현재 시간을 저장.
            while (isAttack)
            {
                animator.SetBool("ComboAttack", true);
                yield return new WaitForSeconds(1.0f);   
            }
        }
    }

    public void PlayerDashAttackOn()                   // 플레이어 대시 함수.
    {
        if (animator != null && !isDash)
        {
            animator.SetTrigger("DashAttackTrigger");
            isDash = true;
            animator.applyRootMotion = true;
            Invoke("PlayerDashAttackEnd", 1.7f);   
        }
    }
    private void PlayerDashAttackEnd()                 // 대시 사용 종료 함수.
    {
        isDash = false;
        animator.applyRootMotion = false;
    }

    public void PlayerMove()                    // 플레이어 이동 함수.
    {
        if (animator != null && !isSkill && !isAttack && !isDash)
        {
            animator.SetFloat("Speed", h * h + v * v);    // Speed 파라미터에 수평, 수직 이동 값의 제곱을 전달.
            if (rb != null)
            {
                Vector3 speed = rb.velocity;    // 현재 속도를 저장.
                speed.x = h * moveSpeed;        // 수평 이동 값에 6을 곱한 값을 x축 속도로 설정.
                speed.z = v * moveSpeed;        // 수직 이동 값에 6을 곱한 값을 z축 속도로 설정.
                if (animator.GetFloat("Speed") > 0.1f)
                    rb.velocity = speed;        // 속도를 적용.
                if (h != 0f || v != 0f)         // 움직일 때만
                {
                    transform.rotation = Quaternion.LookRotation(speed);    // 속도 방향으로 회전.
                    //transform.rotation = Quaternion.LookRotation(new Vector3(h, 0f, v));    // 속도 방향으로 회전.
                }
            }
        }
    }
    public void OnStickPos(Vector3 stickpos)    // 조이스틱의 위치를 받아오는 함수.
    {
        h = stickpos.x;    // 수평 이동 값.
        v = stickpos.y;    // 수직 이동 값.
    }

}
