using System;
using System.Collections.Generic;
using System.Linq;

namespace KissLog
{
    internal class KissLogPackagesContainer
    {
        private readonly List<KissLogPackage> _list;
        public KissLogPackagesContainer()
        {
            _list = new List<KissLogPackage>();
        }

        public void Add(KissLogPackage package)
        {
            if (package == null)
                throw new ArgumentNullException(nameof(package));

            KissLogPackage existing = _list.FirstOrDefault(p => string.Compare(p.Name, package.Name, true) == 0);
            if(existing != null)
            {
                if(existing.Version < package.Version)
                {
                    _list.RemoveAll(p => string.Compare(p.Name, package.Name, true) == 0);
                    _list.Add(package);
                }

                return;
            }

            _list.Add(package);
        }

        public KissLogPackage GetPrimaryPackage()
        {
            if (!_list.Any())
                return Constants.UnknownKissLogPackage;

            KissLogPackage package = _list.LastOrDefault(p => string.Compare(p.Name, "KissLog", true) != 0);
            if (package == null)
                package = _list.LastOrDefault();

            return package;
        }

        internal IEnumerable<KissLogPackage> GetAll()
        {
            return _list.ToList();
        }
    }
}
