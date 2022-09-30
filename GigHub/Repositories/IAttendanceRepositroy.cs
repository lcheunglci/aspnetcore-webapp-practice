using GigHub.Models;

namespace GigHub.Repositories
{
    public interface IAttendanceRepositroy
    {
        Attendance GetAttendance(int gigId, string userId);
        IEnumerable<Attendance> GetFutureAttendances(string userId);
    }
}