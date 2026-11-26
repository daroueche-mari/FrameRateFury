using System.Collections.Generic;

// Tout objet rangé dans une EntityList retient sa position dans la liste.
public interface IListed
{
    int ListIndex { get; set; }
}

// Liste où l'ajout et la suppression sont instantanés
public class EntityList<T> where T : class, IListed
{
    private List<T> items = new List<T>();

    public int Count
    {
        get { return items.Count; }
    }

    public T this[int index]
    {
        get { return items[index]; }
    }

    public void Add(T item)
    {
        if (Contains(item))
        {
            return;
        }

        item.ListIndex = items.Count;
        items.Add(item);
    }

    public void Remove(T item)
    {
        if (!Contains(item))
        {
            return;
        }

        int index = item.ListIndex;
        int lastIndex = items.Count - 1;
        T lastItem = items[lastIndex];

        items[index] = lastItem;
        lastItem.ListIndex = index;
        items.RemoveAt(lastIndex);

        item.ListIndex = -1;
    }

    private bool Contains(T item)
    {
        int index = item.ListIndex;

        if (index < 0 || index >= items.Count)
        {
            return false;
        }

        return items[index] == item;
    }
}

// Toutes les entités actives du jeu. Sert aux collisions et au compteur d'entités du HUD.
public static class EntityRegistry
{
    public static readonly EntityList<Enemy> Enemies = new EntityList<Enemy>();
    public static readonly EntityList<Projectile> PlayerProjectiles = new EntityList<Projectile>();
    public static readonly EntityList<Projectile> EnemyProjectiles = new EntityList<Projectile>();

    // Valeur à afficher dans le HUD.
    public static int ActiveCount
    {
        get { return Enemies.Count + PlayerProjectiles.Count + EnemyProjectiles.Count; }
    }

    // Retire tous les ennemis et toutes les balles 
    public static void ReleaseAll()
    {
        for (int i = Enemies.Count - 1; i >= 0; i--)
        {
            Enemies[i].Despawn();
        }

        for (int i = PlayerProjectiles.Count - 1; i >= 0; i--)
        {
            PlayerProjectiles[i].Release();
        }

        for (int i = EnemyProjectiles.Count - 1; i >= 0; i--)
        {
            EnemyProjectiles[i].Release();
        }
    }
}