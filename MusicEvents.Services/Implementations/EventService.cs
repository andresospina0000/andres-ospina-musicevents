using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using MusicEvents.DataAccess.Repositories;
using MusicEvents.Dto.Request;
using MusicEvents.Dto.Response;
using MusicEvents.Entities;
using MusicEvents.Entities.Complex;
using MusicEvents.Services.Interfaces;

namespace MusicEvents.Services.Implementations;

public class EventService : IEventService
{
    private readonly IEventRepository _repository;
    private readonly IFileUploader _fileUploader;
    private readonly ILogger<EventService> _logger;

    public EventService(IEventRepository repository,
        IFileUploader fileUploader,
        ILogger<EventService> logger)
    {
        _repository = repository;
        _fileUploader = fileUploader;
        _logger = logger;
    }

    public async Task<BaseCollectionResponse<ICollection<ConcertInfo>>> GetAsync(string filter, int page, int rows)
    {
        var response = new BaseCollectionResponse<ICollection<ConcertInfo>>();
        try
        {
            var tuple = await _repository.GetCollectionAsync(filter, page, rows);
            response.Result = tuple.Collection.ToList();
            response.TotalPages = Utils.GetTotalPages(tuple.Total, rows);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.StackTrace);
            response.Success = false;
            response.Errors.Add(ex.Message);
        }

        return response;

    }

    public async Task<BaseResponseGeneric<ICollection<ConcertInfo>>> GetByGenreAsync(int id)
    {
        var response = new BaseResponseGeneric<ICollection<ConcertInfo>>();
        try
        {

            response.Result = await _repository.GetCollectionByGenre(id);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.StackTrace);
            response.Success = false;
            response.Errors.Add(ex.Message);
        }
        return response;
    }

    public async Task<BaseResponseGeneric<ICollection<ConcertMinimalInfo>>> GetMinimalByGenreAsync(int genreId)
    {
        var response = new BaseResponseGeneric<ICollection<ConcertMinimalInfo>>();
        try
        {

            response.Result = await _repository.GetMinimalCollectionByGenre(genreId);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.StackTrace);
            response.Success = false;
            response.Errors.Add(ex.Message);
        }
        return response;
    }

    public async Task<BaseResponseGeneric<Concert>> GetAsync(int id)
    {
        var response = new BaseResponseGeneric<Concert>();
        try
        {
            response.Result = await _repository.GetByIdAsync(id) ?? new Concert();
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.StackTrace);
            response.Success = false;
            response.Errors.Add(ex.Message);
        }
        return response;
    }

    public async Task<BaseResponseGeneric<int>> CreateAsync(DtoEvent request)
    {
        var response = new BaseResponseGeneric<int>();
        try
        {
            var concert = new Concert
            {
                Title = request.Title,
                Description = request.Description,
                Place = request.Place,
                DateEvent = Convert.ToDateTime($"{request.Date} {request.Time}"),
                TicketsQuantity = request.TicketsQuantity,
                GenreId = request.GenreId,
                UnitPrice = request.UnitPrice,
                Status = true
            };

            if (!string.IsNullOrEmpty(request.FileName))
            {
                concert.ImageUrl = await _fileUploader.UploadFileAsync(request.ImageBase64, request.FileName);
            }

            response.Result = await _repository.CreateAsync(concert);

            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.StackTrace);
            response.Success = false;
            response.Errors.Add(ex.Message);
        }
        return response;
    }

    public async Task<BaseResponse> FinalizeAsync(int id)
    {
        var response = new BaseResponse();

        try
        {
            await _repository.Finalize(id);

            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            response.Success = false;
            response.Errors.Add(ex.Message);
        }

        return response;
    }

    public async Task<BaseResponseGeneric<int>> UpdateAsync(int id, DtoEvent request)
    {
        var response = new BaseResponseGeneric<int>();
        try
        {
            var concert = new Concert
            {
                Id = id,
                Title = request.Title,
                Description = request.Description,
                Place = request.Place,
                DateEvent = Convert.ToDateTime($"{request.Date} {request.Time}"),
                TicketsQuantity = request.TicketsQuantity,
                GenreId = request.GenreId,
                UnitPrice = request.UnitPrice,
                Status = true
            };

            if (!string.IsNullOrEmpty(request.FileName))
            {
                concert.ImageUrl = await _fileUploader.UploadFileAsync(request.ImageBase64, request.FileName);
            }

            response.Result = await _repository.UpdateAsync(concert);
            response.Success = true;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.StackTrace);
            response.Success = false;
            response.Errors.Add(ex.Message);
        }
        return response;
    }

    public async Task<BaseResponseGeneric<int>> DeleteAsync(int id)
    {
        var response = new BaseResponseGeneric<int>();
        try
        {
            await _repository.DeleteAsync(id);

            response.Success = true;
            response.Result = id;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.StackTrace);
            response.Success = false;
            response.Errors.Add(ex.Message);
        }
        return response;
    }
}