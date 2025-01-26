using CommunityToolkit.Mvvm.Input;
using HashGo.Core.Enum;
using HashGo.Wpf.App.Behavior;
using HashGo.Wpf.App.Common;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public RelayCommand<string> AddCharacter { get; set; }
        public RelayCommand ChangeCasing { get; set; }
        public RelayCommand<string> RemoveCharacter { get; set; }
        public RelayCommand ChangeKeyboardType { get; set; }
        public RelayCommand EnterCharacter { get; set; }

        private TextBox textBox;

        public TextBox TextBox
        {
            get { return textBox; }
            set { textBox = value; }
        }

        public VirtualKeyboardViewModel()
        {
            _keyboardType = KeyboardType.Alphabet;
            _uppercase = false;
            AddCharacter = new RelayCommand<string>(OnAddCharacter);
            ChangeCasing = new RelayCommand(OnChangeCasing);
            RemoveCharacter = new RelayCommand<string>(OnRemoveCharacter);
            EnterCharacter = new RelayCommand(OnEnterCharacter);
            ChangeKeyboardType = new RelayCommand(OnChangeKeyboardType);
        }

        private void OnChangeKeyboardType()
        {
            if (KeyboardType == KeyboardType.Alphabet) KeyboardType = KeyboardType.Special;
            else KeyboardType = KeyboardType.Alphabet;
        }

        private void OnEnterCharacter()
        {
            if (TextBox is TextBox textBox)
            {
                // Get the current caret position
                int caretIndex = textBox.CaretIndex;

                // Insert a new line at the caret position
                textBox.Text = textBox.Text.Insert(caretIndex, Environment.NewLine);

                // Move the caret to the new position after the new line
                textBox.CaretIndex = caretIndex + Environment.NewLine.Length;
                textBox.Focus();
                Keyboard.Focus(textBox);
            }
        }

        private void OnRemoveCharacter(string obj)
        {
            if (TextBox is TextBox textBox)
            {
                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    int caretIndex = textBox.CaretIndex;
                    if (textBox.SelectedText.Length > 0)
                    {
                        textBox.Text = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);
                        textBox.CaretIndex = caretIndex - textBox.SelectionLength;
                        textBox.Focus();
                        Keyboard.Focus(textBox);
                    }
                    else
                    {
                        // Remove the character at the caret position (like Backspace)
                        if (caretIndex > 0)
                        {
                            textBox.Text = textBox.Text.Remove(caretIndex - 1, 1);
                            textBox.CaretIndex = caretIndex - 1;
                            textBox.Focus();
                            Keyboard.Focus(textBox);
                        }
                    }
                }
            }

        }

        private void OnChangeCasing()
        {
            Uppercase = !Uppercase;
        }

        private void OnAddCharacter(string obj)
        {
            if (obj is string character)
            {
                if (character.Length == 1)
                {
                    if (Uppercase)
                        character = character.ToUpper();

                    var focusedElement = FocusManager.GetFocusedElement(Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive)) as FrameworkElement;

                    if (TextBox is TextBox textBox)
                    {
                        var behaviors = Interaction.GetBehaviors(textBox);
                        bool hasIntegerInputBehaviour = behaviors.OfType<IntegerInputBehaviour>().Any();
                        if (hasIntegerInputBehaviour)
                        {
                            if (textBox.MaxLength > textBox.Text.Length)
                            {
                                if (textBox.Name.Contains("ContactNumberTextBox"))
                                {
                                    string newText = textBox.Text.Insert(textBox.SelectionStart, character);

                                    if (IsTextValid(newText))
                                    {
                                        updateText(character);
                                    }

                                }
                                else if (IsTextAllowed(character))
                                {
                                    updateText(character);
                                }
                            }
                        }
                        else
                        {
                            updateText(character);
                        }
                        textBox.Focus();
                        Keyboard.Focus(textBox);

                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+"); // Matches text that is not a number
            return !regex.IsMatch(text);
        }

        private static readonly Regex _regex = new Regex("^[896][0-9]*$");

        private bool IsTextValid(string text)
        {
            var result = _regex.IsMatch(text);
            return result;
        }

        private void updateText(string character)
        {
            int caretIndex = textBox.CaretIndex;
            textBox.Text = textBox.Text.Insert(caretIndex, character);
            textBox.CaretIndex = caretIndex + 1;
        }

    }
}

