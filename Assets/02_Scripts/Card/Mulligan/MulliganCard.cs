using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MulliganCard : MonoBehaviour
{
    //멀리건 시스템에 표시되는 카드
    public int TowerIndex { get; private set; }
    [SerializeField] private SpriteAtlas atlas;
    public Button Btn { get; set; }
    public Outline Outline { get; set; }


    private void Awake()
    {
        Btn = GetComponent<Button>();
        Outline = GetComponent<Outline>();
    }

    public void Init(int index)
    {
        TowerIndex = index;
        GetComponent<Image>().sprite = atlas.GetSprite($"Card_{index}");
        Outline.enabled = false;
    }
}
