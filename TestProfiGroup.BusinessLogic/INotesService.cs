using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProfiGroup.DomainModel;

namespace TestProfiGroup.BusinessLogic
{
    public interface INotesService
    {
        IList<Note> GetAllLocal();
        Note GetLocal(Guid id);
        Guid CreateLocal(Note note);
        void UpdateLocal(Note note);
        void DeleteLocal(Guid id);
        void SynchronizeAll();
    }
}
