public class Stone : ADamagable
{
    void Update()
    {
        if (hp <= 0)
            gameObject.SetActive(false);
    }
}