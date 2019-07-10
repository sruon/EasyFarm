﻿// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
//  
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using System;
using System.IO;
using EasyFarm.Logging;
using Microsoft.Win32;

namespace EasyFarm.Persistence
{
    /// <summary>
    ///     Manages the saving and loading of game data to
    ///     file under their own extensions.
    /// </summary>
    public class SettingsManager
    {
        private readonly string _extension;
        private readonly string _fileType;
        private readonly string _startPath;
        private readonly Func<string> _defaultFileName;

        public SettingsManager(string extension, string fileType, Func<string> defaultFileName)
        {
            _defaultFileName = defaultFileName;            
            _extension = extension;
            _fileType = fileType;
            _startPath = Environment.CurrentDirectory;
        }

        /// <summary>
        ///     Saves settings to file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public bool TrySave<T>(T value)
        {
            string path = GetSavePath();

            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            try
            {                
                Serialization.Serialize(path, value);
                return true;
            }
            catch (InvalidOperationException ex)
            {
                Logger.Log(new LogEntry(LoggingEventType.Error, $"{GetType()}.{nameof(TrySave)} : Failure on serialize settings", ex));
                return false;
            }
        }

        /// <summary>
        ///     Loads settings from file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// ///
        /// <exception cref="InvalidOperationException"></exception>
        public T TryLoad<T>()
        {
            string path = GetLoadPath();

            if (!File.Exists(path))
            {
                return default(T);
            }

            try
            {                
                return Serialization.Deserialize<T>(path);
            }
            catch (InvalidOperationException ex)
            {
                Logger.Log(new LogEntry(LoggingEventType.Error, $"{GetType()}.{nameof(TrySave)} : Failure on de-serialize settings", ex));
                return default(T);
            }
        }

        private string GetSavePath()
        {
            string fileName = null;

            if (_defaultFileName != null)
            {
                fileName = _defaultFileName() + $".{_extension}";
            }

            var sfd = new SaveFileDialog
            {
                OverwritePrompt = true,
                InitialDirectory = _startPath,
                AddExtension = true,
                DefaultExt = _extension,
                FileName = fileName,
                Filter = GetFilter()
            };

            sfd.ShowDialog();
            return sfd.FileName;
        }

        private string GetLoadPath()
        {
            string fileName = null;

            if (_defaultFileName != null)
            {
                fileName = _defaultFileName() + $".{_extension}";
            }
            var ofd = new OpenFileDialog
            {
                InitialDirectory = _startPath,
                AddExtension = true,
                DefaultExt = _extension,
                FileName=fileName,
                Filter = GetFilter()
            };

            ofd.ShowDialog();
            return ofd.FileName;
        }

        private string GetFilter()
        {
            return string.Format("{0} ({1})|*.{1}", _fileType, _extension);
        }
    }
}