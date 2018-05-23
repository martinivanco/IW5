using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using IW5Gallery.App.Commands;
using IW5Gallery.App.Views;
using IW5Gallery.BL;
using IW5Gallery.BL.Messages;
using IW5Gallery.BL.Models;
using IW5Gallery.BL.Repositories;

namespace IW5Gallery.App.ViewModels
{
    public class AlbumListViewModel : ViewModelBase
    {
        private readonly AlbumRepository _albumRepository;
        private readonly IMessenger _messenger;

        public ObservableCollection<MiniatureModel> Albums { get; set; } = new ObservableCollection<MiniatureModel>();

        public ICommand SelectAlbumCommand { get; }
        public ICommand OnLoadCommand { get; }
        public ICommand OnUnloadCommand { get; }

        public AlbumListViewModel(AlbumRepository albumRepository, Messenger messenger)
        {
            _albumRepository = albumRepository;
            _messenger = messenger;

            SelectAlbumCommand = new RelayCommand(AlbumSelectionChanged);
            OnLoadCommand = new RelayCommand(OnLoad);
            OnUnloadCommand = new RelayCommand(OnUnload);

            _messenger.Register<DeleteAlbumMessage>(DeleteAlbumMessageReceived);
            _messenger.Register<NewAlbumMessage>(NewAlbumMessageReceived);
            _messenger.Register<UpdateAlbumMessage>(UpdateAlbumMessageReceived);
        }

        private void AlbumSelectionChanged(object parameter)
        {
            if (parameter is MiniatureModel album)
            {
                _messenger.Send(new SelectedAlbumMessage { Id = album.Id });
            }
        }

        private void OnLoad()
        {
            Albums.Clear();
            var albums = _albumRepository.GetAllAlbums();

            foreach (var album in albums)
            {
                Albums.Add(album);
            }
        }

        private void OnUnload()
        {
            _messenger.UnRegister<DeleteAlbumMessage>(DeleteAlbumMessageReceived);
            _messenger.UnRegister<NewAlbumMessage>(NewAlbumMessageReceived);
            _messenger.UnRegister<UpdateAlbumMessage>(UpdateAlbumMessageReceived);
        }

        private void DeleteAlbumMessageReceived(DeleteAlbumMessage message)
        {
            _albumRepository.RemoveAlbum(message.AlbumId);
            _messenger.Send(new UnloadAlbumMessage());
            OnLoad();
        }

        private void NewAlbumMessageReceived(NewAlbumMessage message)
        {
            if (_albumRepository.ContainsAlbumName(message.AlbumName))
            {
                MessageBox.Show("Album you are trying to add already exists!");
                return;
            }
            _albumRepository.InsertAlbum(new AlbumDetailModel { Name = message.AlbumName });
            OnLoad();
        }

        private void UpdateAlbumMessageReceived(UpdateAlbumMessage message)
        {
            _albumRepository.UpdateAlbumInfo(message.Album);
            OnLoad();
        }
    }
}
