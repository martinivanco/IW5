using System.Windows;
using System.Windows.Input;
using IW5Gallery.App.Commands;
using IW5Gallery.App.Views;
using IW5Gallery.BL;
using IW5Gallery.BL.Messages;

namespace IW5Gallery.App.ViewModels
{
    public class AlbumsTabViewModel : TabViewModelBase
    {
        private readonly IMessenger _messenger;
        private FrameworkElement _currentView;
        private string _albumName = "Enter album name";

        public ICommand CreateAlbumCommand { get; }
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
        public string AlbumName
        {
            get => _albumName;
            set
            {
                _albumName = value;
                OnPropertyChanged();
            }
        }

        public AlbumsTabViewModel(IMessenger messenger)
        {
            _messenger = messenger;

            CreateAlbumCommand = new RelayCommand(AddNewAlbum);
            OnUnloadCommand = new RelayCommand(OnUnload);

            _messenger.Register<AlbumImageDoubleClickMessage>(OpenImage);
            _messenger.Register<ReturnToAlbumMessage>(CloseImage);
            _messenger.Register<UnloadAlbumMessage>(UnloadAlbum);

            TabName = "Albums";
            CurrentView = new AlbumDetailView();
        }

        private void AddNewAlbum()
        {
            if (AlbumName == string.Empty)
            {
                MessageBox.Show("Please specify a name for the album.");
                return;
            }
            _messenger.Send(new NewAlbumMessage(AlbumName));
        }

        private void OnUnload()
        {
            _messenger.UnRegister<AlbumImageDoubleClickMessage>(OpenImage);
            _messenger.UnRegister<ReturnToAlbumMessage>(CloseImage);
            _messenger.UnRegister<UnloadAlbumMessage>(UnloadAlbum);
        }

        private void OpenImage(AlbumImageDoubleClickMessage message)
        {
            CurrentView = new ImageDetailView();
            _messenger.Send(new OpenedAlbumImageMessage { ImageId = message.ImageId, AlbumId = message.AlbumId });
        }

        private void CloseImage(ReturnToAlbumMessage message)
        {
            CurrentView = new AlbumDetailView();
            _messenger.Send(new SelectedAlbumMessage {Id = message.Id});
        }

        private void UnloadAlbum(UnloadAlbumMessage message)
        {
            CurrentView = new AlbumDetailView();
        }
    }
}