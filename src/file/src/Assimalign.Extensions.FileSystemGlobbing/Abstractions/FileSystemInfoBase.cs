﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileSystemGlobbing.Abstractions
{
    /// <summary>
    /// Shared abstraction for files and directories
    /// </summary>
    public abstract class FileSystemInfoBase
    {
        /// <summary>
        /// A string containing the name of the file or directory
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// A string containing the full path of the file or directory
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// The parent directory for the current file or directory
        /// </summary>
        public abstract DirectoryInfoBase ParentDirectory { get; }
    }
}