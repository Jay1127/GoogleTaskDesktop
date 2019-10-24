using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Threading.Tasks;

namespace GoogleTaskDesktop.ViewModel
{
    public class EditorDialogViewModel : ViewModelBase
    {
        private string _title;
        private string _name;
        private string _description;
        private bool _isShown;

        /// <summary>
        /// 팝업 타이틀
        /// </summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }            

        /// <summary>
        /// 생성 또는 업데이트할 데이터
        /// </summary>
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        /// <summary>
        /// 상세 설명(Mask Textboxk)
        /// </summary>
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        /// <summary>
        /// 현재 팝업이 호출된지 판단
        /// </summary>
        public bool IsShown
        {
            get => _isShown;
            set => Set(ref _isShown, value);
        }

        public RelayCommand UpdateCommand { get; }
        public RelayCommand CancelCommand { get; }

        public delegate Task UpdatedHandler(string updatedName);
        public event UpdatedHandler Updated;

        public EditorDialogViewModel()
        {
            Title = string.Empty;
            Description = string.Empty;
            Name = string.Empty;

            UpdateCommand = new RelayCommand(Update);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Cancel()
        {
            Close();
        }

        private void Update()
        {
            Updated?.Invoke(Name);
            Close();
        }

        public void Show(string title, string description)
        {
            Name = string.Empty;
            Title = title;
            Description = description;
            IsShown = true;
        }

        public void Close()
        {
            IsShown = false;
        }
    }
}