using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

namespace BattleUI 
{
    public abstract class CharacterUI : MonoBehaviour
    {
        protected Character character;

        public virtual void Initialize(Character character) 
        { 
            this.character = character;
        }
    }

    [RequireComponent(typeof(Slider))]
    public abstract class CharacterUI_slider : CharacterUI 
    {
        protected Slider slider;

        public void SetMaxValue(int value) 
        { 
            slider.maxValue = value;
        }

        public virtual void SetValue(int value) 
        { 
            slider.value = value; 
        }

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
        }
    }
} 
