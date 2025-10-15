namespace Shared.WPF.ViewModels.Abstract
{
    public abstract class ObservableObject<TModel>(TModel model) : BaseViewModel where TModel : class
    {
        protected TModel _model = model ?? throw new ArgumentNullException(nameof(model));

        public abstract TModel ToModel();

        public virtual void CommitChanges(TModel newModel)
        {
            _model = newModel ?? throw new ArgumentNullException(nameof(newModel));
            OnPropertyChanged(string.Empty);
        }

        public abstract void RevertChanges();
    }
}
