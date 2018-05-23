using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IW5Gallery.App.Commands;
using IW5Gallery.BL;
using IW5Gallery.BL.Messages;
using IW5Gallery.BL.Models;
using IW5Gallery.BL.Repositories;
using IW5Gallery.DAL.Entities;

namespace IW5Gallery.App.ViewModels
{
    public class TagDetailViewModel : ViewModelBase
    {
        private readonly TagRepository _tagRepository;
        private readonly Messenger _messenger;
        private ThingDetailModel _detail;
        private TagType _tagType;

        public ICommand RemoveTaggableCommand { get; }
        public ICommand OpenImageCommand { get; }
        public ICommand OnUnloadCommand { get; }

        public ThingDetailModel Detail
        {
            get => _detail;
            set
            {
                if (Equals(value, _detail)) return;
                _detail = value;
                OnPropertyChanged();
            }
        }

        public TagDetailViewModel(TagRepository tagRepository, Messenger messenger)
        {
            _tagRepository = tagRepository;
            _messenger = messenger;

            RemoveTaggableCommand = new RelayCommand(RemoveTaggable);
            OpenImageCommand = new RelayCommand(OpenImage);
            OnUnloadCommand = new RelayCommand(OnUnload);

            _messenger.Register<SelectedTagMessage>(SelectedTag);
        }

        private void RemoveTaggable()
        {
            _messenger.Send(new DeleteTagMessage {Id = Detail.Id, Type = _tagType});
        }

        private void OpenImage(object parameter)
        {
            if (parameter is MiniatureModel miniature)
            {
                _messenger.Send(new TagImageDoubleClickMessage {ImageId = miniature.Id, TagId = Detail.Id, TagType = _tagType});
            }
        }

        private void OnUnload()
        {
            _messenger.UnRegister<SelectedTagMessage>(SelectedTag);
        }

        private void SelectedTag(SelectedTagMessage message)
        {
            if (message.Type == TagType.Person)
            {
                var person = _tagRepository.GetPersonById(message.Id);
                Detail = new ThingDetailModel
                {
                    Id = person.Id,
                    Name = person.Name + " " + person.Surname,
                    Images = person.Images
                };
                _tagType = TagType.Person;
            }
            else
            {
                Detail = _tagRepository.GetThingById(message.Id);
                _tagType = TagType.Thing;
            }
        }
    }
}
