using Terminex.Common.Primitives;

namespace Shared.Client.Security.Abstraction
{
    public interface IKeyManager
    {
        void Clear();
        void ClearDek();
        Maybe<byte[]> GetDek();
        void SaveDek(byte[] value);
    }
}