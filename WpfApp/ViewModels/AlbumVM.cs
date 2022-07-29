using WpfApp.Model;

namespace WpfApp.ViewModels
{
    public class AlbumVM
    {
        private readonly Album _model;

        public AlbumVM(Album model)
        {
            _model = model;
        }

        public string Title => _model.Title;
    }
}
