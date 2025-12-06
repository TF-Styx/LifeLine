using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Shared.WPF.Extensions
{
    public static class CollectionExtension
    {
        public static void Load<TObs>(this ObservableCollection<TObs> obs, List<TObs>? loadData, bool cleaning = false)
        {
            if (cleaning)
                obs.Clear();

            loadData ??= [];

            foreach (var item in loadData)
                obs.Add(item);
        }
    }
}
