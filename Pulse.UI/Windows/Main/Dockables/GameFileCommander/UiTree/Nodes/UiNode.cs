﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media;
using Pulse.UI.Annotations;

namespace Pulse.UI
{
    public abstract class UiNode : INotifyPropertyChanged
    {
        protected static readonly UiNode[] EmptyChilds = new UiNode[0];

        public UiNodeType Type  { get; private set; }
        public string Name { get; protected internal set; }
        public UiNode Parent { get; internal set; }

        protected UiNode(string name, UiNodeType type)
        {
            Name = name;
            Type = type;
        }

        // Binding
        public virtual ImageSource Icon
        {
            get { return null; }
        }

        #region Childs

        public virtual UiNode[] GetChilds()
        {
            return EmptyChilds;
        }

        #endregion

        private bool _isSelected;
        private bool _isExpanded;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                if (_isSelected && _isChecked != null)
                    SetIsChecked(_isChecked);
                OnPropertyChanged();
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                if (_isExpanded && _isChecked != null)
                    SetIsChecked(_isChecked);
                OnPropertyChanged();
            }
        }

        #region IsChecked

        private enum CheckedChanger
        {
            Manual,
            Parent,
            Child
        }

        private bool? _isChecked = false;
        private CheckedChanger _checkedChanger = CheckedChanger.Manual;
        private int _checkedCount;
        private int _unknownCount;

        public bool? IsChecked
        {
            get { return _isChecked; }
            set
            {
                try
                {
                    if (_isChecked == value)
                        return;

                    SetIsChecked(value);
                    OnPropertyChanged();
                }
                finally
                {
                    _checkedChanger = CheckedChanger.Manual;
                }
            }
        }

        private void SetIsChecked(bool? value)
        {
            if (value == null && _checkedChanger == CheckedChanger.Manual)
                value = !_isChecked;

            if (_checkedChanger != CheckedChanger.Child && (_isExpanded || _isSelected))
            {
                UiNode[] childs = GetChilds();
                for (int i = 0; i < childs.Length; i++)
                {
                    UiNode entry = childs[i];
                    entry._checkedChanger = CheckedChanger.Parent;
                    entry.IsChecked = value;
                }
            }

            if (Parent != null)
                Parent.OnChildCheckedChanged(_isChecked, value);

            _isChecked = value;
        }

        private void OnChildCheckedChanged(bool? oldValue, bool? newValue)
        {
            switch (oldValue)
            {
                case null:
                    Interlocked.Decrement(ref _unknownCount);
                    break;
                case true:
                    Interlocked.Decrement(ref _checkedCount);
                    break;
            }

            switch (newValue)
            {
                case null:
                    Interlocked.Increment(ref _unknownCount);
                    break;
                case true:
                    Interlocked.Increment(ref _checkedCount);
                    break;
            }

            _checkedChanger = CheckedChanger.Child;

            try
            {
                if (_unknownCount > 0)
                    IsChecked = null;
                else if (_checkedCount > 0)
                    IsChecked = _checkedCount == GetChilds().Length ? (bool?)true : null;
                else
                    IsChecked = false;
            }
            finally
            {
                _checkedChanger = CheckedChanger.Manual;
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}