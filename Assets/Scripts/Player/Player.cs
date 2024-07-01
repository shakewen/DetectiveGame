using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    public Animator animator;

    //获取按键输入
    public float moveInput;

    public bool isInteract;

    public bool isTab;


    //是否可以移动
    public bool canmove = true;

    public float speed = 2f;
   
    public bool faceRight = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator =GetComponent<Animator>();
    }

    
    void Update()
    {
        moveInput = InputManager.GethorizontalInput();

        isInteract = InputManager.GetJInput();

        isTab = InputManager.GetTabInput();


        if(canmove&& moveInput!= 0f)
        {
            animator.SetFloat("Move", Mathf.Abs(moveInput));
        }
        
        //如果进入交换面板且场景内物体可交互，则canMove为false，表示人物不可移动和进行动画

        //
    }

    void FixedUpdate()
    {
        if (canmove)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        MoveToFlip();
    }


    void MoveToFlip()
    {
        if (canmove)
        {
            if ((!faceRight && moveInput > 0f) || (faceRight && moveInput < 0f))
            {
                faceRight = !faceRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
    }
}
