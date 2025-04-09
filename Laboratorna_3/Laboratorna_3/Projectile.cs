using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorna_3
{
    public abstract class Projectile
    {
        public int damage;

        public Projectile(int damage)
        {
            this.damage = damage;
        }

        public abstract void HitTarget(IDamageable target);
    }
}
