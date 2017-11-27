using System;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace ReferencePatcher {
    public interface IDialogService {
        string ShowOpenFileDialog(string extension, string filter);
        void Close();
    }

    public class DialogService : IDialogService {
        Window owner;
        public DialogService(Window owner) {
            this.owner = owner;
        }
        public string ShowOpenFileDialog(string extension, string filter) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = extension;
            openFileDialog.Filter = filter;
            if (openFileDialog.ShowDialog(this.owner).GetValueOrDefault(false)) {
                return openFileDialog.FileName;
            }
            return String.Empty;
        }

        public void Close() {
            this.owner.Close();
        }
    }
}
