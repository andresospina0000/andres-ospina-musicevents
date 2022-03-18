using Microsoft.Extensions.Logging;
using MusicEvents.DataAccess.Repositories;
using MusicEvents.Dto.Request;
using MusicEvents.Dto.Response;
using MusicEvents.Entities;
using MusicEvents.Services.Interfaces;

namespace MusicEvents.Services.Implementations
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _repository;
        private readonly ILogger<GenreService> _logger;

        public GenreService(IGenreRepository repository, ILogger<GenreService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BaseResponseGeneric<ICollection<Genre>>> GetAsync()
        {
            var response = new BaseResponseGeneric<ICollection<Genre>>();
            try
            {
                response.Result = await _repository.GetCollectionAsync();
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

        public async Task<BaseResponseGeneric<Genre>> GetAsync(int id)
        {
            var response = new BaseResponseGeneric<Genre>();
            try
            {
                response.Result = await _repository.GetByIdAsync(id) ?? new Genre();
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

        public async Task<BaseResponseGeneric<int>> CreateAsync(DtoGenre request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                response.Result = await _repository.CreateAsync(new Genre
                {
                    Description = request.Description,
                    Status = true
                });

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

        public async Task<BaseResponseGeneric<int>> UpdateAsync(int id, DtoGenre request)
        {
            var response = new BaseResponseGeneric<int>();
            try
            {
                response.Result = await _repository.UpdateAsync(new Genre
                {
                    Id = id,
                    Description = request.Description,
                    Status = request.Status
                });
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
}
