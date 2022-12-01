﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Melody.Infrastructure.Data {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
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
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
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
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
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
        ///   Ищет локализованную строку, похожую на INSERT INTO Playlists (Name, AuthorId)
        ///    OUTPUT Inserted.Id
        ///VALUES (@Name, @AuthorId).
        /// </summary>
        internal static string CreatePlaylist {
            get {
                return ResourceManager.GetString("CreatePlaylist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на INSERT INTO UserRefreshTokens(UserId, RefreshToken)
        ///VALUES (@UserId, @Token);.
        /// </summary>
        internal static string CreateRefreshToken {
            get {
                return ResourceManager.GetString("CreateRefreshToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на INSERT INTO Roles
        ///VALUES (@Name, @NormalizedName).
        /// </summary>
        internal static string CreateRole {
            get {
                return ResourceManager.GetString("CreateRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на INSERT INTO Songs (UserId, Name, Path, AuthorName, Year, SizeBytes, UploadedAt, GenreId, Duration)
        ///    OUTPUT Inserted.Id
        ///VALUES (@UserId, @Name, @Path, @AuthorName, @Year, @SizeBytes, @UploadedAt, @GenreId, @Duration).
        /// </summary>
        internal static string CreateSong {
            get {
                return ResourceManager.GetString("CreateSong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на INSERT INTO Users (UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, PhoneNumber)
        ///    OUTPUT Inserted.Id
        ///VALUES (@UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @PhoneNumber)
        ///.
        /// </summary>
        internal static string CreateUser {
            get {
                return ResourceManager.GetString("CreateUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на UPDATE Playlists
        ///SET IsDeleted = 1
        ///WHERE Id = @Id
        ///  AND IsDeleted = 0.
        /// </summary>
        internal static string DeletePlaylist {
            get {
                return ResourceManager.GetString("DeletePlaylist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на DELETE From PlaylistSongs
        ///WHERE PlaylistId = @Id AND SongId = @SongId.
        /// </summary>
        internal static string DeletePlaylistSong {
            get {
                return ResourceManager.GetString("DeletePlaylistSong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на DELETE
        ///FROM UserRefreshTokens
        ///WHERE RefreshToken = @Token;.
        /// </summary>
        internal static string DeleteRefreshToken {
            get {
                return ResourceManager.GetString("DeleteRefreshToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на DELETE
        ///FROM Roles
        ///WHERE Id = @Id;.
        /// </summary>
        internal static string DeleteRole {
            get {
                return ResourceManager.GetString("DeleteRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на DELETE
        ///FROM UserRoles
        ///WHERE UserId = @UserId;.
        /// </summary>
        internal static string DeleteRoles {
            get {
                return ResourceManager.GetString("DeleteRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на UPDATE Songs
        ///SET IsDeleted = 1
        ///WHERE Id = @Id
        ///  AND IsDeleted = 0.
        /// </summary>
        internal static string DeleteSong {
            get {
                return ResourceManager.GetString("DeleteSong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на DELETE
        ///FROM Users
        ///WHERE Id = @Id;.
        /// </summary>
        internal static string DeleteUser {
            get {
                return ResourceManager.GetString("DeleteUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT Id, UserId, RefreshToken
        ///FROM UserRefreshTokens
        ///WHERE RefreshToken = @Token;.
        /// </summary>
        internal static string FindRefreshToken {
            get {
                return ResourceManager.GetString("FindRefreshToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT Id, Name
        ///FROM Genres.
        /// </summary>
        internal static string GetAllGenres {
            get {
                return ResourceManager.GetString("GetAllGenres", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT Id, Name, AuthorId, IsDeleted
        ///FROM Playlists
        ///WHERE IsDeleted = 0.
        /// </summary>
        internal static string GetAllPlaylists {
            get {
                return ResourceManager.GetString("GetAllPlaylists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT Id,
        ///       UserId,
        ///       UploadedAt,
        ///       SizeBytes,
        ///       Name,
        ///       Path,
        ///       AuthorName, Year, GenreId, Duration, IsDeleted
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
        ///   Ищет локализованную строку, похожую на SELECT Id, Name
        ///FROM Genres
        ///WHERE Id = @Id.
        /// </summary>
        internal static string GetGenreById {
            get {
                return ResourceManager.GetString("GetGenreById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT
        ///    p.Id,
        ///    p.Name,
        ///    p.AuthorId,
        ///    p.IsDeleted,
        ///    ps.Id,
        ///    ps.UserId,
        ///    ps.UploadedAt,
        ///    ps.SizeBytes,
        ///    ps.Name,
        ///    ps.Path,
        ///    ps.AuthorName,
        ///    ps.Year,
        ///    ps.GenreId,
        ///    ps.Duration,
        ///    ps.IsDeleted,
        ///    ps.GenreId as Id,
        ///    ps.GenreName as Name
        ///FROM Playlists p
        ///LEFT JOIN
        ///    (
        ///        SELECT
        ///            s.Id,
        ///            s.UserId,
        ///            s.UploadedAt,
        ///            s.SizeBytes,
        ///            s.Name,
        ///            s.Path,
        ///            s.AuthorName [остаток строки не уместился]&quot;;.
        /// </summary>
        internal static string GetPlaylistById {
            get {
                return ResourceManager.GetString("GetPlaylistById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT p.Id,
        ///       p.Name,
        ///       p.AuthorId,
        ///       p.IsDeleted,
        ///       ps.Id,
        ///       ps.UserId,
        ///       ps.UploadedAt,
        ///       ps.SizeBytes,
        ///       ps.Name,
        ///       ps.Path,
        ///       ps.AuthorName,
        ///       ps.Year,
        ///       ps.GenreId,
        ///       ps.Duration,
        ///       ps.IsDeleted
        ///FROM Playlists p
        ///LEFT JOIN
        ///    (
        ///        SELECT
        ///            s.Id,
        ///            s.UserId,
        ///            s.UploadedAt,
        ///            s.SizeBytes,
        ///            s.Name,
        ///            s.Path,
        ///            s.AuthorName,
        ///           [остаток строки не уместился]&quot;;.
        /// </summary>
        internal static string GetPlaylistsCreatedByUserId {
            get {
                return ResourceManager.GetString("GetPlaylistsCreatedByUserId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT Id, Name, NormalizedName
        ///FROM Roles
        ///WHERE Id = @Id;.
        /// </summary>
        internal static string GetRoleById {
            get {
                return ResourceManager.GetString("GetRoleById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT Id, Name, NormalizedName
        ///FROM Roles
        ///WHERE NormalizedName = @NormalizedName;.
        /// </summary>
        internal static string GetRoleByName {
            get {
                return ResourceManager.GetString("GetRoleByName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT Roles.Id, Roles.Name, Roles.NormalizedName
        ///FROM Roles
        ///         INNER JOIN UserRoles ON UserRoles.RoleId = Roles.Id
        ///         INNER JOIN Users ON UserRoles.UserId = Users.Id
        ///WHERE Users.Id = @UserId;.
        /// </summary>
        internal static string GetRoles {
            get {
                return ResourceManager.GetString("GetRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT Id,
        ///       UserId,
        ///       UploadedAt,
        ///       SizeBytes,
        ///       Name,
        ///       Path,
        ///       AuthorName,
        ///       Year,
        ///       GenreId,
        ///       Duration,
        ///       IsDeleted
        ///FROM Songs
        ///WHERE Id = @Id
        ///  AND IsDeleted = 0
        ///.
        /// </summary>
        internal static string GetSongById {
            get {
                return ResourceManager.GetString("GetSongById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на .
        /// </summary>
        internal static string GetSongs {
            get {
                return ResourceManager.GetString("GetSongs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT Songs.Id,
        ///       UserId,
        ///       UploadedAt,
        ///       SizeBytes,
        ///       Songs.Name,
        ///       Path,
        ///       AuthorName,
        ///       Year,
        ///       GenreId,
        ///       Duration,
        ///       IsDeleted,
        ///       Genres.Id,
        ///       Genres.Name
        ///FROM Songs
        ///    INNER JOIN Genres
        ///ON Songs.GenreId = Genres.Id
        ///WHERE IsDeleted = 0 AND UserId = @UserId.
        /// </summary>
        internal static string GetSongsUploadedByUserId {
            get {
                return ResourceManager.GetString("GetSongsUploadedByUserId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT Id,
        ///       UserName,
        ///       NormalizedUserName,
        ///       Email,
        ///       NormalizedEmail,
        ///       EmailConfirmed,
        ///       PasswordHash,
        ///       PhoneNumber,
        ///       IsBanned,
        ///       IsDeleted
        ///FROM Users
        ///WHERE NormalizedEmail = @NormalizedEmail.
        /// </summary>
        internal static string GetUserByEmail {
            get {
                return ResourceManager.GetString("GetUserByEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT Id,
        ///       UserName,
        ///       NormalizedUserName,
        ///       Email,
        ///       NormalizedEmail,
        ///       EmailConfirmed,
        ///       PasswordHash,
        ///       PhoneNumber,
        ///       IsBanned,
        ///       IsDeleted
        ///FROM Users
        ///WHERE Id = @Id;.
        /// </summary>
        internal static string GetUserById {
            get {
                return ResourceManager.GetString("GetUserById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT Id,
        ///       UserName,
        ///       NormalizedUserName,
        ///       Email,
        ///       NormalizedEmail,
        ///       EmailConfirmed,
        ///       PasswordHash,
        ///       PhoneNumber,
        ///       IsBanned,
        ///       IsDeleted
        ///FROM Users
        ///WHERE NormalizedUserName = @NormalizedUserName;.
        /// </summary>
        internal static string GetUserByName {
            get {
                return ResourceManager.GetString("GetUserByName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT UserId, RoleId
        ///FROM UserRoles
        ///WHERE UserId = @UserId
        ///  AND RoleId = @RoleId;.
        /// </summary>
        internal static string GetUserRoles {
            get {
                return ResourceManager.GetString("GetUserRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT Users.Id,
        ///       Users.UserName,
        ///       Users.NormalizedUserName,
        ///       Users.Email,
        ///       Users.NormalizedEmail,
        ///       Users.EmailConfirmed,
        ///       Users.PasswordHash,
        ///       Users.PhoneNumber,
        ///       Users.IsBanned,
        ///       Users.IsDeleted
        ///FROM Users
        ///         INNER JOIN Roles ON Users.RoleId = Roles.Id
        ///WHERE Roles.NormalizedName = @NormalizedName;.
        /// </summary>
        internal static string GetUsersInRole {
            get {
                return ResourceManager.GetString("GetUsersInRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на SELECT SUM(SizeBytes)
        ///FROM Songs
        ///WHERE UserId = @UserId
        ///  AND IsDeleted = 0.
        /// </summary>
        internal static string GetUserTotalUploadsSize {
            get {
                return ResourceManager.GetString("GetUserTotalUploadsSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на INSERT INTO PlaylistSongs (PlaylistId, SongId)
        ///VALUES (@PlaylistId, @SongId).
        /// </summary>
        internal static string InsertPlaylistSong {
            get {
                return ResourceManager.GetString("InsertPlaylistSong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на INSERT INTO UserRoles (UserId, RoleId)
        ///VALUES (@UserId, @RoleId);.
        /// </summary>
        internal static string InsertRoles {
            get {
                return ResourceManager.GetString("InsertRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на UPDATE Playlists
        ///SET Name     = @Name,
        ///    Link     = @Link,
        ///    AuthorId = @AuthorId
        ///WHERE Id = @Id
        ///  AND IsDeleted = 0.
        /// </summary>
        internal static string UpdatePlaylist {
            get {
                return ResourceManager.GetString("UpdatePlaylist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на UPDATE UserRefreshTokens
        ///SET RefreshToken = @Token
        ///WHERE UserId = @Id;.
        /// </summary>
        internal static string UpdateRefreshToken {
            get {
                return ResourceManager.GetString("UpdateRefreshToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на UPDATE Roles
        ///SET Name           = @Name,
        ///    NormalizedName = @NormalizedName
        ///WHERE Id = @Id;.
        /// </summary>
        internal static string UpdateRole {
            get {
                return ResourceManager.GetString("UpdateRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на UPDATE Songs
        ///SET UserId     = @UserId,
        ///    Name       = @Name,
        ///    Path       = @Path,
        ///    AuthorName = @AuthorName,
        ///    Year       = @Year,
        ///    SizeBytes  = @SizeBytes,
        ///    UploadedAt = @UploadedAt,
        ///    GenreId    = @GenreId,
        ///    Duration   = @Duration,
        ///    WHERE Id = @Id
        ///        AND IsDeleted = 0.
        /// </summary>
        internal static string UpdateSong {
            get {
                return ResourceManager.GetString("UpdateSong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на UPDATE Users
        ///SET UserName           = @UserName,
        ///    NormalizedUserName = @NormalizedUserName,
        ///    Email              = @Email,
        ///    NormalizedEmail    = @NormalizedEmail,
        ///    EmailConfirmed     = @EmailConfirmed,
        ///    PasswordHash       = @PasswordHash,
        ///    PhoneNumber        = @PhoneNumber,
        ///    IsBanned           = @IsBanned,
        ///    IsDeleted          = @IsDeleted
        ///WHERE Id = @Id;.
        /// </summary>
        internal static string UpdateUser {
            get {
                return ResourceManager.GetString("UpdateUser", resourceCulture);
            }
        }
    }
}
