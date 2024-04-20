using UnityEngine;

public abstract class ADamagable : MonoBehaviour
{
    public int hp;
    public ResourceType dropType;

    public void GetDamage(int damage) =>
        hp -= damage;
}
