using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject itemController;

    public Color color =new Color(1,1,1,1);

   [SerializeField] private float outlineOffset = 3f;

   //public bool isInteract = false;

    private List<SpriteRenderer> ItemSpriteRenderer;

    //设置每个可采样的物体是否进入交互状态
    public Dictionary<SpriteRenderer, bool> interactDictionary = new Dictionary<SpriteRenderer, bool>();


    public SpriteRenderer currentSpriteRenderer;

    public SpriteRenderer CurrentInteractSR;

    public MaterialPropertyBlock block;

    

    [SerializeField] private float fadeSpeed = 3.5f;


     void OnEnable()
    {
        
       
    }

    void Start()
    {
        ItemSpriteRenderer = new List<SpriteRenderer>();

        itemController=GameObject.FindWithTag("ItemController");

        block = new MaterialPropertyBlock();



        for (int i = 0; i < itemController.transform.childCount; i++)
        {
           SpriteRenderer itemRenderer = itemController.transform.GetChild(i).GetComponent<SpriteRenderer>();
           
            ItemSpriteRenderer.Add(itemRenderer);

            //设置键值对应初始为false
            interactDictionary[itemRenderer] = false;
        }

        
    }

     void OnDisable()
    {
        
    }


    void Update()
    {

        //是否开启轮廓，当前物体，轮廓颜色，轮廓采样偏移量（锐化程度）
        UpdateOutline(interactDictionary[currentSpriteRenderer], outlineOffset);


        IsInteract();
    }

    //是否开启轮廓，轮廓渐变
    void UpdateOutline(bool gradualchangeColor, float outlineOffset)
    {
        if (currentSpriteRenderer == null)
            return;

        currentSpriteRenderer.GetPropertyBlock(block);

        block.SetFloat("_Outline", gradualchangeColor ? 1f : 0);
        //使用全局变量的字符串
        block.SetFloat("_SampleOffset", outlineOffset);

        //color.a =Mathf.Clamp(color.a,0,1);

        //////暂时取消渐变，因为物体密集度太高
        //if (gradualchangeColor)
        //{
        //    color.a += Time.deltaTime * fadeSpeed;
        //}
        //else
        //{
        //    color.a -= Time.deltaTime * fadeSpeed;
        //}
        //设置渐变效果要设置为全局变量
        block.SetColor("_OutlineColor", color);

        currentSpriteRenderer.SetPropertyBlock(block);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        OnPlayerEnterItem(collision,true);

      
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnPlayerEnterItem(collision,false);
    }

   
    void OnPlayerEnterItem(Collider2D collision,bool _isInteract)
    {
        foreach (SpriteRenderer itemRenderer in ItemSpriteRenderer)
        {

            if (itemRenderer.gameObject == collision.gameObject)
            {
                
                if (collision.gameObject.layer==(LayerMask.NameToLayer("Transparency")))
                {
                   
                    isTransparecy(itemRenderer, _isInteract);
                }

                //主角触碰到可交互物体
                if (collision.CompareTag("Item"))
                {
                    print(itemRenderer.name);

                        //当前可被采样的物体
                        currentSpriteRenderer = itemRenderer;

                    //只接收当前在玩家进入时的物体才能被交互
                    if (_isInteract)
                    {
                        //当前可交互物体
                        CurrentInteractSR = itemRenderer;

                    }
                        
                    interactDictionary[itemRenderer] = _isInteract;
                }

            }


        }

    }

    void isTransparecy(SpriteRenderer _itemRenderer,bool _isTransparent)
    {
        _itemRenderer.color = new Color(_itemRenderer.color.r, _itemRenderer.color.g, _itemRenderer.color.b, _isTransparent ? 0.65f : 1);
    }

    void IsInteract()
    {
        //当前可交互的物体能够交互
        if (interactDictionary[CurrentInteractSR])
        {
            if (InputManager.GetJInput())
            {
                print("可以交互");
                //显示文本框输出文本
                //等待文本全部输入完成，再次点击鼠标左键，关闭文本框
            }
        }
    }
}
