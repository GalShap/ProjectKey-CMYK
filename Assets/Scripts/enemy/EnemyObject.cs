using UnityEngine;

public abstract class EnemyObject : MonoBehaviour, ColorChangeListener
{
    [SerializeField] private bool oneHit;
    protected int lifeCount = 3;
    protected Rigidbody2D rb;
    protected Vector3 movement;
    protected bool onGround;
    protected bool grounLeft;
    protected bool grountRight;
    protected Vector2 collisionOffset;
    protected SpriteRenderer _renderer;
    protected Animator _animator;
    protected bool colored;
    protected Vector2? KickBackVector;
 

    // Start is called before the first frame update
    
    public virtual void Start()
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

    public bool IsOneHit
    {
        get => oneHit;
        set => oneHit = value;
    }


    public virtual void OnColorChange(ColorManager.ColorLayer layer)
    {
        colored = layer.index == gameObject.layer;
    }

    protected void isOnGround()
    {
        float height = _renderer.sprite.rect.height / _renderer.sprite.pixelsPerUnit;
        grounLeft = Physics2D.Raycast(
            rb.position + collisionOffset,
            Vector2.down,
            height * 0.5f + 0.05f,
            ColorManager.GroundLayers);

        grountRight = Physics2D.Raycast(
            rb.position - collisionOffset,
            Vector2.down,
            height * 0.5f + 0.05f,
            ColorManager.GroundLayers);

        onGround = grounLeft && grountRight;
    }

    public void SetKickBack(Vector2 newKick)
    {
        KickBackVector = newKick;
    }

   
    protected abstract void UponDead();
}