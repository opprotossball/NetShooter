public interface IDamagable {
    float Health { get; set; }
    void Damage(float damage);
}