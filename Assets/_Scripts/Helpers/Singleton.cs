using UnityEngine;

[DefaultExecutionOrder(-1)]
public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    // Die word geroep uit die perspektief van iets anders ( GameManager.Instance )
    // Dit will sê iets anders roep die GameManager bv die playerController, dus die gameManager.Instance
    static T instance;
    public static T Instance
    {
        get
        {
            if (!instance)
                instance = FindAnyObjectByType<T>();

            if (!instance)
            {
                GameObject obj = new GameObject(); 
                obj.name = typeof(T).Name;
                instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
            }

            return instance;
        }
    }

    // VS die wat uit die perspektief geroep word van die uit die Singleton script. 
    /// Met ander woorde: die GameManager se **Awake** word geroep

    // Awake = Pre -start ( gebeur voor start)
    protected virtual void Awake()
    {
        if (!instance)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);    
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
