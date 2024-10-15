using GameEngine;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

//TODO: Удалить этот класс!
//Развернуть архитектуру на Zenject/VContainer/Custom

// Архитектуру развернул на Zenject, а этот класс оставлю, что бы можно было методами UnitManager и ResourseService пользоваться через инвпектор
public sealed class EntryPoint : MonoBehaviour
{
    [ShowInInspector]
    private UnitManager _unitManager;
    
    [ShowInInspector, ReadOnly]
    private ResourceService _resourceService;
    
    [ShowInInspector, ReadOnly]
    private SaveSystemManager _saveSystemManager;

    [Inject]
    public void Construct(UnitManager unitManager, 
        ResourceService resourceService, 
        SaveSystemManager saveSystemManager,
        Resource[] resources,
        Unit[] units
        )
    {
        _unitManager = unitManager;
        _resourceService = resourceService;
        _saveSystemManager = saveSystemManager;

        SetupSceneObjects(resources, units);
        // LoadDefaultSave();
    }

    private void SetupSceneObjects(Resource[] resources, Unit[] units)
    {
        _unitManager.SetupUnits(units);
        _resourceService.SetResources(resources);
    }
    
    // Тут дублирование кода из SaveSystemManager просто для удобства
    [Title("Управление загрузкой и сохранением")]
    [Button]
    public void LoadSave()
    {
        _saveSystemManager.LoadSave();
    }

    [Button]
    public void SaveGame()
    {
        _saveSystemManager.SaveGame();
    }
    
    [Button]
    public void LoadDefaultSave()
    {
        _saveSystemManager.LoadSave(-1);
    }
    
    [Button]
    public void LoadSelectedSave(int saveNumber)
    {
        _saveSystemManager.LoadSave(saveNumber);
    }
    
    [Button]
    public void SaveGameToSelectedSave(int saveNumber)
    {
        _saveSystemManager.SaveGame(saveNumber);
    }
}