using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


// ƒ≥∏Ø≈Õ√¢ø°º≠ µ¶ πŸ≤‹ ∂ß πŸ≤‹ ¿Œµ¶Ω∫∏¶ º≥¡§«ÿ¡‹.
public class deckcell_character_deck_module : MonoBehaviour, IPointerEnterHandler
{
    private Deck_changer deck_changer;
    [SerializeField] private int index;

    public void Setup(Deck_changer deck_changer, int index) 
    {
        this.deck_changer = deck_changer;
        this.index = index;
    }

    public void OnPointerEnter(PointerEventData pointerEventData) 
    { 
        deck_changer.set_index(index);
    }
}
