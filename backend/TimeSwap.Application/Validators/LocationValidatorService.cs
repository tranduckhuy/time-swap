using Microsoft.Extensions.Logging;
using TimeSwap.Application.Exceptions.Location;
using TimeSwap.Domain.Interfaces.Repositories;

namespace TimeSwap.Application.Validators
{
    public class LocationValidatorService
    {
        private readonly IWardRepository _wardRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ILogger<LocationValidatorService> _logger;

        public LocationValidatorService(
            IWardRepository wardRepository,
            ICityRepository cityRepository,
            ILogger<LocationValidatorService> logger)
        {
            _wardRepository = wardRepository;
            _cityRepository = cityRepository;
            _logger = logger;
        }

        public async Task ValidateWardAndCityAsync(string? wardId, string? cityId)
        {
            if (!string.IsNullOrEmpty(wardId) && string.IsNullOrEmpty(cityId))
            {
                _logger.LogWarning("CityId is required when WardId is provided.");
                throw new CityIdRequireWhenWardIdProvidedException();
            }

            if (string.IsNullOrEmpty(wardId) && !string.IsNullOrEmpty(cityId))
            {
                _logger.LogWarning("WardId is required when CityId is provided.");
                throw new WardIdRequireWhenCityIdProvidedException();
            }

            if (!string.IsNullOrEmpty(wardId) && !string.IsNullOrEmpty(cityId))
            {
                var ward = await _wardRepository.GetByIdAsync(wardId);
                if (ward == null)
                {
                    _logger.LogWarning("Ward with id {WardId} not found", wardId);
                    throw new WardNotFoundException();
                }

                var city = await _cityRepository.GetByIdAsync(cityId);
                if (city == null)
                {
                    _logger.LogWarning("City with id {CityId} not found", cityId);
                    throw new CityNotFoundException();
                }

                var isValidWardInCity = await _wardRepository.ValidateWardInCityAsync(wardId, cityId);
                if (!isValidWardInCity)
                {
                    _logger.LogWarning("Ward with id {WardId} is not valid in City with id {CityId}", wardId, cityId);
                    throw new InvalidWardInCityException();
                }
            }
        }
    }
}
