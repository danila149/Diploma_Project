public class Enemy : ADamagable
{
    void Update()
    {
        if(hp <= 0)
            gameObject.SetActive(false);
    }
}
