using UnityEngine;

public abstract class EnemyObject : MonoBehaviour, ColorChangeListener
{
    protected int lifeCount = 3;
    protected abstract void UponDead();
    protected Rigidbody2D rb;
    protected Vector3 movement;
    protected bool onGround;
    protected bool grounLeft;
    protected bool grountRight;
    protected Vector2 collisionOffset;
    [SerializeField] protected LayerMask groundLayers;
    protected SpriteRenderer _renderer;
    protected bool colored;

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


    public virtual void OnColorChange(ColorManager.ColorLayer layer)
    {
        // base.OnColorChange(layer);
        colored = layer.index == gameObject.layer;
    }

    protected void isOnGround()
    {
        float height = _renderer.sprite.rect.height / _renderer.sprite.pixelsPerUnit;
        grounLeft = Physics2D.Raycast(
            rb.position + collisionOffset,
            Vector2.down,
            height * 0.5f + 0.05f,
            groundLayers);

        grountRight = Physics2D.Raycast(
            rb.position - collisionOffset,
            Vector2.down,
            height * 0.5f + 0.05f,
            groundLayers);

        onGround = grounLeft && grountRight;
    }
}