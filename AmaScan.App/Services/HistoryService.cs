using AmaScan.App.Models;
using Ninject;
using System;
using System.Collections.Generic;
using UWPCore.Framework.Data;
using UWPCore.Framework.Storage;
using System.Threading.Tasks;

namespace AmaScan.App.Services
{
    public class HistoryService : IHistoryService
    {
        private const string DATA_FILE = "history.data";

        public ISerializationService SerializationService { get; private set; }
        public IStorageService StorageService { get; private set; }

        private bool _hasLoaded = false;

        public IList<HistoryItem> Items { get; private set; } = new List<HistoryItem>();


        [Inject]
        public HistoryService(ISerializationService serializationService, ILocalStorageService storageService)
        {
            SerializationService = serializationService;
            StorageService = storageService;
        }

        public async Task Load()
        {
            if (!_hasLoaded)
            {
                _hasLoaded = true;
                var content = await StorageService.ReadFile(DATA_FILE);
                if (content != null)
                {
                    Items = SerializationService.DeserializeJson<List<HistoryItem>>(content);
                }
            }
        }

        public async Task Save()
        {
            var serializedContent = SerializationService.SerializeJson(Items);
            if (!await StorageService.WriteFile(DATA_FILE, serializedContent))
            {
                // data could not be saved
            }
        }
    }
}
