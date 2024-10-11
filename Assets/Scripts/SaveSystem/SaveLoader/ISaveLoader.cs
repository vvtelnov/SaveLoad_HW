using SaveSystem.GameData;
using SaveSystem.GameRepository;

namespace SaveSystem.SaveLoader.SaveLoaders
{
    public interface ISaveLoaderManager
    {
        public void LoadAllData(int saveSlot);
        public void SaveAllData(int saveSlot);
    }
    
    public interface ISaveLoader
    {
        public void LoadData(IGameRepository gameRepository);
        public void SaveData(IGameRepository gameRepository);
    }
}