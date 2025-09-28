using System;
using System.Collections.Generic;
using System.IO;
using MiAppWinFormsSimple;
using Xunit;

namespace MiApp.Tests
{
    public class UserStorageTests : IDisposable
    {
        private readonly string _tempDir;
        private readonly string _tempFile;

        public UserStorageTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "MiAppTests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDir);
            _tempFile = Path.Combine(_tempDir, "users.json");
        }

        [Fact]
        public void SerializeDeserialize_PreservesData()
        {
            var users = new List<User> { new User { Name = "Ana", Email = "ana@example.com" } };
            var json = UserStorage.Serialize(users);
            var result = UserStorage.Deserialize(json);
            Assert.Single(result);
            Assert.Equal("Ana", result[0].Name);
            Assert.Equal("ana@example.com", result[0].Email);
        }

        [Fact]
        public void SaveAndLoadFile_WorksCorrectly()
        {
            var users = new List<User>
            {
                new User { Name = "Beto", Email = "beto@example.com" },
                new User { Name = "Carla", Email = "carla@example.com" }
            };

            UserStorage.SaveToFile(users, _tempFile);
            Assert.True(File.Exists(_tempFile));

            var loaded = UserStorage.LoadFromFile(_tempFile);
            Assert.Equal(2, loaded.Count);
            Assert.Contains(loaded, u => u.Name == "Beto" && u.Email == "beto@example.com");
            Assert.Contains(loaded, u => u.Name == "Carla" && u.Email == "carla@example.com");
        }

        public void Dispose()
        {
            try { if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, true); } catch { }
        }
    }
}
