using bot.Models;
using System;
using System.Threading.Tasks;
namespace bot.Services
{
    public interface ICacheService
    {
        Task<(bool IsSuccess, PrayerTime prayerTime, Exception exception)> GetOrUpdatePrayerTimeAsync(long chatId, double longitude, double latitude);
        
    }
} 