public interface IAttackable
{
    public int Health { get; set; }
    public int MaxHealth { get; }

    void TakeDamage(int damage);
}