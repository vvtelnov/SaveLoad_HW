using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem.GameRepository
{
    public interface IGameRepository
    {
        public bool TryGetData<T>(out T data);
        public void ReadSaveData(int saveSlot);
        
        public void SetData<T>(T data);
        public void WriteSavedData(int saveSlot);
    }
}