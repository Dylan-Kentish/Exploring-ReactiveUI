using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using WpfApp.Model;

namespace WpfApp.ViewModels;

public sealed class AccountVM : ReactiveObject, IDisposable
{
    private readonly CompositeDisposable _disposables;
    private readonly ObservableAsPropertyHelper<UserVM> _user;

    public AccountVM(IObservable<User?> user)
    {
        _disposables = new CompositeDisposable();

        user.WhereNotNull()
            .Select(user => new UserVM(user))
            .ToProperty(this, x => x.User, out _user)
            .DisposeWith(_disposables);
    }

    public UserVM User => _user.Value;

    public void Dispose()
    {
        _disposables.Dispose();
    }
}