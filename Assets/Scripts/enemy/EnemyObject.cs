using UnityEngine;

public abstract class EnemyObject : MonoBehaviour, ColorChangeListener
{
    protected int lifeCount = 3;
    protected abstract void UponDead();
    protected Rigidbody2D rb;
    protected Vector3 movement;
    
    // Start is called before the first frame update
    void Start()
    {
        ColorManager.RegisterColorListener(this);
    }
    
    public bool isAlive()
    {
        if (lifeCount > 0)
        {
            return true;
        }
        return false;
    }


    public void OnColorChange(ColorManager.ColorLayer layer)
    {
        return;
    }
}
