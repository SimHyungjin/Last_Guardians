using System.Collections;
using UnityEngine;

public class PopupBase : MonoBehaviour
{
    public bool IsInit = false;
    public bool IsOpen => gameObject.activeSelf;
    public virtual void NeedInit()
    {
        if (IsInit == false)
        {
            IsInit = true;
            Init();
        }
    }
    public virtual void Init() { IsInit = true; }
    public virtual void Open()
    {
        NeedInit();
        gameObject.SetActive(true);
        StartCoroutine(OpenedCoroutine());
    }
    public virtual void Opened() { }
    private IEnumerator OpenedCoroutine()
    {
        yield return null;
        Opened();
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
        Closed();
    }
    public virtual void Closed()
    {
    }
}
