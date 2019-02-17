using Project1.Core;
using Project1.Database;
using Project1.Model;
using Project1.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1.ViewModel
{
    public class AddImageViewModel : ValidatableBindableBase
    {
        const string placeholderImage = @"..\Images\placeholder.png";

        private string path;
        private string title;
        private string description;

        private readonly DatabaseContext dbContext = new DatabaseContext();
        private readonly LoginService loginService = LoginService.Instance;
    
        public AddImageViewModel()
        {
            UploadImageCommand = new MyICommand(OnUploadImage);
            AddImageCommand = new MyICommand(OnAddImage, CanAddImage);

            Path = placeholderImage;
        }

        [Required]
        public string Path
        {
            get => path; set
            {
                SetProperty(ref path, value);
                AddImageCommand.RaiseCanExecuteChanged();
            }
        }

        [Required]
        public string Title
        {
            get => title; set
            {
                SetProperty(ref title, value);
                AddImageCommand.RaiseCanExecuteChanged();
            }
        }

        public string Description
        {
            get => description; set
            {
                SetProperty(ref description, value);
                AddImageCommand.RaiseCanExecuteChanged();
            }
        }

        public MyICommand UploadImageCommand { get; }

        public MyICommand AddImageCommand { get; }

        private void OnUploadImage()
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = "Select an image to upload...",
                Filter = "JPG files (*.jpg, *.jpeg) | *.jpg; *.jpeg",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                Path = fileDialog.FileName;
            }
        }

        private void OnAddImage()
        {
            Image image = new Image()
            {
                Path = Path,
                Title = Title,
                Description = Description,
                CreationDateTime = DateTime.Now
            };

            User user = dbContext.Users.Single(u => u.Id == loginService.CurrentUser.Id);
            user.AddImage(image);
            dbContext.SaveChanges();

            MessageBox.Show("Image successfully added!");
        }

        private bool CanAddImage()
        {
            return Validate();
        }

        private bool Validate()
        {
            return !HasErrors && Path != placeholderImage;
        }
    }
}
