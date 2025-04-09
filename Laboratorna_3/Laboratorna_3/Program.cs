namespace Laboratorna_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bullet bullet = new Bullet(10);
            Enemy enemy = new Enemy(5);
            BreakableWall wall = new BreakableWall(15);

            bullet.HitTarget(enemy);
            bullet.HitTarget(wall);

            List<IDamageable> damageables = new List<IDamageable>();
            damageables.Add(enemy);
            damageables.Add(wall);

            foreach (IDamageable item in damageables)
            {
               
                    bullet?.HitTarget(item);
              
                if (item is BreakableWall)
                {
                    bullet?.HitTarget(item);
                }
                
            }
        }
    }
}
