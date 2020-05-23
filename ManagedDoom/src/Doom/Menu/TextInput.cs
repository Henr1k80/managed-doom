﻿using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedDoom
{
    public sealed class TextInput
    {
        private List<char> text;
        private Action<IReadOnlyList<char>> typed;
        private Action<IReadOnlyList<char>> finished;
        private Action canceled;

        private TextInputState state;

        public TextInput(
            IReadOnlyList<char> initialText,
            Action<IReadOnlyList<char>> typed,
            Action<IReadOnlyList<char>> finished,
            Action canceled)
        {
            this.text = initialText.ToList();
            this.typed = typed;
            this.finished = finished;
            this.canceled = canceled;

            state = TextInputState.Typing;
        }

        public bool DoEvent(DoomEvent e)
        {
            if (SFML.Window.Keyboard.Key.A <= e.Key &&
                e.Key <= SFML.Window.Keyboard.Key.Z &&
                e.Type == EventType.KeyDown)
            {
                text.Add((char)(e.Key - SFML.Window.Keyboard.Key.A + 'A'));
                typed(text);
                return true;
            }

            if (e.Key == SFML.Window.Keyboard.Key.Backspace &&
                e.Type == EventType.KeyDown)
            {
                if (text.Count > 0)
                {
                    text.RemoveAt(text.Count - 1);
                }
                typed(text);
                return true;
            }

            if (e.Key == SFML.Window.Keyboard.Key.Enter &&
                e.Type == EventType.KeyDown)
            {
                finished(text);
                state = TextInputState.Finished;
                return true;
            }

            if (e.Key == SFML.Window.Keyboard.Key.Escape &&
                e.Type == EventType.KeyDown)
            {
                canceled();
                state = TextInputState.Canceled;
                return true;
            }

            return true;
        }

        public IReadOnlyList<char> Text => text;
        public TextInputState State => state;
    }
}
