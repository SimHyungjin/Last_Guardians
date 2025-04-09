using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEditer : MonoBehaviour
{
    public HandCardLayout handCardLayout;

    public void AddOne()
    {handCardLayout.AddCard(1); }
    public void AddTwo()
    { handCardLayout.AddCard(2); }
    public void AddThree()
    { handCardLayout.AddCard(3); }
    public void AddFour()
    { handCardLayout.AddCard(4); }
    public void AddFive()
    { handCardLayout.AddCard(5); }

    public void SubOen()
    { handCardLayout.UseCard(1); }
    public void SubTwo()
    { handCardLayout.UseCard(2); }
    public void SubThree()
    { handCardLayout.UseCard(3); }
    public void SubFour()
    { handCardLayout.UseCard(4); }
    public void SubFive()
    { handCardLayout.UseCard(5); }
}

