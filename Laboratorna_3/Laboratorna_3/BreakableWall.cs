using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorna_3
{
    public class BreakableWall : IDamageable
    {
        private int durability;
        public BreakableWall(int durability)
        {
            this.durability = durability;
        }

        public int Durability => durability;

        public void TakeDamage(int amount)
        {
            durability -= amount;
            if (durability < 0) durability = 0;
            Console.WriteLine($"Wall takes {amount} damage. Durability left: {durability}");

            if (durability <= 0) Console.WriteLine("Wall is destroyed\n");
            
            else Console.WriteLine("Wall is not destroyed\n");
        }
    }

}
