using AutoMapper;
using Booking.Application.Common.Exceptions;
using Booking.Application.Repositories;
using Booking.Domain.Entities;
using MediatR;

namespace Booking.Application.Features.TableFeatures;

public sealed class CreateTableHandler : IRequestHandler<CreateTableRequest, CreateTableResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITableRepository _tableRepository;
    private readonly IMapper _mapper;

    public CreateTableHandler(IUnitOfWork unitOfWork, ITableRepository tableRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _tableRepository = tableRepository;
        _mapper = mapper;
    }
    
    public async Task<CreateTableResponse> Handle(CreateTableRequest request, CancellationToken cancellationToken)
    {
        if (await _tableRepository.HasAnyByFilialIdAndName(request.FilialId, request.Name, cancellationToken))
            throw new NotFoundException($"Table with FilialId {request.FilialId} and Name {request.Name} already exists");
            
        var table = _mapper.Map<Table>(request);
        
        _tableRepository.Create(table);
        await _unitOfWork.Save(cancellationToken);

        return _mapper.Map<CreateTableResponse>(table);
    }
}