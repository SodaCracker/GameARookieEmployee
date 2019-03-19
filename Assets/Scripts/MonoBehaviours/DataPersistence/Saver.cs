using UnityEngine;

public abstract class Saver : MonoBehaviour
{
    public string uniqueIdentifier;       
    public SaveData saveData;          
    // 其中会包含 uniqueIdentifier 及其他信息
    protected string key;              
    // 需要用到 SceneController.cs 中的两个 Action
    private SceneController sceneController;   

    private void Awake()
    {
        sceneController = FindObjectOfType<SceneController>();
        if (!sceneController)
            throw new UnityException("Scene Controller lost");
        key = SetKey();
    }

    private void OnEnable()
    {
        sceneController.BeforeSceneUnload += Save;
        sceneController.AfterSceneLoad += Load;
    }


    private void OnDisable()
    {
        sceneController.BeforeSceneUnload -= Save;
        sceneController.AfterSceneLoad -= Load;
    }

    protected abstract string SetKey();
    protected abstract void Save();
    protected abstract void Load();
}
