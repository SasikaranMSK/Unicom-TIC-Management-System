using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnicornTICManagementSystem.Models;
using UnicornTICManagementSystem.Repositories;

namespace UnicornTICManagementSystem.Controllers
{
    public class TimetableController
    {
        private readonly DatabaseManager _databaseManager;

        public TimetableController()
        {
            _databaseManager = new DatabaseManager();
        }

        public async Task<List<Timetable>> GetAllTimetablesAsync()
        {
            return await _databaseManager.GetAllTimetablesAsync();
        }

        public async Task<List<Timetable>> GetTimetableByDayAsync(DayOfWeek dayOfWeek)
        {
            return await _databaseManager.GetTimetableByDayAsync(dayOfWeek);
        }

        public async Task<bool> AddTimetableEntryAsync(Timetable timetable)
        {
            try
            {
                if (await HasTimeConflictAsync(timetable))
                {
                    throw new Exception("Time conflict detected with existing schedule.");
                }
                return await _databaseManager.AddTimetableEntryAsync(timetable);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add timetable entry: {ex.Message}");
            }
        }

        public async Task<bool> DeleteTimetableEntryAsync(int id)
        {
            return await _databaseManager.DeleteTimetableEntryAsync(id);
        }

        private async Task<bool> HasTimeConflictAsync(Timetable newEntry)
        {
            var existingEntries = await GetTimetableByDayAsync(newEntry.DayOfWeek);
            
            foreach (var entry in existingEntries)
            {
                if (entry.Classroom == newEntry.Classroom &&
                    ((newEntry.StartTime >= entry.StartTime && newEntry.StartTime < entry.EndTime) ||
                     (newEntry.EndTime > entry.StartTime && newEntry.EndTime <= entry.EndTime)))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ValidateTimetable(Timetable timetable)
        {
            return timetable.CourseId > 0 &&
                   !string.IsNullOrWhiteSpace(timetable.Classroom) &&
                   timetable.StartTime < timetable.EndTime;
        }
    }
}