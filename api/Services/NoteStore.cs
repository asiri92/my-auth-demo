using my_auth_api_demo.Models;

namespace my_auth_api_demo.Services
{
    public class NoteStore
    {
        private readonly List<Note> _notes = new()
        {
            new Note { Id = 1, Ttitle = "Note 1", Content = "This is the first note.", OwnerId = "20e26db0-da67-4e0d-aba6-d348356b03c9" },
            new Note { Id = 2, Ttitle = "Note 2", Content = "This is the second note.", OwnerId = Guid.NewGuid().ToString()},
            new Note { Id = 3, Ttitle = "Note 3", Content = "This is the third note.", OwnerId = Guid.NewGuid().ToString() }
        };

        public Note? GetById(int id) => _notes.FirstOrDefault(n => n.Id == id);
    }
}
