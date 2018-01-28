using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class AnimateSpriteMenu : MonoBehaviour {

    private UnityEngine.UI.Image image;

    [SerializeField] private Sprite[] frames;
    [SerializeField] private float animationTime = .5f;
    private float time = 0f;
    private int currentIndex = 0;

    void Start()
    {
        image = GetComponent<UnityEngine.UI.Image>();
    }

	void Update ()
    {
        if (frames.Length > 0 && image != null)
        {
            time += Time.deltaTime;
            if (time > animationTime)
            {
                time -= animationTime;
                currentIndex++;
                if (currentIndex > frames.Length - 1)
                    currentIndex = 0;
            }
            image.sprite = frames[currentIndex];
        }
	}
}
