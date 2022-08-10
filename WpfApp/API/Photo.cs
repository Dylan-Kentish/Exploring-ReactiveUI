﻿namespace WpfApp.API
{
    public struct Photo
    {
        public Photo(int id, string title, string url, string thumbnailUrl)
        {
            Id = id;
            Title = title;
            Url = url;
            ThumbnailUrl = thumbnailUrl;
        }

        public int Id { get; }
        public string Title { get; }
        public string Url { get; }
        public string ThumbnailUrl { get; }

    }
}
