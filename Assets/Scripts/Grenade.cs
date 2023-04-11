public class Grenade : Explosive
{
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        Explode();
    }
}
