using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Services.IO
{
    public interface IFileManager
    {
        bool DirectoryExists(string path);

        void CreateDirectory(string path);

        Stream OpenFile(string path, FileMode mode, FileAccess access);

        Stream OpenFile(string path, FileMode mode, FileAccess access, FileShare share);

        bool FileExists(string path);

        DateTimeOffset GetLastWriteTime(string path);
    }

    public enum FileMode
    {
        CreateNew = 1,
        Create = 2,
        Open = 3,
        OpenOrCreate = 4,
        Truncate = 5,
        Append = 6,
    }

    public enum FileAccess
    {
        Read = 1,
        Write = 2,
        ReadWrite = 3,
    }

    // Summary:
    //     Contains constants for controlling the kind of access other Xamarin.Forms.Labs.Services.IO.IFileManager
    //     objects can have to the same file.
    [Flags]
    public enum FileShare
    {
        // Summary:
        //     Declines sharing of the current file. Any request to open the file (by this
        //     process or another process) will fail until the file is closed.
        None = 0,
        //
        // Summary:
        //     Allows subsequent opening of the file for reading. If this flag is not specified,
        //     any request to open the file for reading (by this process or another process)
        //     will fail until the file is closed.
        Read = 1,
        //
        // Summary:
        //     Allows subsequent opening of the file for writing. If this flag is not specified,
        //     any request to open the file for writing (by this process or another process)
        //     will fail until the file is closed.
        Write = 2,
        //
        // Summary:
        //     Allows subsequent opening of the file for reading or writing. If this flag
        //     is not specified, any request to open the file for reading or writing (by
        //     this process or another process) will fail until the file is closed.
        ReadWrite = 3,
        //
        // Summary:
        //     Allows subsequent deleting of a file.
        Delete = 4,
        //
        // Summary:
        //     Makes the file handle inheritable by child processes.
        Inheritable = 16,
    }
}
