using Project1.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Model
{
    public class Image : BindableBase
    {
        private int id;
        private string path;
        private string title;
        private string description;
        private DateTime creationDateTime;

        public int Id { get => id; set => SetProperty(ref id, value); }

        [Required]
        public string Path { get => path; set => SetProperty(ref path, value); }

        [Required]
        public string Title { get => title; set => SetProperty(ref title, value); }

        public string Description { get => description; set => SetProperty(ref description, value); }

        public DateTime CreationDateTime { get => creationDateTime; set => SetProperty(ref creationDateTime, value); }
    }
}
