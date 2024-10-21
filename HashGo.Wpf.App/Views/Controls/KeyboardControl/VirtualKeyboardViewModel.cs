using HashGo.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HashGo.Wpf.App.Views.Controls.KeyboardControl
{
    public class VirtualKeyboardViewModel : INotifyPropertyChanged
    {
        private KeyboardType _keyboardType;
        public KeyboardType KeyboardType
        {
            get => _keyboardType;
            private set
            {
                _keyboardType = value;
                NotifyPropertyChanged(nameof(KeyboardType));
            }
        }
        private bool _uppercase;
        public bool Uppercase
        {
            get => _uppercase;
            private set
            {
                _uppercase = value;
                NotifyPropertyChanged(nameof(Uppercase));
            }
        }
        public Command AddCharacter { get; }
        public Command ChangeCasing { get;}
        public Command RemoveCharacter { get;}
        public Command ChangeKeyboardType { get; }
        public Command EnterCharacter { get;}

        private TextBox TextBox;
        public VirtualKeyboardViewModel(TextBox tBoxName)
        {
            //_keyboardText = initialValue;
            _keyboardType = KeyboardType.Alphabet;
            _uppercase = false;
            TextBox = tBoxName;


            AddCharacter = new Command(a =>
            {
                if (a is string character)
                {
                    if (character.Length == 1)
                    {
                        if (Uppercase)
                            character = character.ToUpper();

                        var focusedElement = FocusManager.GetFocusedElement(Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)) as FrameworkElement;

                        if (TextBox is TextBox textBox)
                        {
                            // Insert the character into the TextBox at the current caret position
                            int caretIndex = textBox.CaretIndex;
                            textBox.Text = textBox.Text.Insert(caretIndex, character);
                            textBox.CaretIndex = caretIndex + 1;
                        }
                    }
                }
            });

            ChangeCasing = new Command(a => Uppercase = !Uppercase);

            RemoveCharacter = new Command(a =>
            {
                // Get the currently focused control
                var focusedElement = FocusManager.GetFocusedElement(Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)) as FrameworkElement;

                if (TextBox is TextBox textBox)
                {
                    if (!string.IsNullOrEmpty(textBox.Text))
                    {
                        int caretIndex = textBox.CaretIndex;
                        if (textBox.SelectedText.Length > 0)
                        {
                            textBox.Text = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);
                            textBox.CaretIndex = caretIndex - textBox.SelectionLength;
                        }
                        else
                        {
                            // Remove the character at the caret position (like Backspace)
                            if (caretIndex > 0)
                            {
                                textBox.Text = textBox.Text.Remove(caretIndex - 1, 1);
                                textBox.CaretIndex = caretIndex - 1;
                            }
                        }
                    }
                }
            });


            EnterCharacter = new Command(a =>
            {
                var focusedElement = Keyboard.FocusedElement as FrameworkElement;

                if (TextBox is TextBox textBox)
                {
                    // Get the current caret position
                    int caretIndex = textBox.CaretIndex;

                    // Insert a new line at the caret position
                    textBox.Text = textBox.Text.Insert(caretIndex, Environment.NewLine);

                    // Move the caret to the new position after the new line
                    textBox.CaretIndex = caretIndex + Environment.NewLine.Length;
                }
            });


            ChangeKeyboardType = new Command(a =>
            {
                if (KeyboardType == KeyboardType.Alphabet) KeyboardType = KeyboardType.Special;
                else KeyboardType = KeyboardType.Alphabet;
            });

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

