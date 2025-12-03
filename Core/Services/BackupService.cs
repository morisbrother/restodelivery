using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ASConfigurator.Core.Services
{
    public class BackupService
    {
        private readonly string _backupFolder = Path.Combine("C:\\ProgramData\\ASConfig", "backups");

        public BackupService()
        {
            Directory.CreateDirectory(_backupFolder);
        }

        public async Task<string> CreateBackupAsync()
        {
            var path = Path.Combine(_backupFolder, $"secedit-{DateTime.UtcNow:yyyyMMddHHmmss}.inf");
            await File.WriteAllTextAsync(path, "Simulated secedit /export output");
            var hash = await ComputeHashAsync(path);
            await File.AppendAllTextAsync(Path.Combine(_backupFolder, "backups.txt"), $"{path}|{hash}\n");
            return path;
        }

        public Task RollbackAsync(string path)
        {
            // placeholder for secedit /configure
            return Task.CompletedTask;
        }

        private static async Task<string> ComputeHashAsync(string path)
        {
            await using var stream = File.OpenRead(path);
            using var sha = SHA256.Create();
            var hash = await sha.ComputeHashAsync(stream);
            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }
    }
}
