using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProfiGroup.DomainModel;

namespace TestProfiGroup.DataAccess
{
    public interface INotesRepository
    {
        IList<Note> ReadAll();
        Note Read(Guid id);
        double ReadDatabaseSize();
        Guid Create(Note note, bool useDateChanged = false);
        void Update(Note note, bool useDateChanged = false);
        void Delete(Guid id);
    }
}
