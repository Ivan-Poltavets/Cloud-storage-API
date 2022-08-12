﻿using CloudStorage.Core.Constants;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;

namespace CloudStorage.Infrastructure.Helpers
{
    public static class FolderHelper
    {
        public static async Task<string> GeneratePath(Guid? folderId, IRepository<FolderInfo> repository)
        {
            string path = Constants.MainDirectory;

            if (folderId is not null)
            {
                var folder = await repository.GetByIdAsync(folderId);
                path = Path.Combine(folder.Path, folder.Name.ToLower());
            }

            return path;
        }
    }
}