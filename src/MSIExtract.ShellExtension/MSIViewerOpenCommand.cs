﻿// Copyright (c) William Kent. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Windows.ApplicationModel;

namespace MSIExtract.ShellExtension
{
    /// <summary>
    /// Implements the "Open in MSI Viewer" Windows Shell command.
    /// </summary>
    [ComVisible(true)]
    [Guid("754de965-4071-4a89-9620-cf6fe858dca2")]
    public sealed class MSIViewerOpenCommand : ExplorerCommandBase
    {
        /// <inheritdoc/>
        public override ExplorerCommandFlag Flags => ExplorerCommandFlag.Default;

        /// <inheritdoc/>
        public override ExplorerCommandState GetState(IEnumerable<string> selectedFiles)
        {
            if (selectedFiles == null)
            {
                throw new ArgumentNullException(nameof(selectedFiles));
            }

            if (selectedFiles.Count() != 1)
            {
                return ExplorerCommandState.Hidden;
            }

            if (Path.GetExtension(selectedFiles.First()) == ".msi")
            {
                return ExplorerCommandState.Enabled;
            }
            else
            {
                return ExplorerCommandState.Hidden;
            }
        }

        /// <inheritdoc/>
        public override string? GetTitle(IEnumerable<string> selectedFiles) => "Open in MSI Viewer";

        /// <inheritdoc/>
        public override string? GetToolTip(IEnumerable<string> selectedFiles) => null;

        /// <inheritdoc/>
        public override void Invoke(IEnumerable<string> selectedFiles)
        {
            if (selectedFiles == null)
            {
                throw new ArgumentNullException(nameof(selectedFiles));
            }

            if (!selectedFiles.Any())
            {
                // No files selected, nothing to do.
                return;
            }

            string path = selectedFiles.First();
            if (Path.GetExtension(path) == ".msi")
            {
                string exePath = Path.Combine(Package.Current.InstalledLocation.Path, "MSIExtract", "MSIExtract.exe");
                System.Diagnostics.Process.Start(exePath, path);
            }
        }
    }
}
