public interface IAttackable
{
    public bool IsAlive { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; }

    void TakeDamage(int damage);
    void OnDie();
}