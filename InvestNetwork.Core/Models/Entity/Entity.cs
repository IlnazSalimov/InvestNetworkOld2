using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestNetwork.Core
{
    public abstract class BaseEntity<T>
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }
    }
}
