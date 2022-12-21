using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleSFTPDragAndDrop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VM myVm;
        public MainWindow()
        {
            myVm = new VM();
            DataContext = myVm;
            InitializeComponent();
        }

        /// Please do not implement anything else in the MainWindow. All should be bound to the VM
        private void ImagePanel_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                myVm.Files.Clear();
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var file in files)
                    myVm.Files.Add(file);
                myVm.OnPropertyChanged("Files");
            }
        }
    }
}