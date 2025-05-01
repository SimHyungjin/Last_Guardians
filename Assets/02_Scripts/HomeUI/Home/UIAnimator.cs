using UnityEngine;
using UnityEngine.UI;

public class UIAnimator : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float frameRate = 0.1f;

    private int currentFrame;
    private float timer;

    private void Awake()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        if (sprites.Length == 0)
        {
            Debug.LogWarning("No sprites assigned to UIAnimator.");
            enabled = false;
        }
    }
    private void Update()
    {
        if (sprites.Length == 0 || image == null) return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % sprites.Length;
            image.sprite = sprites[currentFrame];
        }
    }
}
