using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DispatcherApp.BLL.Files.Queries.DownloadFile;
// Include properties to be used as input for the query
public record DownloadFileQuery(int Id) : IRequest<FileContentResult>;
