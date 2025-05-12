using UnityEngine;

public class UIBase : MonoBehaviour
{
    public bool IsOpen => gameObject.activeSelf;
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
    public virtual void Opened() { }
    public virtual void Init() { }
    public virtual void Close()
    { 
        gameObject.SetActive(false);
    }
    public virtual void Closed() { }
}
