using Library.LearningManagement.Database;
using Library.LearningManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LearningManagement.Services {
    public class ModuleService {

        private static ModuleService? _instance;

        // singleton to make sure we only have one module service
        public static ModuleService Current {
            get {
                if (_instance == null) {
                    _instance = new ModuleService();
                }

                return _instance;
            }
        }

        // initializes ModuleService
        private ModuleService() {

        }

        // adds a module to the list
        public void Add(Module module) {
            FakeDatabase.Modules.Add(module);
        }

        // removes a module from the list
        public void Remove(Module module) {
            FakeDatabase.Modules.Remove(module);
        }

        // returns the list of modules
        public List<Module> Modules {
            get {
                return FakeDatabase.Modules;
            }
        }

        // return all modules which contain query in name
        public IEnumerable<Module> Search(string query) {
            return Modules.Where(module => module.Name.ToUpper().Contains(query.ToUpper()));
        }

    }
}
