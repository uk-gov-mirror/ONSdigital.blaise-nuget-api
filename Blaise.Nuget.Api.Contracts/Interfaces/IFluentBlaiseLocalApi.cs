using StatNeth.Blaise.API.DataRecord;


namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseLocalApi
    {
        IFluentBlaiseLocalApi WithFile(string filePath);

        IDataRecord GetDataRecord(IKey key);

        void WriteDataRecord(IDataRecord dataRecord);
    }
}
