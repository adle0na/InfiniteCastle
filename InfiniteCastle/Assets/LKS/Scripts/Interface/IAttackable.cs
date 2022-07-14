public delegate void HpDelegate();

public interface IAttackable
{
    public bool IsAlive { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; }
    public int Attack { get; set; }
    
    public HpDelegate onHpChange { get; set; }

    void TakeDamage(int damage);
    void OnDie();
}