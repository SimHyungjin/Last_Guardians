using UnityEngine;

public class GradeStar : MonoBehaviour
{
    ///////////==========================타워의 합성 레벨에 따른 별표시================================/////////////////////

    public int grade;
    public Sprite[] Stars;
    SpriteRenderer gradeStar;
    public void Init(int _grade)
    {
        grade = _grade;
        gradeStar = GetComponent<SpriteRenderer>();
        gradeStar.sprite = Stars[grade - 1];
    }
}
