namespace WpfApp.API
{
    public struct Album
    {
        public Album(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public int Id { get; }
        public string Title { get; }
    }
}
