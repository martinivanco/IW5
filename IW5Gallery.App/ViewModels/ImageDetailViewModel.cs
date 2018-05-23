using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using IW5Gallery.App.Commands;
using IW5Gallery.BL;
using IW5Gallery.BL.Messages;
using IW5Gallery.BL.Models;
using IW5Gallery.BL.Repositories;
using Cursor = System.Windows.Forms.Cursor;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Controls;
using IW5Gallery.DAL.Entities;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace IW5Gallery.App.ViewModels
{
    public class ImageDetailViewModel : ViewModelBase
    {
        private readonly ImageRepository _imageRepository;
        private readonly AlbumRepository _albumRepository;
        private TagRepository _tagRepository;
        private readonly IMessenger _messenger;
        private ImageDetailModel _imageDetail;
        private TagModel _tag;
        private Guid? _parentAlbum;
        private Guid? _parentTag;
        private TagType _parentTagType;

        public ICommand UpdateImageCommand { get; }
        public ICommand ReturnCommand { get; }
        public ICommand AddImageToAlbumCommand { get; }
        public ICommand RemoveImageFromAlbumCommand { get; }
        public ICommand AddPersonCommand { get; }
        public ICommand AddThingCommand { get; }
        public ICommand RemoveTagCommand { get; }
        public ICommand RemoveImageCommand { get; }
        public ICommand OnUnloadCommand { get; }
        public ICommand SetLocationCommand { get; }

        public ImageDetailModel Detail
        {
            get => _imageDetail;
            set
            {
                if (Equals(value, _imageDetail)) return;
                _imageDetail = value;
                OnPropertyChanged();
            }
        }
        public TagModel Tag
        {
            get => _tag;
            set
            {
                if (Equals(value, _tag)) return;
                _tag = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MiniatureModel> Albums { get; set; } = new ObservableCollection<MiniatureModel>();
        public ObservableCollection<MiniatureModel> Persons { get; set; } = new ObservableCollection<MiniatureModel>();
        public ObservableCollection<MiniatureModel> Things { get; set; } = new ObservableCollection<MiniatureModel>();
        public IList<TagType> TagTypes => Enum.GetValues(typeof(TagType)).Cast<TagType>().ToList();
        public double ViewportWidth { get; set; }
        public double ViewportHeight { get; set; }

        public ImageDetailViewModel(ImageRepository imageRepository, AlbumRepository albumRepository, TagRepository tagRepository, IMessenger messenger)
        {
            _imageRepository = imageRepository;
            _albumRepository = albumRepository;
            _tagRepository = tagRepository;
            _messenger = messenger;

            UpdateImageCommand = new RelayCommand(ImageDetailsChanged);
            ReturnCommand = new RelayCommand(ReturnBack);
            AddImageToAlbumCommand = new RelayCommand(AddImageToAlbum);
            RemoveImageFromAlbumCommand = new RelayCommand(RemoveImageFromAlbum);
            AddPersonCommand = new RelayCommand(AddPerson);
            AddThingCommand = new RelayCommand(AddThing);
            RemoveTagCommand = new RelayCommand(RemoveTag);
            RemoveImageCommand = new RelayCommand(RemoveImageFromDb);
            OnUnloadCommand = new RelayCommand(OnUnload);
            SetLocationCommand = new RelayCommand(SetLocation);

            _messenger.Register<OpenedImageMessage>(LoadImageDetail);
            _messenger.Register<OpenedAlbumImageMessage>(LoadAlbumImageDetail);
            _messenger.Register<OpenedTagImageMessage>(LoadTagImageDetail);

            Albums.Clear();
            var albums = _albumRepository.GetAllAlbums();
            foreach (var album in albums)
            {
                Albums.Add(album);
            }

            Persons.Clear();
            var persons = _tagRepository.GetAllPersons();
            foreach (var person in persons)
            {
                Persons.Add(person);
            }

            Things.Clear();
            var things = _tagRepository.GetAllThings();
            foreach (var thing in things)
            {
                Things.Add(thing);
            }

            Tag = new TagModel
            {
                Location = new Location()
            };
        }

        private void ImageDetailsChanged(object parameter)
        {
            if (parameter is ImageDetailModel imageDetail)
            {
                if (imageDetail.Name == string.Empty)
                {
                    MessageBox.Show("Image name can't be empty!");
                    return;
                }
                _imageRepository.UpdateImageInfo(imageDetail);
                MessageBox.Show("Image updated!");
            }
        }

        private void ReturnBack(object parameter)
        {
            if (_parentAlbum == null && _parentTag == null)
                _messenger.Send(new ReturnToImageListMessage());
            if (_parentAlbum != null && _parentTag == null)
                _messenger.Send(new ReturnToAlbumMessage { Id = (Guid)_parentAlbum });
            if (_parentAlbum == null && _parentTag != null)
                _messenger.Send(new ReturnToTagMessage { Id = (Guid)_parentTag, Type = _parentTagType });
        }

        private void AddImageToAlbum(object parameter)
        {
            if (parameter is MiniatureModel album)
            {
                if (Detail.Albums.FirstOrDefault(a => a.Id == album.Id) == null)
                {
                    _albumRepository.AddImageToAlbum(Detail.Id, album.Id);
                    Detail = _imageRepository.GetImageById(Detail.Id);
                    MessageBox.Show("Image added to album!");
                }
                else
                {
                    MessageBox.Show("Image is already in album!");
                }
            }
        }

        private void RemoveImageFromAlbum(object parameter)
        {
            if (parameter is MiniatureModel album)
            {
                if (Detail.Albums.FirstOrDefault(a => a.Id == album.Id) == null)
                {
                    MessageBox.Show("Image is not in this album!");
                }
                else
                {
                    _albumRepository.RemoveImageFromAlbum(Detail.Id, album.Id);
                    Detail = _imageRepository.GetImageById(Detail.Id);
                    MessageBox.Show("Image removed from album!");
                }
            }
        }

        private void AddPerson(object parameter)
        {
            if (parameter is MiniatureModel person)
            {
                Tag.TaggableId = person.Id;
                Tag.TagType = TagType.Person;
                _imageRepository.AddTagToImage(Tag, Detail.Id);
                Tag = new TagModel
                {
                    Location = new Location()
                };
                Detail = _imageRepository.GetImageById(Detail.Id);
                MessageBox.Show("Person tag added!");
            }
        }

        private void AddThing(object parameter)
        {
            if (parameter is MiniatureModel thing)
            {
                Tag.TaggableId = thing.Id;
                Tag.TagType = TagType.Thing;
                _imageRepository.AddTagToImage(Tag, Detail.Id);
                Tag = new TagModel
                {
                    Location = new Location()
                };
                Detail = _imageRepository.GetImageById(Detail.Id);
                MessageBox.Show("Thing tag added!");
            }
        }

        private void RemoveTag(object parameter)
        {
            if (parameter is TagModel tag)
            {
                _imageRepository.RemoveTagFromImage(tag.Id);
                Detail = _imageRepository.GetImageById(Detail.Id);
                MessageBox.Show("Tag removed!");
            }
        }

        private void RemoveImageFromDb(object parameter)
        {
            if (parameter is ImageDetailModel imageDetail)
            {
                _imageRepository.RemoveImage(imageDetail.Id);
                ReturnBack(parameter);
                MessageBox.Show("Image removed!");
            }
        }

        private void OnUnload()
        {
            Detail = null;
            _messenger.UnRegister<OpenedImageMessage>(LoadImageDetail);
            _messenger.UnRegister<OpenedAlbumImageMessage>(LoadAlbumImageDetail);
            _messenger.UnRegister<OpenedTagImageMessage>(LoadTagImageDetail);
        }

        private void SetLocation(object parameter)
        {
            if (parameter is MouseButtonEventArgs e)
            {
                var position = e.GetPosition((IInputElement)e.Source);

                var location = new Location
                {
                    XCoordinate = Math.Round(position.X / ViewportWidth, 3),
                    YCoordinate = Math.Round(position.Y / ViewportHeight, 3),
                    Height = 50,
                    Width = 50
                };

                Tag = new TagModel()
                {
                    Id = Tag.Id,
                    Location = location,
                    Name = Tag.Name,
                    TaggableId = Tag.TaggableId,
                    TagType = Tag.TagType
                };
            }
        }

        private void LoadImageDetail(OpenedImageMessage message)
        {
            Detail = _imageRepository.GetImageById(message.ImageId);
            _parentAlbum = null;
            _parentTag = null;
        }

        private void LoadAlbumImageDetail(OpenedAlbumImageMessage message)
        {
            Detail = _imageRepository.GetImageById(message.ImageId);
            _parentAlbum = message.AlbumId;
            _parentTag = null;
        }

        private void LoadTagImageDetail(OpenedTagImageMessage message)
        {
            Detail = _imageRepository.GetImageById(message.ImageId);
            _parentAlbum = null;
            _parentTag = message.TagId;
            _parentTagType = message.TagType;
        }
    }
}
