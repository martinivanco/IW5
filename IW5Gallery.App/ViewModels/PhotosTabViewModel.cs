using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using IW5Gallery.App.Commands;
using IW5Gallery.App.Views;
using IW5Gallery.BL;
using IW5Gallery.BL.Messages;
using IW5Gallery.BL.Repositories;

namespace IW5Gallery.App.ViewModels
{
    public class PhotosTabViewModel : TabViewModelBase
    {
        private readonly IMessenger _messenger;

        private FrameworkElement _currentView;

        public ICommand OnUnloadCommand { get; }

        public FrameworkElement CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public PhotosTabViewModel(IMessenger messenger)
        {
            _messenger = messenger;

            OnUnloadCommand = new RelayCommand(OnUnload);

            _messenger.Register<ImageDoubleClickMessage>(OpenImage);
            _messenger.Register<ReturnToImageListMessage>(CloseImage);

            TabName = "Photos";
            CurrentView = new ImageListView();
        }

        private void OnUnload()
        {
            _messenger.UnRegister<ImageDoubleClickMessage>(OpenImage);
            _messenger.UnRegister<ReturnToImageListMessage>(CloseImage);
        }

        private void OpenImage(ImageDoubleClickMessage message)
        {
            CurrentView = new ImageDetailView();
            _messenger.Send(new OpenedImageMessage() { ImageId = message.Id });
        }

        private void CloseImage(ReturnToImageListMessage obj)
        {
            CurrentView = new ImageListView();
        }
    }
}
