using System;
using Game.Sim;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    // TODO: implement with actual buffer, allow customization
    public class InputBuffer
    {
        private InputFlags[] buffer = new InputFlags[16];
        private int frontIdx = 0;
        private int size = 0;

        public void Saturate()
        {
            InputFlags input = InputFlags.None;

            // Movement
            if (Keyboard.current.aKey.isPressed)
                input |= InputFlags.Left;
            if (Keyboard.current.dKey.isPressed)
                input |= InputFlags.Right;
            if (Keyboard.current.wKey.isPressed)
                input |= InputFlags.Up;
            if (Keyboard.current.sKey.isPressed)
                input |= InputFlags.Down;

            // Attacks
            if (Keyboard.current.jKey.isPressed)
                input |= InputFlags.LightAttack;
            if (Keyboard.current.kKey.isPressed)
                input |= InputFlags.MediumAttack;
            if (Keyboard.current.lKey.isPressed)
                input |= InputFlags.SuperAttack;

            // Mania Keys
            if (Keyboard.current.aKey.isPressed)
                input |= InputFlags.Mania5;
            if (Keyboard.current.sKey.isPressed)
                input |= InputFlags.Mania3;
            if (Keyboard.current.dKey.isPressed)
                input |= InputFlags.Mania1;
            if (Keyboard.current.jKey.isPressed)
                input |= InputFlags.Mania2;
            if (Keyboard.current.kKey.isPressed)
                input |= InputFlags.Mania4;
            if (Keyboard.current.lKey.isPressed)
                input |= InputFlags.Mania6;


            buffer[(frontIdx + size) % buffer.Length] = input;
            if (size < buffer.Length)
            {
                size++;
            } else
            {
                frontIdx = (frontIdx + 1) % buffer.Length;
            }
        }

        public GameInput Consume()
        {
            GameInput res;
            if (size == 0)
            {
                res = new GameInput(InputFlags.None);
            }
            else
            {
                res = new GameInput(buffer[frontIdx]);
                frontIdx = (frontIdx + 1) % buffer.Length;
                size--;
            }

            return res;
        }
    }
}
