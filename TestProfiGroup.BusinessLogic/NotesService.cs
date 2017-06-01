using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProfiGroup.DataAccess;
using TestProfiGroup.DomainModel;

namespace TestProfiGroup.BusinessLogic
{
    public class NotesService : INotesService
    {
        private readonly INotesRepository _repository;
        public const double MaxDatabaseSizeMb = 1.5;

        public NotesService(INotesRepository repository)
        {
            _repository = repository;
        }

        public Guid CreateLocal(Note note)
        {
            if (_repository.ReadDatabaseSize() > MaxDatabaseSizeMb)
            {
                throw new Exception("Maximum database size exceeded");
            }
            return _repository.Create(note);
        }

        public void UpdateLocal(Note note)
        {
            _repository.Update(note);
        }

        public void DeleteLocal(Guid id)
        {
            _repository.Delete(id);
        }

        public IList<Note> GetAllLocal()
        {
            return _repository.ReadAll().Where(x => !x.IsDeleted).ToList();
        }

        public Note GetLocal(Guid id)
        {
            return _repository.Read(id);
        }

        public void SynchronizeAll()
        {
            var localNotes = _repository.ReadAll().ToDictionary(x => x.Id);
            var serverNotes = DownloadList().ToDictionary(x => x.Id);

            foreach(var serverNote in serverNotes)
            {
                if (!localNotes.ContainsKey(serverNote.Key))
                {
                    localNotes.Add(serverNote.Key, serverNote.Value);
                    _repository.Create(serverNote.Value, true);
                }
                else
                {
                    var localNote = localNotes[serverNote.Key];

                    // process updated and deleted (empty) notes
                    if (localNote.DateChanged < serverNote.Value.DateChanged)
                    {
                        localNote.Title = serverNote.Value.Title;
                        localNote.Content = serverNote.Value.Content;
                        localNote.DateChanged = serverNote.Value.DateChanged;
                        _repository.Update(localNote, true);
                    }
                }
            }

            var mergedNotes = localNotes.Select(x => x.Value).ToList();
            PublishList(mergedNotes);
        }

        private const string EmployerKey = "vgjncqj2riuf";

        private void PublishList(IList<Note> notes)
        {
            var client = new RestClient("http://profigroup.by");
            var request = new RestRequest("applicants-tests/mobile/v2/{id}/", Method.PUT);
            request.AddUrlSegment("id", EmployerKey);
            request.AddHeader("Accept", "application/json; charset=utf-8");

            var jsonObj = notes.Select(x => new NoteDTO
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                DateChanged = x.DateChanged
            }).ToList();
            request.AddJsonBody(jsonObj);
            client.Execute(request);
        }

        private IList<Note> DownloadList()
        {
            var client = new RestClient("http://profigroup.by");
            var request = new RestRequest("applicants-tests/mobile/v2/{id}/", Method.GET);
            request.AddUrlSegment("id", EmployerKey);
            var response = client.Execute<List<NoteDTO>>(request);

            if (response.Data == null)
                return new List<Note>();

            return response.Data.Select(x => new Note
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                DateChanged = x.DateChanged
            }).ToList();
        }
    }
}
