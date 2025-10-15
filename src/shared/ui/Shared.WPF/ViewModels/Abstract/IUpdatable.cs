using Shared.WPF.Enums;

namespace Shared.WPF.ViewModels.Abstract
{
    public interface IUpdatable
    {
        void Update<TData>(TData value, TransmittingParameter parameter);
    }
}
