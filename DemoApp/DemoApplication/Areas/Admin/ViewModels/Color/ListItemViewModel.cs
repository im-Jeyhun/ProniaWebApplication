﻿namespace DemoApplication.Areas.Admin.ViewModels.Color
{
    public class ListItemViewModel
    {
        public ListItemViewModel(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
