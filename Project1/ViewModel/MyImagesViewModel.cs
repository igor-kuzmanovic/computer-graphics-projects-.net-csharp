using Project1.Core;
using Project1.Database;
using Project1.Model;
using Project1.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.ViewModel
{
    public class MyImagesViewModel : ValidatableBindableBase
    {
        private ObservableCollection<Image> images;

        private readonly DatabaseContext dbContext = new DatabaseContext();
        private readonly LoginService loginService = LoginService.Instance;

        public MyImagesViewModel()
        {
            Images = new ObservableCollection<Image>(dbContext.Users.Include(u => u.Images).Single(u => u.Id == loginService.CurrentUser.Id).Images);
        }

        public ObservableCollection<Image> Images { get => images; set => images = value; }
    }
}
