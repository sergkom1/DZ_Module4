using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector2 movement;
    private bool facingRight = true;

    [Header("Animations")]
    private Animator animator;
    private string currentState;

    // �������� ����� �������� (�������� �� ����)
    const string IDLE_UP = "IdleUp";
    const string IDLE_DOWN = "IdleDown";
    const string IDLE_LEFT = "IdleLeft";
    const string WALK_UP = "WalkUp";
    const string WALK_DOWN = "WalkDown";
    const string WALK_LEFT = "WalkLeft";
    const string WALK_RIGHT = "WalkLeft"; // ���������� �������� ����� � ��������

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ���� � ����������
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // ������������ ��������
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        // ��������
        GetComponent<Rigidbody2D>().velocity = movement.normalized * moveSpeed;
    }

    void UpdateAnimation()
    {
        bool isMoving = movement.magnitude > 0;

        if (isMoving)
        {
            // ���������� �����������
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                // �������������� ��������
                if (movement.x > 0)
                {
                    ChangeAnimationState(WALK_RIGHT);
                    if (!facingRight) Flip();
                }
                else
                {
                    ChangeAnimationState(WALK_LEFT);
                    if (facingRight) Flip();
                }
            }
            else
            {
                // ������������ ��������
                if (movement.y > 0)
                    ChangeAnimationState(WALK_UP);
                else
                    ChangeAnimationState(WALK_DOWN);
            }
        }
        else
        {
            // �������� ����� (��������� �����������)
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(WALK_UP))
                ChangeAnimationState(IDLE_UP);
            else if (animator.GetCurrentAnimatorStateInfo(0).IsName(WALK_DOWN))
                ChangeAnimationState(IDLE_DOWN);
            else
                ChangeAnimationState(facingRight ? IDLE_LEFT : IDLE_LEFT); // ������� �� ����� ��������
        }

        animator.SetBool("IsMoving", isMoving);
    }

    void Flip()
    {
        // ���������� ��������� ��� ������/����� �������
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}