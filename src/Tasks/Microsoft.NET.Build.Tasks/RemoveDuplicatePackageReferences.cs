﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Globalization;
using NuGet.ProjectModel;
using NuGet.Packaging.Core;
using NuGet.Versioning;

namespace Microsoft.NET.Build.Tasks
{
    /// <summary>
    /// Removes Duplicate Package References.
    /// </summary>
    public class RemoveDuplicatePackageReferences : TaskBase
    {
        private readonly List<ITaskItem> _packageList = new List<ITaskItem>();

        [Required]
        public ITaskItem[] InputPackageRefernces { get; set; }

        /// <summary>
        /// Unique package references
        /// </summary>

        [Output]
        public ITaskItem[] UniquePackageRefernces
        {
            get { return _packageList.ToArray(); }
        }

        protected override void ExecuteCore()
        {
            var packageSet = new HashSet<PackageIdentity>();

            foreach(var pkg in InputPackageRefernces)
            {
                var pkgName = pkg.ItemSpec;
                var pkgVersion = NuGetVersion.Parse(pkg.GetMetadata("Version"));
                packageSet.Add(new PackageIdentity(pkgName, pkgVersion));
            }

            foreach (var pkg in packageSet)
            {
                TaskItem item = new TaskItem(pkg.Id);
                item.SetMetadata("Version", pkg.Version.ToString());
                _packageList.Add(item);
            }
        }
    }
}
