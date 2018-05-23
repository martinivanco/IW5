using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using IW5Gallery.App.Commands;
using IW5Gallery.BL;
using IW5Gallery.BL.Messages;
using IW5Gallery.BL.Models;
using IW5Gallery.BL.Repositories;
using IW5Gallery.DAL.Entities;

namespace IW5Gallery.App.ViewModels
{
    public class ImageListViewModel : ViewModelBase
    {
        private readonly ImageRepository _imageRepository;
        private readonly IMessenger _messenger;

        public ObservableCollection<MiniatureModel> Miniatures { get; set; } =
            new ObservableCollection<MiniatureModel>();

        public ICommand OnLoadCommand { get; }
        public ICommand OnUnloadCommand { get; }
        public ICommand OpenImageCommand { get; }
        public ICommand SearchImagesCommand { get; }
        public ICommand SortImagesCommand { get; }
        public ICommand FilterFormatCommand { get; }
        public ICommand FilterResolutionCommand { get; }
        public ICommand FilterDateCommand { get; }

        private int _width;
        private int _height;

        public int ImageWidth
        {
            get => _width;
            set
            {
                if (Equals(value, _width)) return;
                _width = value;
                OnPropertyChanged();
            }
        }

        public int ImageHeight
        {
            get => _height;
            set
            {
                if (Equals(value, _height)) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        public ImageListViewModel(ImageRepository imageRepository, IMessenger messenger)
        {
            _imageRepository = imageRepository;
            _messenger = messenger;

            OnLoadCommand = new RelayCommand(OnLoad);
            OnUnloadCommand = new RelayCommand(OnUnload);
            OpenImageCommand = new RelayCommand(OpenImage);
            SearchImagesCommand = new RelayCommand(SearchImages);
            SortImagesCommand = new RelayCommand(SortImages);
            FilterFormatCommand = new RelayCommand(FilterFormat);
            FilterResolutionCommand = new RelayCommand(FilterResolution);
            FilterDateCommand = new RelayCommand(FilterDate);

            _messenger.Register<NewImageAddedMessage>(AddImage);
        }

        private void OnLoad()
        {
            Miniatures.Clear();

            var miniatures = _imageRepository.GetAllImagesSortedByName();
            foreach (var m in miniatures)
            {
                Miniatures.Add(m);
            }
        }

        private void OnUnload()
        {
            Miniatures.Clear();
            _messenger.UnRegister<NewImageAddedMessage>(AddImage);
        }

        private void OpenImage(object parameter)
        {
            if (parameter is MiniatureModel miniature)
            {
                _messenger.Send(new ImageDoubleClickMessage() { Id = miniature.Id });
            }
        }

        private void SearchImages(object parameter)
        {
            if (parameter is TextBox queryBox)
            {
                if (queryBox.Text == string.Empty)
                {
                    OnLoad();
                    return;
                }

                Miniatures.Clear();

                var miniatures = _imageRepository.GetImagesByName(queryBox.Text);
                foreach (var m in miniatures)
                {
                    Miniatures.Add(m);
                }
            }
        }

        private void SortImages(object parameter)
        {
            if (parameter is ComboBox sortType)
            {
                Miniatures.Clear();

                List<MiniatureModel> miniatures;
                if (sortType.SelectedIndex == 0)
                    miniatures = _imageRepository.GetAllImagesSortedByName();
                else
                    miniatures = _imageRepository.GetAllImagesSortedByDate();

                foreach (var m in miniatures)
                {
                    Miniatures.Add(m);
                }
            }
        }

        private void FilterFormat(object parameter)
        {
            if (parameter is ComboBox formatFilter)
            {
                Miniatures.Clear();

                var value = ((ComboBoxItem) formatFilter.SelectedItem).Content.ToString();
                if (value == "Any")
                {
                    OnLoad();
                    return;
                }

                var format = (Format)Enum.Parse(typeof(Format), value);
                var miniatures = _imageRepository.GetImagesByFormat(format);
                foreach (var m in miniatures)
                {
                    Miniatures.Add(m);
                }
            }
        }

        private void FilterResolution()
        {
            Miniatures.Clear();

            var miniatures = _imageRepository.GetImagesByResolution(ImageWidth, ImageHeight);
            foreach (var m in miniatures)
            {
                Miniatures.Add(m);
            }
        }

        private void FilterDate(object parameter)
        {
            if (parameter is DateTime date)
            {
                Miniatures.Clear();

                var miniatures = _imageRepository.GetImagesByDateTaken(date);
                foreach (var m in miniatures)
                {
                    Miniatures.Add(m);
                }
            }
        }

        private void AddImage(NewImageAddedMessage message)
        {
            OnLoad();
        }
    }
}
