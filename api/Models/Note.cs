namespace my_auth_api_demo.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Ttitle { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string OwnerId { get; set; } = string.Empty; // Entra 'oid' claim of the user who owns the note
    }
}
