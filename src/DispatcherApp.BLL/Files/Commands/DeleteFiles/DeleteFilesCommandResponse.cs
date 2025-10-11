namespace DispatcherApp.BLL.Files.Commands.DeleteFiles;

public sealed class DeleteFilesCommandResponse
{
    public IEnumerable<string>? DeletedFileNames { get; set; }
}
