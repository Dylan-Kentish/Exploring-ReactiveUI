using ReactiveUI;
using System;
using System.Reactive.Disposables;
using WpfApp.Model;

namespace WpfApp.ViewModels
{
    public sealed class AccountVM : ReactiveObject, IDisposable
    {
        private readonly CompositeDisposable _disposables;
        private readonly ObservableAsPropertyHelper<User> _user;
        public AccountVM(IObservable<User> user)
        {
            _disposables = new CompositeDisposable();
            user.ToProperty(this, x => x.User, out _user);
        }

        public User User => _user.Value;

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
