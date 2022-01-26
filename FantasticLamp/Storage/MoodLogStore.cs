using FantasticLamp.Models;
using SQLite;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FantasticLamp.Storage
{
    public class MoodLogStore
    {
        SQLiteAsyncConnection Database => DependencyService.Get<DatabaseFactory>().GetDatabase();
        MoodStore MoodStore => DependencyService.Get<MoodStore>();
        public async Task<bool> LogMoodAsync(Mood mood)
        {
            var moodLog = new MoodLog();
            moodLog.MoodId = mood.Id;
            moodLog.DateTime = DateTime.UtcNow;
            await Database.InsertAsync(moodLog);
            return await Task.FromResult(true);
        }
        public async Task<Mood> GetMoodAtDateTimeAsync(DateTime dateTime)
        {
            MoodLog moodLog = await Database.Table<MoodLog>().Where(row => row.DateTime <= dateTime).OrderByDescending(top => top.DateTime).Take(1).FirstOrDefaultAsync();
            if (moodLog == null)
            {
                return null;
            }

            // Don't accept moods over two hours old
            if (moodLog.DateTime < dateTime.AddHours(-2))
            {
                return null;
            }

            return await MoodStore.GetMoodAsync(moodLog.MoodId);
        }
    }
}
