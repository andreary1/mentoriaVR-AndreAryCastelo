using UnityEngine;

#if UNITY_META_QUEST
using Oculus.Interaction;
#endif

/// <summary>
/// Anexe no objeto do BOTÃO.
/// Arraste o objeto da TELA no campo "tvScreen" no Inspector.
/// A tela começa invisível e aparece/some ao pressionar o botão (poke).
/// </summary>
public class TVPowerButton : MonoBehaviour
{
    [Tooltip("Arraste aqui o GameObject da TELA da TV")]
    public GameObject tvScreen;

    private float lastPressTime = -99f;
    private const float Cooldown = 0.3f;

#if UNITY_META_QUEST
    private PokeInteractable pokeInteractable;
#endif

    void Start()
    {
        // Começa invisível
        if (tvScreen != null)
            tvScreen.SetActive(false);

#if UNITY_META_QUEST
        pokeInteractable = GetComponent<PokeInteractable>();
        if (pokeInteractable != null)
            pokeInteractable.WhenStateChanged += OnPokeStateChanged;
        else
            Debug.LogWarning("[TVPowerButton] PokeInteractable não encontrado neste GameObject.");
#endif
    }

#if UNITY_META_QUEST
    private void OnPokeStateChanged(InteractableStateChangeArgs args)
    {
        if (args.NewState == InteractableState.Select)
            OnPress();
    }
#endif

    // Pode também ser chamado via UnityEvent no Inspector
    public void OnPress()
    {
        if (Time.time - lastPressTime < Cooldown) return;
        lastPressTime = Time.time;

        if (tvScreen == null) return;

        tvScreen.SetActive(!tvScreen.activeSelf);
    }

    void OnDestroy()
    {
#if UNITY_META_QUEST
        if (pokeInteractable != null)
            pokeInteractable.WhenStateChanged -= OnPokeStateChanged;
#endif
    }
}
