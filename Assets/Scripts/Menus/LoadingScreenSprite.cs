using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;

    private bool playLoop;
    [SerializeField] private float animationSpeed;
    private float countdown;
    private int spriteCount = 0;

    // Start is called before the first frame update
    void OnAwake()
    {
        spriteCount = 0;
        playLoop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playLoop)
        {
            if (countdown >= animationSpeed) {
                this.GetComponent<Image>().sprite = sprites[spriteCount];
                
                countdown = 0;
                if (spriteCount < 4) {
                    spriteCount++;
                }
                else { spriteCount = 0; }
            }
            else {
                countdown += Time.fixedDeltaTime;
            }
        }
    }
}
