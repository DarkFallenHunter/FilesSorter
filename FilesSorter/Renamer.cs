using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace FilesSorter
{
    class Renamer
    {
        private DirectoryInfo _folder; // Выбранная для сортировки папка
        private List<FileInfo> _filesList; // Список сортируемых файлов

        public string FolderPath // Свойство для получения пути выбранной папки
        {
            get
            {
                if (_folder == null)
                    return "";
                else
                    return _folder.Name;
            }

            private set { }
        }

        // Метод для выбора папки
        public void SelectDirectory()
        {
            FolderBrowserDialog folderSelector = new FolderBrowserDialog()
            {
                Description = "Выберите папку, в которой хотите отсортировать фотографии."
            };
            DialogResult result = folderSelector.ShowDialog();
            if (result == DialogResult.OK)
                _folder = new DirectoryInfo(folderSelector.SelectedPath);
        }

        // Метод для выбора файлов из папки в зависимости от типа
        private void TakeFiles(string type)
        {
            _filesList = new List<FileInfo>();
            switch (type)
            {
                case "photoType":
                    foreach (string fileType in new List<string>() { "*.jpg", "*.png" })
                        _filesList.AddRange(_folder.GetFiles(fileType).ToList());
                    break;
                case "videoType":
                    foreach (string fileType in new List<string>() { "*.mov" })
                        _filesList.AddRange(_folder.GetFiles(fileType).ToList());
                    break;
            }
        }

        // Метод, выполняющий сортировку файлов при помощи сортировки кучей
        // и переименовывающий их по порядку
        public void SortFiles(string prefix, string type)
        {
            // Назначение новых имён файлам во избежании проблем с переименовыванием
            TakeFiles(type);
            for (int i = 0; i < _filesList.Count; i++)
                File.Move(_filesList[i].FullName, _folder.FullName + "\\" + (i + 1) + _filesList[i].Extension);

            // Непосредственная сортировка файлов
            TakeFiles(type);
            HeapSortFiles();
            for (int i = 0; i < _filesList.Count; i++)
            {
                string zeros = "";
                while (zeros.Length + i.ToString().Length < 4)
                    zeros += '0';
                File.Move(_filesList[i].FullName, _folder.FullName + "\\" + prefix + "_" + zeros + (i + 1) + _filesList[i].Extension);
            }
        }

        // Метод для постоения кучи по времени последнего изменения файлов
        private void ConstructHeap(int i, int length)
        {
            int largest = i, left = 2 * i + 1, right = 2 * i + 2 ;

            if (left < length && _filesList[left].LastWriteTime > _filesList[largest].LastWriteTime)
                largest = left;

            if (right < length && _filesList[right].LastWriteTime > _filesList[largest].LastWriteTime)
                largest = right;

            if (largest != i)
            {
                FileInfo tmp = _filesList[largest];
                _filesList[largest] = _filesList[i];
                _filesList[i] = tmp;
                ConstructHeap(largest, length);
            }
        }

        // Метод, реализующий саму сортировку кучей
        private void HeapSortFiles()
        {
            int length = _filesList.Count;

            for (int i = length / 2 - 1; i >= 0; i--)
                ConstructHeap(i, length);

            for (int i = length - 1; i >= 0; i--)
            {
                FileInfo tmp = _filesList[0];
                _filesList[0] = _filesList[i];
                _filesList[i] = tmp;
                ConstructHeap(0, i);
            }
        }
    }
}
