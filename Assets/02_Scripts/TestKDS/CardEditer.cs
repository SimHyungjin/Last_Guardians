using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEditer : MonoBehaviour
{
    public DeckHandler handCardLayout;

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
    public void AddSix()
    { handCardLayout.AddCard(6); }
    public void AddSeven()
    { handCardLayout.AddCard(7); }
    public void AddEight()
    { handCardLayout.AddCard(8); }
    public void AddNine()
    { handCardLayout.AddCard(9); }
    public void AddTen()
    { handCardLayout.AddCard(10); }

    //public void SubOen()
    //{ handCardLayout.UseCard(1); }
    //public void SubTwo()
    //{ handCardLayout.UseCard(2); }
    //public void SubThree()
    //{ handCardLayout.UseCard(3); }
    //public void SubFour()
    //{ handCardLayout.UseCard(4); }
    //public void SubFive()
    //{ handCardLayout.UseCard(5); }
}

