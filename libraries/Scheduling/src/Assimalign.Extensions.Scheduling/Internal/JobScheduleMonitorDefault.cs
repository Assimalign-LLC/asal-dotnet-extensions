using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Scheduling.Internal;

internal sealed class JobScheduleMonitorDefault : JobScheduleMonitor
{
    private const string storageDefaultPath = ".job.scheduling";

    private static readonly IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
    private static readonly ConcurrentDictionary<string, IsolatedStorageFileStream> storageFiles = new();

    public JobScheduleMonitorDefault()
    {
        if (!storage.DirectoryExists(storageDefaultPath))
        {
            storage.CreateDirectory(storageDefaultPath);
        }
    }

    public override Task<JobScheduleStatus> GetStatusAsync(string scheduleId)
    {
        try
        {
            var storageFile = GetStorageFile(scheduleId);

            if (storageFile.Length == 0)
            {
                return Task.FromResult<JobScheduleStatus>(null);
            }

            storageFile.Position = 0;

            return JsonSerializer.DeserializeAsync<JobScheduleStatus>(storageFile).AsTask();
        }
        catch
        {
            // best effort
            return Task.FromResult<JobScheduleStatus>(null);
        }
    }
    public override Task UpdateStatusAsync(string scheduleId, JobScheduleStatus scheduleStatus)
    {
        try
        {
            var storageFile = GetStorageFile(scheduleId);

            storageFile.Position = 0;

            return JsonSerializer.SerializeAsync(storageFile, scheduleStatus);
        }
        catch
        {
            // best effort
            return Task.FromResult(true);
        }
    }

    private IsolatedStorageFileStream GetStorageFile(string scheduleId)
    {
        return storageFiles.GetOrAdd(scheduleId, id =>
        {
            return storage.CreateFile($"{scheduleId}.status");
        });
    }
}
