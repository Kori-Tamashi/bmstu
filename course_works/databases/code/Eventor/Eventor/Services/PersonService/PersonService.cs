using Eventor.Common.Core;
using Eventor.Common.Converter;
using Eventor.Database.Repositories;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eventor.Database.Core;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;
    private readonly ILogger<PersonService> _logger;

    public PersonService(IPersonRepository personRepository, ILogger<PersonService> logger)
    {
        _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<Person>> GetAllPersonsAsync()
    {
        try
        {
            return await _personRepository.GetAllPersonsAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving persons");
            throw new PersonServiceException("Failed to retrieve persons", ex);
        }
    }

    public async Task<List<Person>> GetAllPersonsByDayAsync(Guid dayId)
    {
        try
        {
            return await _personRepository.GetAllPersonsByDayAsync(dayId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving persons for day {DayId}", dayId);
            throw new PersonServiceException($"Failed to retrieve persons for day {dayId}", ex);
        }
    }

    public async Task<List<Person>> GetAllPersonsByEventAsync(Guid eventId)
    {
        try
        {
            return await _personRepository.GetAllPersonsByEventAsync(eventId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving persons for event {EventId}", eventId);
            throw new PersonServiceException($"Failed to retrieve persons for event {eventId}", ex);
        }
    }

    public async Task<Person> GetPersonByIdAsync(Guid personId)
    {
        try
        {
            return await _personRepository.GetPersonByIdAsync(personId);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Person {PersonId} not found", personId);
            throw new PersonNotFoundException($"Person {personId} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving person {PersonId}", personId);
            throw new PersonServiceException($"Failed to retrieve person {personId}", ex);
        }
    }

    public async Task AddPersonAsync(Person person)
    {
        try
        {
            await _personRepository.InsertPersonAsync(person);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error creating person");
            throw new PersonCreateException("Failed to create person", ex);
        }
    }

    public async Task UpdatePersonAsync(Person updatePerson)
    {
        try
        {
            var existingPerson = await _personRepository.GetPersonByIdAsync(updatePerson.Id);
            await _personRepository.UpdatePersonAsync(existingPerson);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Person {PersonId} not found for update", updatePerson.Id);
            throw new PersonNotFoundException($"Person {updatePerson.Id} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating person {PersonId}", updatePerson.Id);
            throw new PersonUpdateException($"Failed to update person {updatePerson.Id}", ex);
        }
    }

    public async Task DeletePersonAsync(Guid personId)
    {
        try
        {
            await _personRepository.DeletePersonAsync(personId);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Person {PersonId} not found for deletion", personId);
            throw new PersonNotFoundException($"Person {personId} not found", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting person {PersonId}", personId);
            throw new PersonDeleteException($"Failed to delete person {personId}", ex);
        }
    }
}