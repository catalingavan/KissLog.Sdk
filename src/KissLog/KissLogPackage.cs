using System;

namespace KissLog
{
    internal class KissLogPackage
    {
        public string Name { get; }
        public Version Version { get; }

        public KissLogPackage(string name, Version version)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (version == null)
                throw new ArgumentNullException(nameof(version));

            Name = name;
            Version = version;
        }
    }
}
