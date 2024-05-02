using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI.LearningManagement.ViewModels {
    class ModuleDetailViewModel {
        public ModuleDetailViewModel() {
            Module module = new Module();
        }
        public string Name {
            get => module?.Name ?? string.Empty;
            set { if (module != null) module.Name = value; }
        }
        public string Description {
            get => module?.Description ?? string.Empty;
            set { if (module != null) module.Description = value; }
        }

        private Module module;

        public void AddModule(Shell s) {
            ModuleService.Current.Add(module);
            s.GoToAsync("//CourseDetail");
        }
    }
}
