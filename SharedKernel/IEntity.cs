using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel
{
    public interface IEntity
    {
        int Id { get; set; }
    }

    //public class BaseEntity
    //{
    //    //When the key name is Key
    //    [System.ComponentModel.DataAnnotations.Key]
    //    public int Key { get; set; }
    //
    //    //When the key name is Foo
    //    [System.ComponentModel.DataAnnotations.Key]
    //    public int Foo { get; set; }
    //}
}
