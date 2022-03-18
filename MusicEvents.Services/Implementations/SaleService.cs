using System.Globalization;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicEvents.DataAccess.Repositories;
using MusicEvents.Dto.Request;
using MusicEvents.Dto.Response;
using MusicEvents.Entities;
using MusicEvents.Entities.Complex;
using MusicEvents.Services.Interfaces;

namespace MusicEvents.Services.Implementations;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _repository;
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SaleService> _logger;

    public SaleService(ISaleRepository repository, 
        ILogger<SaleService> logger, 
        IEventRepository eventRepository,
        IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<BaseResponseGeneric<int>> CreateAsync(DtoSale request, string userId)
    {
        var response = new BaseResponseGeneric<int>();

        try
        {
            var concert = await _eventRepository.GetByIdAsync(request.EventId);

            if (concert == null)
            {
                response.Errors.Add($"El Id {request.EventId} del Evento no existe");
                response.Success = false;
                return response;
            }

            //if (concert.DateEvent < DateTime.Today)
            //{
            //    response.Errors.Add($"El Evento {concert.Title} ya finalizó el día {concert.DateEvent:D}");
            //    response.Success = false;
            //    return response;
            //}

            var entity = new Sale
            {
                SaleDate = DateTime.Now,
                ConcertId = request.EventId,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice,
                TotalSale = request.Quantity * request.UnitPrice,
                Status = true,
                UserId = userId
            };

            response.Result = await _repository.CreateAsync(entity);
            response.Success = true;

        }
        catch (Exception ex)
        {
            response.Errors.Add(ex.Message);
            response.Success = false;
            _logger.LogCritical(ex.Message);
        }

        return response;
    }

    public async Task<BaseResponseGeneric<ICollection<DtoSaleInfo>>> GetSaleById(int id)
    {
        var response = new BaseResponseGeneric<ICollection<DtoSaleInfo>>();

        try
        {
            var collection = await _repository.GetSaleById(id);

            response.Result = collection
                .Select(p => _mapper.Map<DtoSaleInfo>(p))
                .ToList();

            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Errors.Add(ex.Message);
            response.Success = false;
            _logger.LogCritical(ex.Message);
        }

        return response;
    }

    public async Task<BaseResponseGeneric<ICollection<DtoSaleInfo>>> GetCollection(int genreId, string dateInit, string dateEnd)
    {
        var response = new BaseResponseGeneric<ICollection<DtoSaleInfo>>();

        try
        {
            var englishFormat = new CultureInfo("en-US");

            var collection = await _repository.GetSaleCollection(genreId, 
                string.IsNullOrEmpty(dateInit) ? null : Convert.ToDateTime(dateInit, englishFormat),
                string.IsNullOrEmpty(dateEnd) ? null : Convert.ToDateTime(dateEnd, englishFormat));

            response.Result = collection
                .Select(p => _mapper.Map<DtoSaleInfo>(p))
                .ToList();

            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Errors.Add(ex.Message);
            response.Success = false;
            _logger.LogCritical(ex.Message);
        }

        return response;
    }

    public async Task<BaseResponseGeneric<ICollection<DtoSaleInfo>>> GetSaleByUserId(string userId)
    {
        var response = new BaseResponseGeneric<ICollection<DtoSaleInfo>>();

        try
        {
            var collection = await _repository.GetSaleByUserId(userId);

            response.Result = collection
                .Select(p => _mapper.Map<DtoSaleInfo>(p))
                .ToList();

            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Errors.Add(ex.Message);
            response.Success = false;
            _logger.LogCritical(ex.Message);
        }

        return response;
    }

    public async Task<BaseResponseGeneric<ICollection<ReportSaleInfo>>> GetReportSales(int genreId, string dateInit, string dateEnd)
    {
        var response = new BaseResponseGeneric<ICollection<ReportSaleInfo>>();

        try
        {
            response.Result = await _repository.GetReportSale(genreId, Convert.ToDateTime(dateInit), Convert.ToDateTime(dateEnd));
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Errors.Add(ex.Message);
            response.Success = false;
            _logger.LogCritical(ex.Message);
        }

        return response;
    }
}