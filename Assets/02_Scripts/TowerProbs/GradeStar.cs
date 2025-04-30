using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradeStar : MonoBehaviour
{
    public int grade;
    public Sprite[] Stars;
    SpriteRenderer gradeStar;
    public void Init(int _grade)
    {
        grade = _grade;
        gradeStar = GetComponent<SpriteRenderer>();
        gradeStar.sprite = Stars[grade-1];
    }
}
