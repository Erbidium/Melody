﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Melody.Infrastructure.Data {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SqlScriptsResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SqlScriptsResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Melody.Infrastructure.Data.SqlScriptsResource", typeof(SqlScriptsResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Playlists (Name, Link, AuthorId)
        ///OUTPUT Inserted.Id
        ///VALUES (@Name, @Link, @AuthorId).
        /// </summary>
        internal static string CreatePlaylist {
            get {
                return ResourceManager.GetString("CreatePlaylist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Songs (Name, Path, AuthorName, Year, GenreId)
        ///OUTPUT Inserted.Id
        ///VALUES (@Name, @Path, @AuthorName, @Year, @GenreId).
        /// </summary>
        internal static string CreateSong {
            get {
                return ResourceManager.GetString("CreateSong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Playlists
        ///SET IsDeleted = 1
        ///WHERE Id = @Id AND IsDeleted = 0.
        /// </summary>
        internal static string DeletePlaylist {
            get {
                return ResourceManager.GetString("DeletePlaylist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Songs
        ///SET IsDeleted = 1
        ///WHERE Id = @Id AND IsDeleted = 0.
        /// </summary>
        internal static string DeleteSong {
            get {
                return ResourceManager.GetString("DeleteSong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id, Name
        ///FROM Genres.
        /// </summary>
        internal static string GetAllGenres {
            get {
                return ResourceManager.GetString("GetAllGenres", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id, Name, Link, AuthorId, IsDeleted
        ///FROM Playlists
        ///WHERE IsDeleted = 0.
        /// </summary>
        internal static string GetAllPlaylists {
            get {
                return ResourceManager.GetString("GetAllPlaylists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id, Name, Path, AuthorName, Year, GenreId, IsDeleted
        ///FROM Songs
        ///WHERE IsDeleted = 0
        ///.
        /// </summary>
        internal static string GetAllSongs {
            get {
                return ResourceManager.GetString("GetAllSongs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id, Name
        ///FROM Genres
        ///WHERE Id = @Id.
        /// </summary>
        internal static string GetGenreById {
            get {
                return ResourceManager.GetString("GetGenreById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id, Name, Link, AuthorId, IsDeleted
        ///FROM Playlists
        ///WHERE Id = @Id AND IsDeleted = 0.
        /// </summary>
        internal static string GetPlaylistById {
            get {
                return ResourceManager.GetString("GetPlaylistById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Id, Name, Path, AuthorName, Year, GenreId, IsDeleted
        ///FROM Songs
        ///WHERE Id = @Id AND IsDeleted = 0
        ///.
        /// </summary>
        internal static string GetSongById {
            get {
                return ResourceManager.GetString("GetSongById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string GetSongs {
            get {
                return ResourceManager.GetString("GetSongs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Playlists
        ///SET Name = @Name, Link = @Link, AuthorId = @AuthorId
        ///WHERE Id = @Id AND IsDeleted = 0.
        /// </summary>
        internal static string UpdatePlaylist {
            get {
                return ResourceManager.GetString("UpdatePlaylist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Songs
        ///SET Name = @Name, Path = @Path, AuthorName = @AuthorName, Year = @Year, GenreId = @GenreId
        ///WHERE Id = @Id AND IsDeleted = 0.
        /// </summary>
        internal static string UpdateSong {
            get {
                return ResourceManager.GetString("UpdateSong", resourceCulture);
            }
        }
    }
}