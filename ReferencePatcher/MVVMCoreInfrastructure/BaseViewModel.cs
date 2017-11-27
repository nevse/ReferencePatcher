using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace ReferencePatcher {
    public abstract class ViewModelBase : INotifyPropertyChanged {

        event PropertyChangedEventHandler PropertyChanged;
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged {
            add {
                PropertyChanged += value;
            }
            remove {
                PropertyChanged -= value;
            }
        }
        protected void SetProperty<T>(ref T storage, T value, Expression<Func<T>> expression) {
            SetPropertyCore<T>(ref storage, value, GetPropertyNameFast(expression), null);
        }

        protected void RaisePropertyChanged(string propertyName) {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        string GetPropertyNameFast(LambdaExpression expression) {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException("Not a property", "expression");
            var member = memberExpression.Member;
            return member.Name;
        }

        void SetPropertyCore<T>(ref T storage, T value, string propertyName, Action callback) {
            if (Object.Equals(storage, value))
                return;
            storage = value;
            if (callback != null)
                callback();
            RaisePropertyChanged(propertyName);
        }
    }
}
