using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorna_3
{
    public class Bullet : Projectile
    {
        public Bullet(int damage) : base(damage) { }

        public override void HitTarget(IDamageable target)
        {
            if (target is Enemy enemy)
            {
                Console.WriteLine($"Enemy's health before hit: {enemy.Health}");
            }
            else if (target is BreakableWall wall)
            {
                Console.WriteLine($"Wall's health before hit: {wall.Durability}");
            }

            Console.WriteLine("Bullet hits the target");
            target.TakeDamage(damage);
        }

    }
}
