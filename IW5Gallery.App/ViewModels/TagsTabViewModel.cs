using System.Windows;
using System.Windows.Input;
using IW5Gallery.App.Commands;
using IW5Gallery.App.Views;
using IW5Gallery.BL;
using IW5Gallery.BL.Messages;

namespace IW5Gallery.App.ViewModels
{
    public class TagsTabViewModel : TabViewModelBase
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

        public TagsTabViewModel(IMessenger messenger)
        {
            _messenger = messenger;

            OnUnloadCommand = new RelayCommand(OnUnload);

            _messenger.Register<TagImageDoubleClickMessage>(OpenImage);
            _messenger.Register<ReturnToTagMessage>(CloseImage);
            _messenger.Register<UnloadTagMessage>(UnloadTag);

            TabName = "Tags";
            CurrentView = new TagDetailView();
        }

        private void OnUnload()
        {
            _messenger.UnRegister<TagImageDoubleClickMessage>(OpenImage);
            _messenger.UnRegister<ReturnToTagMessage>(CloseImage);
            _messenger.UnRegister<UnloadTagMessage>(UnloadTag);
        }

        private void OpenImage(TagImageDoubleClickMessage message)
        {
            CurrentView = new ImageDetailView();
            _messenger.Send(new OpenedTagImageMessage { ImageId = message.ImageId, TagId = message.TagId , TagType = message.TagType});
        }

        private void CloseImage(ReturnToTagMessage message)
        {
            CurrentView = new TagDetailView();
            _messenger.Send(new SelectedTagMessage { Id = message.Id , Type = message.Type});
        }

        private void UnloadTag(UnloadTagMessage message)
        {
            CurrentView = new TagDetailView();
        }
    }
}