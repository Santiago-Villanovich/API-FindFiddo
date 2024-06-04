using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFiddo.Abstractions
{
    public interface ICrud<T>
    {
        T Save (T entity);
        IList<T> GetAll ();
        T GetById (int id);
        void DeleteById (int id);
    }
}
