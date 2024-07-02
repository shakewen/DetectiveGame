using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject itemController;

    public Color color =new Color(1,1,1,1);

   [SerializeField] private float outlineOffset = 3f;

   [SerializeField] private bool startOutline = false;

    private List<SpriteRenderer> ItemSpriteRenderer;

    public SpriteRenderer currentSpriteRenderer;

    


     void OnEnable()
    {
        
       
    }

    void Start()
    {
        ItemSpriteRenderer = new List<SpriteRenderer>();
        itemController=GameObject.Find("ItemController");
        for (int i = 0; i < itemController.transform.childCount; i++)
        {
            ItemSpriteRenderer.Add(itemController.transform.GetChild(i).GetComponent<SpriteRenderer>());
        }
       
        
    }

     void OnDisable()
    {
        
    }


    void Update()
    {
        if (currentSpriteRenderer == null)
            return;
            
            //当前物体，是否开启轮廓，轮廓颜色，轮廓采样偏移量（锐化程度）
            UpdateOutline(currentSpriteRenderer, startOutline, color,outlineOffset);
        
    }

    //是否开启轮廓，轮廓渐变
    void UpdateOutline(SpriteRenderer itemRenderer,bool gradualchangeColor,Color _color,float outlineOffset)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();

        itemRenderer.GetPropertyBlock(block);

        block.SetFloat("_Outline", gradualchangeColor ? 1f : 0);
        //使用全局变量的字符串
        block.SetFloat("_SampleOffset", outlineOffset);

        //设置渐变效果要设置为全局变量
        block.SetColor("_OutlineColor", _color);

        
        itemRenderer.SetPropertyBlock(block);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        OnPlayerEnterItem(collision,true);

      
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnPlayerEnterItem(collision,false);
    }

   
    void OnPlayerEnterItem(Collider2D collision,bool _isOutline)
    {
        if (collision.CompareTag("Item"))
        {
            foreach (SpriteRenderer itemRenderer in ItemSpriteRenderer)
            {
                if (itemRenderer.gameObject == collision.gameObject)
                {
                    currentSpriteRenderer = itemRenderer;

                    startOutline = _isOutline;
                }
            }
        }
    }
}
