using System;
using System.Collections.Generic;
using System.ComponentModel;
using SaveSystem.GameRepository;
using SaveSystem.SaveLoader.SaveLoaders;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Zenject;

public sealed class SaveSystemManager
{
    [ShowInInspector, Sirenix.OdinInspector.ReadOnly] 
    private Dictionary<string, string> _saves;

    private GameRepository _gameRepository;
    private ISaveLoaderManager _saveLoaderManager;
    
    private const uint RESENT_SAVE_SLOT = 0;
    private const int DEFAULT_SAVE_SLOT = -1;

    [Inject]
    public void Construct(GameRepository gameRepository, ISaveLoaderManager saveLoaderManager)
    {
        _gameRepository = gameRepository;
        _saveLoaderManager = saveLoaderManager;

        _saves = _gameRepository.GetAllSavesInfo();
    }

    internal void LoadSave(int saveSlot = 0)
    {
        _saveLoaderManager.LoadAllData(saveSlot);

        if (saveSlot == RESENT_SAVE_SLOT)
            ConsolePrinter.PrintSaveSystemMessage("Latest save was loaded");
        else if (saveSlot == DEFAULT_SAVE_SLOT)
            ConsolePrinter.PrintSaveSystemMessage("Default save was loaded");
        else
            ConsolePrinter.PrintSaveSystemMessage($"Save under #{saveSlot} was loaded");
    }
    
    internal void SaveGame(int saveSlot = 0)
    {
        if (saveSlot == -1)
            throw new Exception("You cannot save game under save slot #-1. This is a default save slot");
        
        _saveLoaderManager.SaveAllData(saveSlot);
        _saves = _gameRepository.GetAllSavesInfo();
        
        ConsolePrinter.PrintSaveSystemMessage(saveSlot == RESENT_SAVE_SLOT
            ? "Game saved to latest save"
            : $"Game saved under #{saveSlot} slot");
    }
}