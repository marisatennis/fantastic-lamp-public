using FantasticLamp.Models;
using SQLite;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FantasticLamp.Storage
{
    public class DatabaseFactory
    {
        static readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FantasticLamp.db");

        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(path, SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache, false);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public DatabaseFactory()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                await Database.CreateTablesAsync(CreateFlags.None, typeof(Image), typeof(Location), typeof(Mood), typeof(MoodLog), typeof(Play), typeof(RawPlay)).ConfigureAwait(false);
                if (Database.QueryScalarsAsync<int>("SELECT COUNT(*) FROM Location").Result.FirstOrDefault() == 0)
                {
                    await Database.InsertAllAsync(LocationStore.predefineLocations);
                }
                initialized = true;
            }
        }
        public SQLiteAsyncConnection GetDatabase()
        {
            return Database;
        }
    }
}
