
using UnityEngine;

public static class LayerMaskData
{ 
    public static readonly LayerMask monster = LayerMask.GetMask("Monster");
    public static readonly LayerMask tower = LayerMask.GetMask("Tower");
    public static readonly LayerMask buildBlock= LayerMask.GetMask("Tower","Obstacle","Center");
    public static readonly LayerMask trapObject = LayerMask.GetMask("TrapObject");
}
