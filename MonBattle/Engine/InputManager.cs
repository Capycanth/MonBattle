using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Capybara_1.Engine
{
    [Flags]
    public enum InputEnum
    {
        ZERO = 0,
        ONE = 1 << 0,
        TWO = 1 << 1,
        THREE = 1 << 2,
        FOUR = 1 << 3,
        FIVE = 1 << 4,
        SIX = 1 << 5,
        SEVEN = 1 << 6,
        EIGHT = 1 << 7,
        NINE = 1 << 8,
        UP = 1 << 9,
        DOWN = 1 << 10,
        LEFT = 1 << 11,
        RIGHT = 1 << 12,
        ENTER = 1 << 13,
        ESC = 1 << 14,
        SPACE = 1 << 15,
    }

    public class InputManager
    {
        private Dictionary<Keys, InputEnum> keyBindings;
        private int playerInput;
        private InputContainer inputContainer;

        public InputManager() 
        {
            this.inputContainer = new InputContainer();
        }

        public int Update()
        {
            playerInput = 0;
            foreach (Keys key in inputContainer.c_Keyboard.GetPressedKeys())
            {
                if (keyBindings.ContainsKey(key))
                {
                    playerInput |= (int)keyBindings[key];
                }
            }
            return playerInput;
        }

        public void HandleInput(KeyboardState keyboardState, MouseState mouseState)
        {
            inputContainer.Update(keyboardState, mouseState);
        }

        public bool Pressed(params InputEnum[] input)
        {
            int n = 0;
            foreach (int key in input)
            {
                n |= key;
            }
            return (playerInput & n) > 0;
        }

        public Dictionary<Keys, InputEnum> KeyBindings { get { return keyBindings; } set { this.keyBindings = value; } }

        private class InputContainer
        {
            public KeyboardState p_Keyboard;
            public KeyboardState c_Keyboard;
            public MouseState p_Mouse;
            public MouseState c_Mouse;

            public InputContainer() { }

            public void Update(KeyboardState keyboardState, MouseState mouseState)
            {
                this.p_Keyboard = this.c_Keyboard;
                this.c_Keyboard = keyboardState;
                this.p_Mouse = this.c_Mouse;
                this.c_Mouse = mouseState;
            }
        }
    }
}
