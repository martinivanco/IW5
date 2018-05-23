using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    public class TagListViewModel : ViewModelBase
    {
        private readonly TagRepository _tagRepository;
        private readonly IMessenger _messenger;

        public ObservableCollection<MiniatureModel> Persons { get; set; } = new ObservableCollection<MiniatureModel>();
        public ObservableCollection<MiniatureModel> Things { get; set; } = new ObservableCollection<MiniatureModel>();

        public ICommand SelectPersonCommand { get; }
        public ICommand SelectThingCommand { get; }
        public ICommand AddPersonCommand { get; }
        public ICommand AddThingCommand { get; }
        public ICommand OnLoadCommand { get; }
        public ICommand OnUnloadCommand { get; }
        public ICommand SearchCommand { get; }

        public TagListViewModel(TagRepository tagRepository, IMessenger messenger)
        {
            _tagRepository = tagRepository;
            _messenger = messenger;

            SelectPersonCommand = new RelayCommand(PersonSelectionChanged);
            SelectThingCommand = new RelayCommand(ThingSelectionChanged);
            AddPersonCommand = new RelayCommand(AddPerson);
            AddThingCommand = new RelayCommand(AddThing);
            OnLoadCommand = new RelayCommand(OnLoad);
            OnUnloadCommand = new RelayCommand(OnUnload);
            SearchCommand = new RelayCommand(SearchTags);

            _messenger.Register<DeleteTagMessage>(RemoveTag);
        }

        private void PersonSelectionChanged(object parameter)
        {
            if (parameter is MiniatureModel person)
            {
                _messenger.Send(new SelectedTagMessage { Id = person.Id , Type = TagType.Person});
            }
        }

        private void ThingSelectionChanged(object parameter)
        {
            if (parameter is MiniatureModel thing)
            {
                _messenger.Send(new SelectedTagMessage { Id = thing.Id, Type = TagType.Thing });
            }
        }

        private void AddPerson(object parameter)
        {
            if (parameter is TextBox name)
            {
                if (name.Text == string.Empty || !name.Text.Contains(" "))
                {
                    MessageBox.Show("Please specify a name and a surname separated by a space for the person.");
                    return;
                }

                var forname = name.Text.Substring(0, name.Text.IndexOf(' '));
                var surname = name.Text.Substring(name.Text.IndexOf(' ') + 1);

                if (forname == string.Empty || surname == string.Empty)
                {
                    MessageBox.Show("Please specify a name and a surname separated by a space for the person.");
                    return;
                }

                if (_tagRepository.ContainsPerson(forname, surname))
                {
                    MessageBox.Show("Person tag that you are trying to create already exists");
                    return;
                }

                _tagRepository.InsertPerson(new PersonDetailModel
                {
                    Name = forname,
                    Surname = surname
                });
            }

            OnLoad();
        }

        private void AddThing(object parameter)
        {
            if (parameter is TextBox name)
            {
                if (name.Text == string.Empty)
                {
                    MessageBox.Show("Please specify a name for the thing.");
                    return;
                }

                if (_tagRepository.ContainsThing(name.Text))
                {
                    MessageBox.Show("Thing tag that you are trying to create already exists");
                    return;
                }

                _tagRepository.InsertThing(new ThingDetailModel
                {
                    Name = name.Text
                });
            }

            OnLoad();
        }

        private void OnLoad()
        {
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
        }

        private void OnUnload()
        {
            _messenger.UnRegister<DeleteTagMessage>(RemoveTag);
        }

        private void SearchTags(object parameter)
        {
            if (parameter is TextBox queryBox)
            {
                if (queryBox.Text == string.Empty)
                {
                    OnLoad();
                    return;
                }

                Persons.Clear();
                var persons = _tagRepository.GetPersonsByName(queryBox.Text);
                foreach (var person in persons)
                {
                    Persons.Add(person);
                }

                Things.Clear();
                var things = _tagRepository.GetThingsByName(queryBox.Text);
                foreach (var thing in things)
                {
                    Things.Add(thing);
                }
            }
        }

        private void RemoveTag(DeleteTagMessage message)
        {
            if (message.Type == TagType.Person)
                _tagRepository.RemovePerson(message.Id);
            else
                _tagRepository.RemoveThing(message.Id);

            _messenger.Send(new UnloadTagMessage());
            OnLoad();
        }
    }
}
