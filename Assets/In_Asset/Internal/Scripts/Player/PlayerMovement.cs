using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private CharacterConfig config;
    private Animator animator;

    [SerializeField] private float moveSpeed = 1f;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.TryGetComponent<Animator>(out animator) && child.gameObject.TryGetComponent<CharacterConfig>(out config))
            {
                break;
            }
        }
    }
    public void Movement(Vector2 input)
    {
        if (input.sqrMagnitude >= 0.1f)
        {
            animator.SetFloat("Horizontal", input.x);
            animator.SetFloat("Vertical", input.y);

            if (input.x > 0f)
            {
                animator.SetFloat("Idle", 1);
            }
            else if (input.x < 0f)
            {
                animator.SetFloat("Idle", 2);
            }
            else
            {
                if (input.y > 0f)
                {
                    animator.SetFloat("Idle", 0);
                }
                else if (input.y < 0f)
                {
                    animator.SetFloat("Idle", 3);
                }
            }

            rb.MovePosition(rb.position + moveSpeed * Time.deltaTime * input.normalized);
        }
        animator.SetFloat("Speed", input.sqrMagnitude >= 0.1f ? 1f : 0f);
    }
}
