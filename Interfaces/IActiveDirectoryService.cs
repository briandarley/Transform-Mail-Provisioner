namespace TransformNewMailProvisionerData.Interfaces
{
    public interface IActiveDirectoryService
    {
        Task<UNC.ActiveDirectory.Common.Entities.Entities.Entity> GetEntityByOnyen(string onyen);

    }
}