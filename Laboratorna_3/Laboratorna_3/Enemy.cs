using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorna_3
{
    public class Enemy : IDamageable
    {
        private int health;

        public Enemy(int health)
        {
            this.health = health;
        }

        public int Health => health;

        public void TakeDamage(int amount)
        {
            health -= amount;
            if (health < 0) health = 0;
            Console.WriteLine($"Enemy takes {amount} damage. Health left: {health}");

            if (health <= 0) Console.WriteLine("Enemy is dead\n");
            
            else Console.WriteLine("Enemy is alive\n");
        }
    }
}
