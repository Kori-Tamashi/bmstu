using Eventor.Common.Core;
using Eventor.Common.Enums;
using Eventor.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving persons");
            throw new PersonServiceException("Unexpected error occurred", ex);
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving persons for day {DayId}", dayId);
            throw new PersonServiceException("Unexpected error occurred", ex);
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving persons for event {EventId}", eventId);
            throw new PersonServiceException("Unexpected error occurred", ex);
        }
    }

    public async Task<Person> GetPersonByIdAsync(Guid personId)
    {
        try
        {
            var person = await _personRepository.GetPersonByIdAsync(personId);
            if (person == null)
            {
                _logger.LogError("Person {PersonId} not found", personId);
                throw new PersonNotFoundException($"Person {personId} not found");
            }
            return person;
        }
        catch (PersonNotFoundException)
        {
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error retrieving person {PersonId}", personId);
            throw new PersonServiceException($"Failed to retrieve person {personId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving person {PersonId}", personId);
            throw new PersonServiceException("Unexpected error occurred", ex);
        }
    }

    public async Task AddPersonAsync(Person person)
    {
        try
        {
            ValidatePerson(person);
            await _personRepository.InsertPersonAsync(person);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Validation error creating person");
            throw new PersonCreateException("Invalid person data: " + ex.Message, ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error creating person");
            throw new PersonCreateException("Failed to create person", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating person");
            throw new PersonCreateException("Unexpected error occurred", ex);
        }
    }

    public async Task UpdatePersonAsync(Person updatePerson)
    {
        try
        {
            ValidatePerson(updatePerson);
            var existingPerson = await GetExistingPerson(updatePerson.Id);
            await _personRepository.UpdatePersonAsync(existingPerson);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Validation error updating person {PersonId}", updatePerson.Id);
            throw new PersonUpdateException("Invalid person data: " + ex.Message, ex);
        }
        catch (PersonNotFoundException)
        {
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating person {PersonId}", updatePerson.Id);
            throw new PersonUpdateException($"Failed to update person {updatePerson.Id}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating person {PersonId}", updatePerson.Id);
            throw new PersonUpdateException("Unexpected error occurred", ex);
        }
    }

    public async Task DeletePersonAsync(Guid personId)
    {
        try
        {
            var existingPerson = await GetExistingPerson(personId);
            await _personRepository.DeletePersonAsync(personId);
        }
        catch (PersonNotFoundException)
        {
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting person {PersonId}", personId);
            throw new PersonDeleteException($"Failed to delete person {personId}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting person {PersonId}", personId);
            throw new PersonDeleteException("Unexpected error occurred", ex);
        }
    }

    private async Task<Person> GetExistingPerson(Guid personId)
    {
        var person = await _personRepository.GetPersonByIdAsync(personId);
        return person ?? throw new PersonNotFoundException($"Person {personId} not found");
    }

    private void ValidatePerson(Person person)
    {
        if (string.IsNullOrWhiteSpace(person.Name))
            throw new ArgumentException("Person name is required");
    }
}