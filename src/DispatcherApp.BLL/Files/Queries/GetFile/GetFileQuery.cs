using DispatcherApp.Common.DTOs.Files;
using MediatR;

namespace DispatcherApp.BLL.Files.Queries.GetFile;
// Include properties to be used as input for the query
public record GetFileQuery(int FileId) : IRequest<FileMetadataResponse>;
