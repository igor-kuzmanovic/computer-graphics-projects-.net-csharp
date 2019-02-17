using Project1.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Model
{
    public class User : ValidatableBindableBase
    {
        private int id;
        private string username;
        private string password;
        private ICollection<Image> images;

        public User()
        {
            images = new HashSet<Image>();
        }

        public int Id { get => id; set => SetProperty(ref id, value); }

        [Required]
        [RegularExpression(@"^[^\d\W]\w+")]
        public string Username { get => username; set => SetProperty(ref username, value); }

        [Required]
        [MinLength(7)]
        [RegularExpression(@"^\w+")]
        public string Password { get => password; set => SetProperty(ref password, value); }

        public ICollection<Image> Images { get => images; set => SetProperty(ref images, value); }

        public void AddImage(Image image)
        {
            if (!images.Contains(image))
            {
                Images.Add(image);
                RaisePropertyChanged("Images");
            }
        }

        public void RemoveImage(Image image)
        {
            if (images.Contains(image))
            {
                Images.Remove(image);
                RaisePropertyChanged("Images");
            }
        }
    }
}
