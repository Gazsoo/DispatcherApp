using AutoMapper;
using DispatcherApp.BLL.Common.Interfaces;
using DispatcherApp.Common.DTOs.Files;
using MediatR;

namespace DispatcherApp.BLL.Files.Queries.GetFile;
internal sealed class GetFileQueryHandler(
    IFileService fileService,
    IMapper mapper
    ) : IRequestHandler<GetFileQuery, FileMetadataResponse>
{
    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;

    public async Task<FileMetadataResponse> Handle(GetFileQuery request, CancellationToken cancellationToken)
    {
        var file = await _fileService.GetFileMetadataAsync(request.FileId);
        return _mapper.Map<FileMetadataResponse>(file);
    }
}
