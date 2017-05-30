using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Conseg.Administracao.Domain.Core
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public byte[] RowVersion { get; set; }

        public override bool Equals(object obj)
        { return Equals(obj as BaseEntity); }

        public static bool IsTransient(BaseEntity obj)
        { return obj != null && Equals(obj.Id, default(int)); }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(int)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        { return Equals(x, y); }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        { return Equals(x, y); }

    }
}
