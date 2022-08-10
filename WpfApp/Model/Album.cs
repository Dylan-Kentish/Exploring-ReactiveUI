namespace WpfApp.Model
{
    public class Album
    {
        public Album(int userId, int id, string title)
        {
            UserId = userId;
            Id = id;
            Title = title;
        }

        public int UserId { get; }
        public int Id { get; }
        public string Title { get; }
    }
}
