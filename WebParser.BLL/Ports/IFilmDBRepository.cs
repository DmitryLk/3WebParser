using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebParser.Domain;

namespace WebParser.App
{

    public interface IFilmDBRepository : IDisposable
    {
        IEnumerable<Film> GetBookList();
        Film GetBook(int id);
        void Create(Film item);
        void Update(Film item);
        void Delete(int id);
        void Save();
    }



}
