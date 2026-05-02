using UnityEngine;

#if UNITY_META_QUEST
using Oculus.Interaction;
#endif

/// <summary>
/// Anexe no objeto do BOTÃO.
/// Arraste o componente Light do abajur no campo "lampLight" no Inspector.
/// A luz começa apagada e acende/apaga ao pressionar o botão (poke).
/// </summary>
public class LampButton : MonoBehaviour
{
    [Tooltip("Arraste aqui o componente Light do abajur")]
    public Light lampLight;

    private float lastPressTime = -99f;
    private const float Cooldown = 0.3f;

#if UNITY_META_QUEST
    private PokeInteractable pokeInteractable;
#endif

    void Start()
    {
        if (lampLight != null)
            lampLight.enabled = false;

#if UNITY_META_QUEST
        pokeInteractable = GetComponent<PokeInteractable>();
        if (pokeInteractable != null)
            pokeInteractable.WhenStateChanged += OnPokeStateChanged;
        else
            Debug.LogWarning("[LampButton] PokeInteractable não encontrado neste GameObject.");
#endif
    }

#if UNITY_META_QUEST
    private void OnPokeStateChanged(InteractableStateChangeArgs args)
    {
        if (args.NewState == InteractableState.Select)
            OnPress();
    }
#endif

    public void OnPress()
    {
        if (Time.time - lastPressTime < Cooldown) return;
        lastPressTime = Time.time;

        if (lampLight == null) return;

        lampLight.enabled = !lampLight.enabled;
    }

    void OnDestroy()
    {
#if UNITY_META_QUEST
        if (pokeInteractable != null)
            pokeInteractable.WhenStateChanged -= OnPokeStateChanged;
#endif
    }
}
