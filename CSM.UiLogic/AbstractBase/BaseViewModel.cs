using CSM.Framework.ServiceLocation;
using CSM.UiLogic.Commands;
using CSM.UiLogic.Services;
using System.IO;

namespace CSM.UiLogic.AbstractBase
{
    internal abstract class BaseViewModel : BaseNotifiable
    {
        private readonly string baseViewDefinitionPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Custom Songs Manager", "ViewDefinitions");
        private bool loadingInProgress;
        private string loadingInProgressMessage = string.Empty;

        public bool LoadingInProgress
        {
            get => loadingInProgress;
            set
            {
                if (loadingInProgress == value)
                    return;
                loadingInProgress = value;
                OnPropertyChanged();
            }
        }

        public string LoadingInProgressMessage
        {
            get => loadingInProgressMessage;
            set
            {
                if (loadingInProgressMessage == value)
                    return;
                loadingInProgressMessage = value;
                OnPropertyChanged();
            }
        }

        public virtual async Task<ViewDefinition?> SaveViewDefinitionAsync(Stream stream, SavableUiElement savableUiElement, string? name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                var editNewViewDefinitionName = new NewViewDefinitionViewModel(ServiceLocator, "Cancel", EditViewModelCommandColor.Default, "Continue", EditViewModelCommandColor.Default);
                UserInteraction.ShowWindow(editNewViewDefinitionName);
                if (!editNewViewDefinitionName.Continue)
                    return null;
                name = editNewViewDefinitionName.ViewDefinitionName;
            }

            if (string.IsNullOrEmpty(name))
                return null;

            Directory.CreateDirectory(Path.Combine(baseViewDefinitionPath, savableUiElement.ToString()));

            var filePath = Path.Combine(baseViewDefinitionPath, savableUiElement.ToString(), $"{name}.xml");
            var fileExisted = File.Exists(filePath);

            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            stream.Position = 0;
            await stream.CopyToAsync(fileStream);

            if (!fileExisted)
            {
                return new ViewDefinition
                {
                    Name = name,
                    Stream = stream
                };
            }
            return null;
        }

        public virtual void DeleteViewDefinition(SavableUiElement savableUiElement, string name)
        {
            var path = Path.Combine(baseViewDefinitionPath, savableUiElement.ToString(), $"{name}.xml");
            if (File.Exists(path))
                File.Delete(path);
        }

        public async Task<List<ViewDefinition>> LoadViewDefinitionsAsync(SavableUiElement savableUiElement)
        {
            var retval = new List<ViewDefinition>();

            var path = Path.Combine(baseViewDefinitionPath, savableUiElement.ToString());
            if (!Directory.Exists(path))
                return retval;

            foreach (var file in Directory.GetFiles(path))
            {
                using var fileStream = File.OpenRead(file);
                var stream = new MemoryStream();
                await fileStream.CopyToAsync(stream);
                stream.Position = 0;

                var viewDefinition = new ViewDefinition
                {
                    Name = Path.GetFileNameWithoutExtension(file),
                    Stream = stream
                };
                retval.Add(viewDefinition);
            }

            return retval;
        }

        protected ICommandFactory CommandFactory { get; }

        protected IServiceLocator ServiceLocator { get; }

        protected IUserInteraction UserInteraction { get; }

        protected IUiText UiText { get; }

        protected BaseViewModel(IServiceLocator serviceLocator)
        {
            ServiceLocator = serviceLocator;
            CommandFactory = ServiceLocator.GetService<ICommandFactory>();
            UserInteraction = ServiceLocator.GetService<IUserInteraction>();
            UiText = ServiceLocator.GetService<IUiText>();
        }

        protected void SetLoadingInProgress(bool value, string message)
        {
            LoadingInProgressMessage = message;
            LoadingInProgress = value;
        }
    }
}
