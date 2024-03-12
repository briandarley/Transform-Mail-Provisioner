namespace TransformNewMailProvisionerData.Interfaces;

public interface IPidUpdateService
{
    Task Process(string filePath);
}
