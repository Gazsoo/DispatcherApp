using AutoMapper;
using DispatcherApp.API.Controllers;
using MediatR;

namespace DispatcherApp.BLL.Files.Queries.GetFiles;
internal sealed class GetFilesQueryHandler(
    IFileService fileService,
    IMapper mapper
    ) : IRequestHandler<GetFilesQuery, IEnumerable<FileMetadataResponse>>
{

    private readonly IFileService _fileService = fileService;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<FileMetadataResponse>> Handle(GetFilesQuery request, CancellationToken cancellationToken)
    {
        var file = await _fileService.GetFilesMetadataAsync();
        return _mapper.Map<IEnumerable<FileMetadataResponse>>(file);
    }
}
