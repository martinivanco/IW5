using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using IW5Gallery.App.Commands;
using IW5Gallery.BL;
using IW5Gallery.BL.Messages;
using IW5Gallery.BL.Models;
using IW5Gallery.BL.Repositories;
using IW5Gallery.DAL.Entities;

namespace IW5Gallery.App.ViewModels
{
    public class AlbumDetailViewModel : ViewModelBase
    {
        private readonly AlbumRepository _albumRepository;
        private readonly Messenger _messenger;
        private readonly FileManager _fileManager;
        private AlbumDetailModel _detail;

        public ICommand UpdateAlbumNameCommand { get; }
        public ICommand ExportAlbumCommand { get; }
        public ICommand DeleteAlbumCommand { get; }
        public ICommand OpenImageCommand { get; }
        public ICommand OnUnloadCommand { get; }

        public AlbumDetailModel Detail
        {
            get => _detail;
            set
            {
                if (Equals(value, _detail)) return;
                _detail = value;
                OnPropertyChanged();
            }
        }

        public AlbumDetailViewModel(AlbumRepository albumRepository, Messenger messenger, FileManager fileManager)
        {
            _albumRepository = albumRepository;
            _messenger = messenger;
            _fileManager = fileManager;

            UpdateAlbumNameCommand = new RelayCommand(UpdateAlbumName);
            ExportAlbumCommand = new RelayCommand(ExportAlbum);
            DeleteAlbumCommand = new RelayCommand(DeleteAlbum);  
            OpenImageCommand = new RelayCommand(OpenImage);
            OnUnloadCommand = new RelayCommand(OnUnload);

            _messenger.Register<SelectedAlbumMessage>(SelectedAlbum);
        }

        private void UpdateAlbumName(object parameter)
        {
            if (Detail.Name == string.Empty)
            {
                MessageBox.Show("The album name can not be empty!");
                return;
            }
            _messenger.Send(new UpdateAlbumMessage { Album = Detail });
        }

        private void ExportAlbum()
        {
            if (Detail.Images.Any())
                _fileManager.ExportImagesFromAlbum(Detail);
            else
                MessageBox.Show("The album is empty!");
        }

        private void DeleteAlbum()
        {
            _messenger.Send(new DeleteAlbumMessage(Detail.Id));
        }

        private void OnUnload()
        {
            _messenger.UnRegister<SelectedAlbumMessage>(SelectedAlbum);
        }

        private void OpenImage(object parameter)
        {
            if (parameter is MiniatureModel miniature)
            {
                _messenger.Send(new AlbumImageDoubleClickMessage() { ImageId = miniature.Id, AlbumId = Detail.Id });
            }
        }

        private void SelectedAlbum(SelectedAlbumMessage message)
        {
            Detail = _albumRepository.GetAlbumById(message.Id);
        }
    }
}
