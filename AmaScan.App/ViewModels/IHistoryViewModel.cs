using UWPCore.Framework.Mvvm;

namespace AmaScan.App.ViewModels
{
    public interface IHistoryViewModel
    {
        ICommand OpenInExternalBrowserCommand { get; }
        ICommand RemoveCommand { get; }
    }
}