using DispatcherApp.Common.DTOs.Files;
using MediatR;

namespace DispatcherApp.BLL.Files.Queries.GetFiles;
// Include properties to be used as input for the query
public record GetFilesQuery() : IRequest<IEnumerable<FileMetadataResponse>>;
