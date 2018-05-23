using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using IW5Gallery.App.Commands;
using IW5Gallery.BL;
using IW5Gallery.BL.Messages;
using IW5Gallery.BL.Repositories;

namespace IW5Gallery.App.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IMessenger _messenger;
        private readonly FileManager _fileManager;
        
        private ViewModelBase _selectedTab;
        private ObservableCollection<TabViewModelBase> _tabsCollection;
        public string Name { get; set; } = "Not loaded";

        public ICommand ImportImagesCommand { get; set; }

        public MainViewModel(IMessenger messenger, FileManager fileManager)
        {
            _messenger = messenger;
            _fileManager = fileManager;
            
            _messenger.Register<MenuTabChangedMessage>(MenuTabChangedMessageReceived);

            ImportImagesCommand = new RelayCommand(ImportImages);
        }

        public ViewModelBase SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (Equals(value, SelectedTab)) return;
                _selectedTab = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TabViewModelBase> TabsCollection => _tabsCollection ?? (_tabsCollection = new ObservableCollection<TabViewModelBase>
        {
            new PhotosTabViewModel(_messenger),
            new AlbumsTabViewModel(_messenger),
            new TagsTabViewModel(_messenger)
        });

        private void MenuTabChangedMessageReceived(MenuTabChangedMessage obj)
        {
            SelectedTab = TabsCollection[0];
        }

        private void ImportImages()
        {
            _fileManager.BrowseFiles();
        }
    }
}