using UnityEngine;
using UnityEngine.InputSystem;

public class RollKeyRebindManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    public void OnRebindRollKey()
    {
        var action = inputActions.FindAction("Roll");

        if (action is null)
            return;

        Debug.Log("Нажмите клавишу, на которую нужно переназначить бросок кубиков");

        action.Disable();

        action.PerformInteractiveRebinding(0)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(callback =>
            {
                callback.Dispose();
                action.Enable();
                Debug.Log($"Переназначение выполнено на {action.GetBindingDisplayString()}");
            })
            .OnCancel(callback =>
            {
                callback.Dispose();
                action.Enable();
                Debug.Log("Переназначение отменено");
            })
            .Start();
    }
}
